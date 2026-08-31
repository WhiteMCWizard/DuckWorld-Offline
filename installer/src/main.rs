use std::{
    env,
    fs::{self, File},
    io,
    path::{Path, PathBuf},
    process::Command,
    thread,
    time::Duration,
};

#[cfg(windows)]
use std::os::windows::ffi::OsStrExt;

#[cfg(windows)]
use windows::{
    core::PCWSTR,
    Win32::Foundation::HANDLE,
    Win32::Security::{GetTokenInformation, TokenElevation, TOKEN_ELEVATION, TOKEN_QUERY},
    Win32::System::Threading::{GetCurrentProcess, OpenProcessToken},
    Win32::UI::Shell::ShellExecuteW,
    Win32::UI::WindowsAndMessaging::SW_SHOWNORMAL,
};

const INSTALLER_URL: &str =
    "https://archive.org/download/duck-world-installer_202312/DuckWorld-installer.exe";

const DLL_URL: &str =
    "https://github.com/WhiteMCWizard/DuckWorld-Offline/releases/latest/download/Assembly-CSharp.dll";

const DOWNLOAD_TIMEOUT: Duration = Duration::from_secs(30 * 60);
const DOWNLOAD_ATTEMPTS: usize = 3;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    if env::consts::OS != "windows" {
        return Err("Unsupported OS: this installer is only for Windows.".into());
    }

    #[cfg(windows)]
    {
        if !is_elevated() {
            println!("Requesting administrator privileges...");
            relaunch_elevated();
        }
    }

    let temp = env::temp_dir().join("duckworld-offline-installer");
    fs::create_dir_all(&temp)?;

    let target = install_windows(&temp)?;

    println!("Downloading patched DLL...");
    let dll_path = temp.join("Assembly-CSharp.dll");
    download(DLL_URL, &dll_path)?;

    println!("Copying DLL...");
    fs::create_dir_all(&target)?;
    fs::copy(&dll_path, target.join("Assembly-CSharp.dll"))?;

    let _ = fs::remove_dir_all(&temp);

    println!("Done!");

    // Keep the console window open so the user can see the result,
    // since a relaunched elevated process may open a new console.
    println!("Press Enter to exit...");
    let mut buf = String::new();
    let _ = io::stdin().read_line(&mut buf);

    Ok(())
}

#[cfg(windows)]
fn is_elevated() -> bool {
    unsafe {
        let mut token = HANDLE::default();
        if OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY, &mut token).is_err() {
            return false;
        }

        let mut elevation = TOKEN_ELEVATION::default();
        let mut size = 0u32;
        let ok = GetTokenInformation(
            token,
            TokenElevation,
            Some(&mut elevation as *mut _ as *mut _),
            std::mem::size_of::<TOKEN_ELEVATION>() as u32,
            &mut size,
        );

        ok.is_ok() && elevation.TokenIsElevated != 0
    }
}

#[cfg(windows)]
fn relaunch_elevated() -> ! {
    let exe = env::current_exe().expect("failed to get current exe path");
    let exe_wide: Vec<u16> = exe
        .as_os_str()
        .encode_wide()
        .chain(std::iter::once(0))
        .collect();
    let verb: Vec<u16> = "runas\0".encode_utf16().collect();

    unsafe {
        let result = ShellExecuteW(
            None,
            PCWSTR(verb.as_ptr()),
            PCWSTR(exe_wide.as_ptr()),
            PCWSTR::null(),
            PCWSTR::null(),
            SW_SHOWNORMAL,
        );

        // ShellExecuteW returns a value > 32 on success.
        if (result.0 as isize) <= 32 {
            eprintln!("Failed to relaunch with elevation (user may have declined UAC prompt).");
        }
    }

    std::process::exit(0);
}

fn install_windows(temp: &Path) -> Result<PathBuf, Box<dyn std::error::Error>> {
    let installer = temp.join("DuckWorld-installer.exe");

    println!("Downloading Windows installer...");
    download(INSTALLER_URL, &installer)?;

    println!("Running silent install...");
    run(Command::new(&installer).arg("/S"))?;

    Ok(PathBuf::from(
        r"C:\Program Files (x86)\DuckWorld\DuckWorld_Data\Managed",
    ))
}

fn download(url: &str, path: &Path) -> Result<(), Box<dyn std::error::Error>> {
    let client = reqwest::blocking::Client::builder()
        .timeout(DOWNLOAD_TIMEOUT)
        .connect_timeout(Duration::from_secs(30))
        .user_agent("duckworld-offline-installer/1.0")
        .build()?;

    let mut last_error = None;

    for attempt in 1..=DOWNLOAD_ATTEMPTS {
        match download_once(&client, url, path) {
            Ok(()) => return Ok(()),
            Err(error) => {
                last_error = Some(error.to_string());
                let _ = fs::remove_file(path);

                if attempt < DOWNLOAD_ATTEMPTS {
                    println!("Download failed, retrying ({attempt}/{DOWNLOAD_ATTEMPTS})...");
                    thread::sleep(Duration::from_secs(2 * attempt as u64));
                }
            }
        }
    }

    Err(format!(
        "Download failed after {DOWNLOAD_ATTEMPTS} attempts: {}",
        last_error.unwrap_or_else(|| "unknown error".to_string())
    )
    .into())
}

fn download_once(
    client: &reqwest::blocking::Client,
    url: &str,
    path: &Path,
) -> Result<(), Box<dyn std::error::Error>> {
    let mut response = client.get(url).send()?.error_for_status()?;
    let mut file = File::create(path)?;
    io::copy(&mut response, &mut file)?;
    Ok(())
}

fn run(cmd: &mut Command) -> Result<(), Box<dyn std::error::Error>> {
    let status = cmd.status()?;

    if !status.success() {
        return Err(format!("Command failed with status: {status}").into());
    }

    Ok(())
}