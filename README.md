# DuckWorld Offline

[![Join Discord](https://img.shields.io/badge/Discord-Join%20Server-5865F2?logo=discord\&logoColor=white)](https://discord.gg/wYDtsw8yBr)

Een offline-speelbare versie van **DuckWorld**, een Unity-spel voor kinderen dat oorspronkelijk door Sanoma werd uitgebracht in 2015. De servers werden op 2 september 2021 afgesloten. Het originele spel vereiste een permanente internetverbinding voor authenticatie, opgeslagen spelvoortgang, de winkel, achievements en analytics. Dit project vervangt al die serverafhankelijkheden door lokale opslag, zodat het spel weer volledig speelbaar is zonder internetverbinding.

---

## Disclaimer

Dit project is uitsluitend bedoeld voor preservatiedoeleinden. Alle originele game-assets, handelsmerken en intellectueel eigendom zijn en blijven eigendom van de respectieve rechthebbenden. Deze repository bevat alleen gedecompileerde en aangepaste C#-code, er zijn geen originele game-assets inbegrepen.

Ben je rechthebbende en heb je bezwaar tegen dit project? Neem dan contact op via **[takedown@whitemcwizard.nl](mailto:takedown@whitemcwizard.nl)**, dan wordt het zo snel mogelijk verwijderd.

---

# Installatie

## Automatische installatie (aanbevolen)

Gebruik de installer of het script voor jouw platform.

### Windows

Download en start de installer:

[DuckWorld-Offline-Installer.exe](https://github.com/WhiteMCWizard/DuckWorld-Offline/releases/latest/download/DuckWorld-Offline-Installer.exe)

### Linux / macOS

```bash
curl -fsSL https://raw.githubusercontent.com/WhiteMCWizard/DuckWorld-Offline/refs/heads/main/install.sh | bash
```
---

## Handmatige installatie

Gebruik deze methode alleen als je de automatische installer niet wilt gebruiken.

---

### 1. Het spel downloaden

Je hebt een kopie van het originele DuckWorld nodig. Het spel is niet meer te koop, maar is gearchiveerd op Internet Archive:

| Platform | Download                                                                                                    |
| -------- | ----------------------------------------------------------------------------------------------------------- |
| Windows  | [DuckWorld-installer.exe](https://archive.org/download/duck-world-installer_202312/DuckWorld-installer.exe) |
| macOS    | [DuckWorld.app.dmg](https://archive.org/download/duck-world-installer_202312/DuckWorld.app.dmg)             |
| Linux    | [DuckWorld.deb](https://archive.org/download/duck-world-installer_202312/DuckWorld.deb)                     |

> **Let op:** De Windows-versie is het meest stabiel. Op Linux wordt aangeraden deze via Wine te draaien in plaats van het `.deb`-pakket.

---

### 2. DLL verkrijgen

Download de DLL via de [Releases-pagina](https://github.com/WhiteMCWizard/DuckWorld-Offline/releases), of bouw hem zelf.

#### Zelf bouwen

**Vereisten**

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) of nieuwer

```bash
dotnet restore ./Assembly-CSharp.csproj
dotnet build ./Assembly-CSharp.csproj --configuration Release
```

Output:

```
bin/Release/net35/Assembly-CSharp.dll
```

---

### 3. Installeren

1. **Ga naar de DuckWorld installatiemap → `Managed` map:**

   * Windows: `DuckWorld_Data/Managed/`
   * macOS: `DuckWorld.app/Contents/Resources/Data/Managed/`
   * Linux: `DuckWorld_Data/Managed/`

2. **Maak een back-up:**

   ```bash
   cp Assembly-CSharp.dll Assembly-CSharp.dll.original
   ```

3. **Vervang de DLL:**
   Kopieer jouw nieuwe `Assembly-CSharp.dll` naar deze map.

4. **Start het spel**

---

### Terugzetten

```bash
mv Assembly-CSharp.dll.original Assembly-CSharp.dll
```

---

## Opslagsysteem

### Bestandslocatie

Saves worden opgeslagen in Unity's `Application.persistentDataPath`:

| Platform | Pad                                                          |
| -------- | ------------------------------------------------------------ |
| Windows  | `%APPDATA%/../LocalLow/<CompanyName>/<ProductName>/`         |
| macOS    | `~/Library/Application Support/<CompanyName>/<ProductName>/` |
| Linux    | `~/.config/unity3d/<CompanyName>/<ProductName>/`             |

---

### Bestanden

| Bestand              | Beschrijving        |
| -------------------- | ------------------- |
| `savegame.dat`       | Hoofdopslagbestand  |
| `savegame.dat.bak`   | Back-up             |
| `avatar_mugshot.png` | Profielfoto         |
| `ghosts/ghost_*.dat` | Ghost data          |
| `player.log`         | Debuglog (max 1 MB) |
