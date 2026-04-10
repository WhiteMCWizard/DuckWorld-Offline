# DuckWorld Offline

Een offline-speelbare versie van **DuckWorld**, een Unity-gebaseerd kinderspel oorspronkelijk ontwikkeld en uitgegeven door Sanoma in 2015. De servers zijn rond 2 september 2021 uitgezet. Het originele spel had een constante serververbinding nodig voor authenticatie, opgeslagen data, winkels, achievements en analytics. Dit project vervangt alle serverafhankelijkheden door lokale opslag, zodat het spel volledig speelbaar is zonder internetverbinding.

## Disclaimer

Dit project is een fan-made aanpassing bedoeld voor preservatie. Alle originele game-assets, handelsmerken en intellectueel eigendom behoren toe aan hun respectieve eigenaren. Deze repository bevat alleen gedecompileerde en aangepaste C#-code — er zijn geen originele game-assets opgenomen.

Als je een rechthebbende bent en bezwaren hebt tegen dit project, neem dan contact op met **takedown@whitemcwizard.nl** en het wordt zo snel mogelijk verwijderd.

## Het spel verkrijgen

Deze mod vereist een kopie van het originele DuckWorld-spel. Het spel wordt niet meer verkocht, maar is gearchiveerd:

| Platform | Download |
|----------|----------|
| Windows | [DuckWorld-installer.exe](https://archive.org/download/duck-world-installer_202312/DuckWorld-installer.exe) |
| macOS | [DuckWorld.app.dmg](https://archive.org/download/duck-world-installer_202312/DuckWorld.app.dmg) |
| Linux | [DuckWorld.deb](https://archive.org/download/duck-world-installer_202312/DuckWorld.deb) |

> **Opmerking:** De Windows-versie werkt over het algemeen het beste. Op Linux wordt aangeraden om de Windows-versie via Wine te draaien in plaats van het native `.deb`-pakket.

## Functies

- **Volledig offline speelbaar** — Alle minigames, locaties en voortgang werken zonder server
- **Lokaal opslagsysteem** — Spelvoortgang, avatar, kartconfiguratie, achievements en aankopen worden automatisch op schijf opgeslagen
- **Winkelsysteem** — Kledingwinkel en kartwinkel met volledig lokaal assortiment en portemonnee-beheer
- **Achievement-tracking** — Alle singleplayer-achievements worden lokaal bijgehouden
- **Ghost-opnames** — Kartrace ghost-replays worden lokaal opgeslagen en geladen
- **Auto-save** — Spelstatus wordt elke 20 seconden en bij afsluiten opgeslagen, met automatische back-ups

## Bouwen

### Vereisten

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) of nieuwer

### De DLL bouwen

```bash
dotnet restore ./Assembly-CSharp.csproj
dotnet build ./Assembly-CSharp.csproj --configuration Release
```

De gecompileerde DLL staat dan op:
```
bin/Release/net35/Assembly-CSharp.dll
```

Er is ook een GitHub Actions workflow (`.github/workflows/build.yml`) die automatisch bouwt bij elke push en de DLL als artifact uploadt — je kunt het downloaden via de Actions-tab zonder zelf te bouwen.

### Installeren in het spel

1. **Zoek de installatiemap van DuckWorld.** Zoek de `Managed`-map in de data-directory van het spel:
   - **Windows:** `DuckWorld_Data/Managed/`
   - **macOS:** `DuckWorld.app/Contents/Data/Managed/`
   - **Linux:** `DuckWorld_Data/Managed/`

2. **Maak een back-up van de originele DLL:**
   ```
   cp Managed/Assembly-CSharp.dll Managed/Assembly-CSharp.dll.original
   ```

3. **Vervang de DLL:**
   Kopieer de gebouwde `Assembly-CSharp.dll` naar de `Managed/`-map, ter vervanging van het origineel.

4. **Start het spel.** Het draait nu volledig offline — geen serververbinding nodig. Je bestaande opgeslagen data is compatibel.

> **Opmerking:** Als je ooit terug wilt, herstel dan simpelweg `Assembly-CSharp.dll.original`.

## Projectstructuur

```
DuckWorld-Offline/
├── Assembly-CSharp.csproj    # .NET projectbestand (target net35 voor Unity-compatibiliteit)
├── lib/                      # Unity- en dependency-DLL's
├── SLAM/                     # Kerncode van het spel
│   ├── Achievements/         # Achievement-tracking systeem
│   ├── Analytics/            # Tracking-stubs (offline uitgeschakeld)
│   ├── Avatar/               # Avatar-aanpassingssysteem
│   ├── BuildSystem/          # Asset bundle laden
│   ├── Engine/               # Core game controller, views, constanten
│   ├── Hub/                  # Hub-wereld (hoofdmenu/overworld)
│   ├── Kart/                 # Kartconfiguratie en racen
│   ├── KartRacing/           # Kartrace-minigame + ghost-systeem
│   ├── Kartshop/             # Kartwinkel UI en logica
│   ├── SaveSystem/           # Save manager, serialisatie, datatypes
│   ├── Shops/                # Kledingwinkel UI en logica
│   ├── Smartphone/           # In-game smartphone UI
│   ├── Webservices/          # Datatypes en stubs (bewaard voor save-compatibiliteit)
│   └── ...                   # Overige minigame-modules
├── GameData/                 # Speldefinities (winkels, locaties, items)
├── Components/               # Herbruikbare MonoBehaviours (audio, spawning, rendering)
├── MiniGameExtras/           # Minigame-specifieke scripts
├── Utilities/                # Hulpklassen, extensies, singletons, logging
├── ThirdParty/
│   ├── NGUI/                 # NGUI UI-framework + tweens
│   └── AudioToolkit/         # ClockStone Audio Toolkit + object pooling
├── AnimationOrTween/         # Animatie/tween enums
├── CinemaDirector/           # Cutscène-director systeem
├── LitJson/                  # JSON-serialisatiebibliotheek
└── System/                   # Systeem-hulpklassen
```

## Opslagsysteem

### Bestandslocatie

Opgeslagen bestanden staan in Unity's `Application.persistentDataPath`:

| Platform | Pad |
|----------|-----|
| Windows  | `%APPDATA%/../LocalLow/<CompanyName>/<ProductName>/` |
| macOS    | `~/Library/Application Support/<CompanyName>/<ProductName>/` |
| Linux    | `~/.config/unity3d/<CompanyName>/<ProductName>/` |

### Bestanden

| Bestand | Beschrijving |
|---------|-------------|
| `savegame.dat` | Hoofdopslagbestand (binair formaat) |
| `savegame.dat.bak` | Automatische back-up van vorige opslag |
| `avatar_mugshot.png` | Speler-avatar foto |
| `ghosts/ghost_*.dat` | Kartrace ghost-opnames |
| `player.log` | Debug-logbestand (geroteerd, max 1 MB) |

### Opgeslagen data

Het opslagbestand bevat:
- **Spelerprofiel** — Naam, avatarconfiguratie, pasfoto
- **Spelvoortgang** — Scores per spel, tijden, voltooiingsstatus, vrijgespeelde levels
- **Achievements** — Voortgang en voltooiingsstatus van alle achievements
- **Aankopen** — Gekochte kleding- en kartitems
- **Portemonnee** — In-game valutasaldo
- **Kartconfiguratie** — Aangepaste kartopstelling
- **Berichten** — In-game berichten

### Compatibiliteit

Opslagbestanden zijn voorzien van versienummers. Nieuwe versies van de mod kunnen oude bestanden lezen — ontbrekende velden krijgen veilige standaardwaarden. Het spel maakt automatisch een back-up (`savegame.dat.bak`) voor elke opslag.

## Technische notities

### Namespace-beperking

Types in de `SLAM.Webservices`-namespace (bijv. `UserProfile`, `ShopItemData`, `Message`) **mogen niet hernoemd of verplaatst worden** naar een andere namespace. Het opslagsysteem gebruikt .NET's `BinaryFormatter`, die de volledige typenaam inclusief namespace in het binaire bestand opslaat. Het wijzigen van namespaces zou alle bestaande opslagbestanden onbruikbaar maken.

### Wat is verwijderd

De volgende serverafhankelijke functies zijn uitgeschakeld of vervangen door stubs:
- **Authenticatie** — Geen login nodig; het spel start direct
- **Multiplayer** — Vriendenuitdagingen, uitnodigingen en ranglijsten zijn niet beschikbaar
- **Analytics** — Google Analytics en server-side telemetrie zijn uitgeschakeld
- **Cloud-opslag** — Alle data wordt alleen lokaal opgeslagen
- **Updatesysteem** — Automatische versiecontrole is uitgeschakeld
- **Sommige achievements** — Achievements die multiplayer vereisen (vrienden, uitdagingen) zijn verborgen

### Samenvatting offline-aanpassingen

Het originele spel communiceerde met `duckworld.com`-API's voor alle opslag. Dit project:

1. Heeft een `SaveManager` toegevoegd met `BinaryFormatter`-gebaseerde lokale opslag
2. Heeft alle `ApiClient`-aanroepen vervangen door lokale `SaveData`-bewerkingen
3. Heeft winkelassortimenten (`FashionStoreItems.cs`, `KartshopItems.cs`) en locatiedata (`Locations.cs`) hardcoded
4. Heeft lokale portemonnee, aankoopregistratie en achievement-beheer geïmplementeerd
5. Heeft ghost-opname opslaan/laden voor kartrace-replays toegevoegd
6. Heeft analytics, authenticatie en web-foutafhandeling verwijderd
