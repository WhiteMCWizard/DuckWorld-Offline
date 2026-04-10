# DuckWorld Offline

Een offline-speelbare versie van **DuckWorld**, een Unity-spel voor kinderen dat oorspronkelijk door Sanoma werd uitgebracht in 2015. De servers werden op 2 september 2021 afgesloten. Het originele spel vereiste een permanente internetverbinding voor authenticatie, opgeslagen spelvoortgang, de winkel, achievements en analytics. Dit project vervangt al die serverafhankelijkheden door lokale opslag, zodat het spel weer volledig speelbaar is zonder internetverbinding.

## Disclaimer

Dit project is uitsluitend bedoeld voor preservatiedoeleinden. Alle originele game-assets, handelsmerken en intellectueel eigendom zijn en blijven eigendom van de respectieve rechthebbenden. Deze repository bevat alleen gedecompileerde en aangepaste C#-code, er zijn geen originele game-assets inbegrepen.

Ben je rechthebbende en heb je bezwaar tegen dit project? Neem dan contact op via **takedown@whitemcwizard.nl**, dan wordt het zo snel mogelijk verwijderd.

## Het spel downloaden

Je hebt een kopie van het originele DuckWorld nodig. Het spel is niet meer te koop, maar is gearchiveerd op Internet Archive:

| Platform | Download |
|----------|----------|
| Windows | [DuckWorld-installer.exe](https://archive.org/download/duck-world-installer_202312/DuckWorld-installer.exe) |
| macOS | [DuckWorld.app.dmg](https://archive.org/download/duck-world-installer_202312/DuckWorld.app.dmg) |
| Linux | [DuckWorld.deb](https://archive.org/download/duck-world-installer_202312/DuckWorld.deb) |

> **Let op:** De Windows-versie is over het algemeen het meest stabiel. Op Linux wordt aangeraden de Windows-versie via Wine te draaien in plaats van het native `.deb`-pakket te gebruiken.

## De DLL verkrijgen

Je kunt de gecompileerde DLL direct downloaden via de [Releases-pagina](https://github.com/WhiteMCWizard/DuckWorld-Offline/releases), of hem zelf bouwen aan de hand van de onderstaande instructies.

### Zelf bouwen

**Vereisten**

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) of nieuwer

**Bouwen**

```bash
dotnet restore ./Assembly-CSharp.csproj
dotnet build ./Assembly-CSharp.csproj --configuration Release
```

De gecompileerde DLL is daarna te vinden op:
```
bin/Release/net35/Assembly-CSharp.dll
```

## Installeren

1. **Zoek de installatiemap van DuckWorld** en open de `Managed`-map in de data-directory:
   - **Windows:** `DuckWorld_Data/Managed/`
   - **macOS:** `DuckWorld.app/Contents/Data/Managed/`
   - **Linux:** `DuckWorld_Data/Managed/`

2. **Maak een back-up van de originele DLL:**
   ```
   cp Managed/Assembly-CSharp.dll Managed/Assembly-CSharp.dll.original
   ```

3. **Vervang de DLL:**
   Kopieer de gedownloade of gebouwde `Assembly-CSharp.dll` naar de `Managed`-map ter vervanging van het origineel.

4. **Start het spel.** Het draait nu volledig offline — er is geen internetverbinding meer nodig. Bestaande saves blijven gewoon werken.

> **Terugzetten:** Wil je terugkeren naar de originele versie? Zet dan simpelweg `Assembly-CSharp.dll.original` terug als `Assembly-CSharp.dll`.

## Opslagsysteem

### Bestandslocatie

Saves worden opgeslagen in Unity's `Application.persistentDataPath`:

| Platform | Pad |
|----------|-----|
| Windows  | `%APPDATA%/../LocalLow/<CompanyName>/<ProductName>/` |
| macOS    | `~/Library/Application Support/<CompanyName>/<ProductName>/` |
| Linux    | `~/.config/unity3d/<CompanyName>/<ProductName>/` |

### Bestanden

| Bestand | Beschrijving |
|---------|-------------|
| `savegame.dat` | Het hoofdopslagbestand (binair formaat) |
| `savegame.dat.bak` | Automatische back-up van de vorige opslag |
| `avatar_mugshot.png` | Profielfoto van de speler |
| `ghosts/ghost_*.dat` | Ghost-opnames van kartraces |
| `player.log` | Debuglog (geroteerd, maximaal 1 MB) |