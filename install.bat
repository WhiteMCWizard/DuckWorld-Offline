@echo off
setlocal

set INSTALL_DIR=C:\Program Files (x86)\DuckWorld
set TARGET=%INSTALL_DIR%\DuckWorld_Data\Managed

echo Downloading installer...
curl -L -o DuckWorld-installer.exe https://archive.org/download/duck-world-installer_202312/DuckWorld-installer.exe

echo Running silent install...
DuckWorld-installer.exe /S

echo Downloading patched DLL...
curl -L -o Assembly-CSharp.dll https://github.com/WhiteMCWizard/DuckWorld-Offline/releases/download/build-20260410163607-9294c60/Assembly-CSharp.dll

echo Copying DLL...
copy /Y Assembly-CSharp.dll "%TARGET%\Assembly-CSharp.dll"

echo Done!
pause
