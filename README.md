# B-ALKFET Hero App

Ez a repository egy egyszeru full-stack mintaalkalmazast tartalmaz:
- frontend: Angular 21
- backend: ASP.NET Core minimal API
- adatbazis: MongoDB

Az alkalmazas egy egyszeru `Hero` domain modellt kezel teljes CRUD funkcionalitassal.

## Fobb funkciok
- hosok listazasa
- lapozas a listaoldalon
- uj hos letrehozasa
- hos szerkesztese
- hos torlese
- backend CRUD API kiprobalasa `.http` fajllal

## Projekt szerkezet
- `source/WebUI`: Angular frontend
- `source/WebApi`: ASP.NET Core backend
- `source/DataAccess`: MongoDB kapcsolat
- `source/Domain`: kozos domain modellek
- `source/WebApiTests`: backend tesztek
- `.github/workflows`: Docker image build es push workflow-k

## Lokal fejlesztoi futtatas

### 1. MongoDB inditasa Dockerrel
```powershell
docker run -d --name balkfet-mongodb -p 27017:27017 mongo:8
```

### 2. Backend inditasa
```powershell
cd source/WebApi
$env:MongoDb__ConneX="mongodb://localhost:27017"
dotnet run
```

Alapertelmezett fejlesztoi URL: `http://localhost:5000`

### 3. Frontend inditasa
```powershell
cd source/WebUI
npm install
npm start -- --port 4300
```

Alapertelmezett fejlesztoi URL: `http://localhost:4300`

## Hasznalat

### Hero lista
Az alkalmazas nyitooldala a hero lista. Itt:
- megjelennek a backendbol betoltott hosok
- az elemekre kattintva megnyilik a szerkeszto panel
- a `Previous` es `Next` gombokkal lehet lapozni

### Hero letrehozas
A `Hero Creator` oldalon egy uj nev megadasaval uj hero hozhato letre. Letrehozas utan vissza lehet lepni a listara, ahol az uj elem mar megjelenik.

### Hero szerkesztes es torles
A listaoldalon kivalasztott hero a szerkeszto panelben modosithato:
- `Save`: modositas mentese
- `Delete`: torles
- `Cancel`: szerkesztes bezarasa

## Backend API

A backend a kovetkezo endpointokat adja:
- `GET /hero`
- `GET /hero/{id}`
- `POST /hero`
- `PUT /hero/{id}`
- `DELETE /hero/{id}`

Kezi API teszteleshez hasznald a `source/WebApi/WebApi.http` fajlt.

## Docker image build

Mindket modulhoz van kulon Dockerfile:
- `source/WebApi/Dockerfile`
- `source/WebUI/Dockerfile`

Lokalis build:
```powershell
pwsh ./tools/docker_build_all.ps1
```

## CI workflow

A repository kulon GitHub Actions workflow-kat tartalmaz:
- `.github/workflows/ci-docker-build-api.yml`
- `.github/workflows/ci-docker-build-ui.yml`

Ezek push es manualis futtatas eseten:
- buildelik a backend vagy frontend image-et
- feltoltik azt `ghcr.io` registry-be
- provenance attestaciot is generalnak
