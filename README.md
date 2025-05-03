# AndroidTV-Unity

Explore the latest APK release here: [Android-TV Download](https://github.com/PilgrimArcheon/AndroidTV-Unity/releases/)

---

## 📽️ Overview

**Android-TV** (Unity) is a sleek and responsive Android TV application built with **Unity**. It integrates **The Movie Database (TMDb) API** to let users search and explore movies, view high-quality posters, and read detailed movie information. The app is optimized for fluid navigation and includes offline caching for enhanced usability.

---

## 🚀 Features

- 🔍 **Search**: Find movies instantly by title.
- 🎬 **Details**: Access movie overviews, ratings, genres, and posters.
- 🖼️ **Poster Gallery**: Displays high-resolution movie posters.
- 🎨 **Smooth UI/UX**: Includes animations and screen transitions.
- 📴 **Offline Support**: Caches recent searches for offline use.
- 🚨 **Error Handling**: Detects invalid API keys and connection issues.
- 🎥 **Splash Screen**: Stylish animated app intro.

---

## 🛠️ Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/PilgrimArcheon/FastTV-Unity.git
cd fast-tv-app
```

### 2. Open in Unity
- Launch **Unity Hub**.
- Click **Add Project** and select the cloned directory.

### 3. Install Dependencies
Ensure these Unity packages are installed:
- `UnityWebRequest`
- `TextMeshPro`
- `DoTween`

### 4. Get a TMDb API Key
- Register at [The Movie Database](https://www.themoviedb.org/).
- Navigate to API settings and generate your key.

### 5. Enter API Key
- On first app launch, input your TMDb API key when prompted.

### 6. Build & Run on Android
- Connect your Android device.
- In Unity: **File > Build Settings > Android > Build & Run**.

---

## 🧱 App Architecture

The app follows an MVC structure:

### 🔹 Model
- `Movie.cs`: Contains movie metadata.
- `MovieList.cs`: Manages lists of search results.

### 🔹 View
- `SplashScreen.unity`: Startup animation.
- `MainScene.unity`: Search UI and results.
- `DetailScreen.unity`: Full movie details.

### 🔹 Controller
- `MovieAPI.cs`: API request/response logic.
- `MovieSearchController.cs`: Handles search and results UI.
- `MovieDetailsController.cs`: Displays selected movie data.
- `APIResponseCache.cs`: Caches data locally.
- `UIContentTween.cs`: Controls UI animations.

---

## 🎨 Design Decisions

### ✅ UnityWebRequest Over External Libraries
- **Why**: Simpler setup.
- **Trade-off**: Requires manual JSON handling.

### ✅ PlayerPrefs for Caching
- **Why**: Quick and easy offline access.
- **Trade-off**: Limited data storage. Consider SQLite for scalability.

### ✅ Responsive UI with Unity Canvas
- **Why**: Supports different orientations/resolutions.
- **Trade-off**: May require fine-tuning for TV resolutions.

### ✅ GitHub Actions for CI/CD
- **Why**: Automates builds and tests.
- **Trade-off**: Longer setup, but robust release cycle.

---

## ⚠️ Known Issues

- Large TMDb responses can delay results.
- Offline cache limited to recent searches.
- Older devices may show lag in transitions.

---

## 💡 Future Enhancements

- Integrate **SQLite** for richer offline storage.
- Add **movie trailers** using TMDb video API.
- Implement **auto-suggestions** during search input.

---

## 🔄 CI/CD Pipeline

GitHub Actions automates build, testing, and release:

### `.github/workflows/test-build.yml`
```yaml
name: Unit-Test-Build project

on: [push]

jobs:
  buildForAllSupportedPlatforms:
    name: Build for ${{ matrix.targetPlatform }}
    runs-on: ubuntu-latest
    strategy:
      matrix:
        targetPlatform:
          - Android
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0
          lfs: true
      - uses: actions/cache@v3
        with:
          path: Library
          key: Library-${{ matrix.targetPlatform }}
          restore-keys: Library-
      - if: matrix.targetPlatform == 'Android'
        uses: jlumbroso/free-disk-space@v1.3.1
      - uses: game-ci/unity-builder@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          targetPlatform: ${{ matrix.targetPlatform }}
      - uses: actions/upload-artifact@v4
        with:
          name: Android-TV-${{ matrix.targetPlatform }}
          path: build/${{ matrix.targetPlatform }}
```

---

## 👏 Contributions
All contributions are welcome. 
Please submit a pull request or open an issue on [GitHub](https://github.com/PilgrimArcheon/AndroidTV-Unity).

---
