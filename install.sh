#!/bin/bash
set -e

# -------------------------
# Linux
# -------------------------
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    if ! command -v wine &> /dev/null; then
        echo "Wine not installed. Install it first."
        exit 1
    fi

    echo "Downloading Windows installer..."
    curl -L -o DuckWorld-installer.exe https://archive.org/download/duck-world-installer_202312/DuckWorld-installer.exe

    echo "Running silent install with Wine..."
    wine DuckWorld-installer.exe /S

    INSTALL_DIR="$HOME/.wine/drive_c/Program Files (x86)/DuckWorld"
    TARGET="$INSTALL_DIR/DuckWorld_Data/Managed"
fi

# -------------------------
# macOS
# -------------------------
if [[ "$OSTYPE" == "darwin"* ]]; then
    echo "Downloading macOS version..."
    curl -L -o DuckWorld.dmg https://archive.org/download/duck-world-installer_202312/DuckWorld.app.dmg

    hdiutil attach DuckWorld.dmg
    cp -R /Volumes/DuckWorld/DuckWorld.app /Applications/
    hdiutil detach /Volumes/DuckWorld

    TARGET="/Applications/DuckWorld.app/Contents/Resources/Data/Managed"
fi

# -------------------------
# DLL patch
# -------------------------
echo "Downloading patched DLL..."
curl -L -o Assembly-CSharp.dll https://github.com/WhiteMCWizard/DuckWorld-Offline/releases/download/build-20260410163607-9294c60/Assembly-CSharp.dll

echo "Copying DLL..."
mkdir -p "$TARGET"
cp Assembly-CSharp.dll "$TARGET/Assembly-CSharp.dll"

rm -f Assembly-CSharp.dll DuckWorld-installer.exe DuckWorld.dmg
echo "Done!"
