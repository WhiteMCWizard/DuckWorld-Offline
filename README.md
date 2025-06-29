# DuckWorld-Offline

## Introduction

DuckWorld-Offline is a community-driven project aimed at preserving and providing an offline version of the beloved DuckWorld game. DuckWorld was originally an online multiplayer game featuring Disney's duck characters in various mini-games and adventures. Unfortunately, the original servers have been permanently shut down, making the game inaccessible to players.

This repository contains the decompiled source code from the original Unity project, allowing the community to potentially create an offline version that can be enjoyed without requiring server connectivity.

## Important Disclaimer

⚠️ **This repository contains decompiled code from the original DuckWorld game.** 

- **This is NOT original source code** - All code in this repository has been decompiled from the original Assembly-CSharp.dll files
- **No ownership claimed** - We do not claim ownership of any original game code, assets, or intellectual property
- **Reverse engineering purposes** - This code has been reverse-engineered for educational and preservation purposes only
- **Disney property** - DuckWorld and all related characters, assets, and intellectual property belong to Disney

## Game Features

This offline version includes numerous mini-games and features from the original DuckWorld:

### Mini-Games
- **Kart Racing** - Race with Disney duck characters
- **Assembly Line** - Factory automation puzzle game
- **Bat Cave** - Platformer adventure game
- **Chase the Boat/Truck** - Action platformer games
- **Crane Operator** - Precision control game
- **Jump the Croc** - Timing-based platformer
- **Money Dive** - Scrooge McDuck's money bin diving game
- **Connect the Pipes** - Puzzle game
- **Zoo Transport** - Animal transportation game
- **Hangman** - Word guessing game
- **Duck Quiz** - Trivia game
- **Train Spotting** - Observation game
- And many more!

### Systems
- **Avatar Creation & Customization** - Create and customize your duck character
- **Achievement System** - Track progress and unlock achievements
- **Multiple Language Support** - Localization system included
- **Performance Settings** - Adjustable quality settings
- **Audio System** - Complete audio management

## Requirements & Dependencies

### System Requirements
- **Operating System**: Windows, macOS, or Linux
- **.NET Framework**: .NET Framework 3.5 or higher / .NET Core equivalent
- **Unity Engine**: Unity 2018.x or compatible version (for running/building)

### Dependencies
The project includes the following Unity and .NET dependencies in the `lib/` folder:
- `UnityEngine.dll` - Core Unity engine
- `Assembly-CSharp-firstpass.dll` - Unity first-pass assembly
- `System.Core.dll` - .NET System libraries
- Additional Unity modules for networking and UI

### Development Tools
- **Visual Studio** or **Visual Studio Code** - For code editing
- **Unity Editor** (recommended 2018.x) - For building and running the game
- **.NET SDK** - For compiling the C# code

## Setup & Installation

### For Players (Pre-built Game)
1. Download the latest release from the releases section
2. Extract the game files to your desired location
3. Run the executable file
4. Enjoy the offline DuckWorld experience!

### For Developers (Building from Source)
1. **Clone the repository**:
   ```bash
   git clone https://github.com/WhiteMCWizard/DuckWorld-Offline.git
   cd DuckWorld-Offline
   ```

2. **Open in Unity** (Recommended):
   - Install Unity Hub and Unity Editor (2018.x recommended)
   - Open the project folder in Unity
   - Build and run the project

3. **Compile as C# Project**:
   ```bash
   # Using .NET SDK
   dotnet build DuckWorld-Offline.sln
   
   # Or using MSBuild
   msbuild DuckWorld-Offline.sln
   ```

4. **Running the Game**:
   - If built through Unity: Run the generated executable
   - If compiled as library: Integration with Unity runtime required

## Project Structure

```
DuckWorld-Offline/
├── SLAM.*/                     # Game modules and systems
│   ├── SLAM.AssemblyLine/      # Assembly Line mini-game
│   ├── SLAM.BatCave/          # Bat Cave platformer
│   ├── SLAM.KartRacing/       # Kart racing game
│   ├── SLAM.Avatar/           # Avatar system
│   ├── SLAM.Webservices/      # Original web services (offline)
│   └── ...                    # Other game modules
├── lib/                       # Unity and .NET dependencies
├── Assembly-CSharp.csproj     # C# project file
├── DuckWorld-Offline.sln      # Visual Studio solution
└── README.md                  # This file
```

## Contributing

We welcome contributions from the community! However, please keep in mind:

- **Respect intellectual property** - Do not add copyrighted assets
- **Educational focus** - Contributions should focus on code preservation and education
- **No commercial use** - This project is for educational and preservation purposes only

### How to Contribute
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request with a clear description

## Legal & Educational Notice

This project is created and maintained for **educational and preservation purposes only**:

- **No Commercial Use** - This project must not be used for commercial purposes
- **Educational Tool** - Intended for learning game development and reverse engineering techniques
- **Preservation Effort** - Helps preserve gaming history and code for future study
- **Fair Use** - Created under educational fair use provisions
- **Respect IP Rights** - All original intellectual property rights remain with Disney

If you are a copyright holder and have concerns about this project, please contact the repository maintainers.

## Acknowledgments

- **Original Developers** - Thank you to the original DuckWorld development team
- **Disney** - For creating the beloved duck characters and universe
- **Community** - Thanks to all community members working on game preservation
- **Reverse Engineering Community** - For tools and techniques that made this preservation possible

## Disclaimer

This software is provided "as is" without warranty of any kind. The maintainers are not responsible for any issues arising from the use of this software. This is an unofficial fan project and is not affiliated with Disney or the original DuckWorld developers.

---

**Remember**: This project is for educational and preservation purposes only. Please respect intellectual property rights and use this code responsibly.