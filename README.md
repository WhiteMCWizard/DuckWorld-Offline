# DuckWorld-Offline

## Build Status

This project includes an automated GitHub Actions workflow that builds the .NET DLL from the source code.

### Automated Building

The workflow is triggered on:
- Pushes to the `main` branch
- Pull requests targeting the `main` branch

### Build Process

1. **Setup**: Uses Ubuntu latest runner with .NET SDK 8.0.x
2. **Restore**: Runs `dotnet restore DuckWorld-Offline.sln`
3. **Build**: Compiles the project with `dotnet build --configuration Release`
4. **Artifacts**: Uploads the generated DLL and PDB files as build artifacts

### Manual Building

To build the project locally:

```bash
# Restore dependencies
dotnet restore DuckWorld-Offline.sln

# Build the project (Debug)
dotnet build DuckWorld-Offline.sln

# Build the project (Release)
dotnet build DuckWorld-Offline.sln --configuration Release
```

The main output DLL will be located at `bin/Release/net35/Assembly-CSharp.dll` for Release builds.