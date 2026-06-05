use std::{
    env,
    fs::{self, File},
    io,
    path::{Path, PathBuf},
    process::Command,
    thread,
    time::Duration,
};

const INSTALLER_URL: &str =
    "https://archive.org/download/duck-world-installer_202312/DuckWorld-installer.exe";

const DLL_URL: &str =
    "https://github.com/WhiteMCWizard/DuckWorld-Offline/releases/latest/download/Assembly-CSharp.dll";

const DOWNLOAD_TIMEOUT: Duration = Duration::from_secs(30 * 60);
const DOWNLOAD_ATTEMPTS: usize = 3;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let temp = env::temp_dir().join("duckworld-offline-installer");
    fs::create_dir_all(&temp)?;

    if env::consts::OS != "windows" {
        return Err("Unsupported OS: this installer is only for Windows.".into());
    }

    let target = install_windows(&temp)?;

    println!("Downloading patched DLL...");
    let dll_path = temp.join("Assembly-CSharp.dll");
    download(DLL_URL, &dll_path)?;

    println!("Copying DLL...");
    fs::create_dir_all(&target)?;
    fs::copy(&dll_path, target.join("Assembly-CSharp.dll"))?;

    let _ = fs::remove_dir_all(&temp);

    println!("Done!");
    Ok(())
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
