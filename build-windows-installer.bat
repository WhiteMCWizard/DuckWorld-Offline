@echo off
setlocal

pushd "%~dp0"

where cargo >nul 2>nul
if errorlevel 1 (
    echo Rust/Cargo was not found. Install Rust from https://rustup.rs/ and run this script again.
    popd
    exit /b 1
)

echo Building Windows installer...
cargo build --manifest-path installer\Cargo.toml --release
if errorlevel 1 (
    popd
    exit /b 1
)

if not exist dist mkdir dist
copy /Y installer\target\release\duckworld-offline-installer.exe dist\DuckWorld-Offline-Installer.exe >nul

echo Built: %CD%\dist\DuckWorld-Offline-Installer.exe

popd
endlocal
