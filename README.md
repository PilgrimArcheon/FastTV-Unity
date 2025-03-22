# FastTV-Unity

# Movie Exploration App

## Overview
This is a Unity-based Android application that allows users to search for movies, view detailed movie information, and explore movie posters using **The Movie Database (TMDb) API**. The app is designed with a responsive UI, smooth transitions, and offline caching for better user experience.

## Features
- **Movie Search**: Users can search for movies by title.
- **Movie Details**: Click on a movie to see its synopsis, rating, genres, and poster.
- **Poster Display**: High-quality movie posters are fetched dynamically.
- **Smooth UI/UX**: Animations and transitions enhance user experience.
- **Offline Mode**: Recent searches are stored for offline access.
- **Splash Screen**: Displays an animated introduction.
- **Error Handling**: Handles network failures and invalid API keys.

---

## Setup Instructions

### 1. Clone the Repository
```sh
git clone https://github.com/PilgrimArcheon/FastTV-Unity/.git
cd fast-tv-app
```

### 2. Open in Unity
- Open **Unity Hub**.
- Click **Add Project** and select the cloned folder.
- Open the project in Unity.

### 3. Install Dependencies
Ensure you have the following Unity packages installed:
- **UnityWebRequest** (for API requests)
- **TextMeshPro** (for UI text rendering)

### 4. Get a TMDb API Key
- Sign up at [The Movie Database](https://www.themoviedb.org/).
- Go to your **API settings** and generate an API key.

### 5. Enter API Key in Unity
- Launch the app.
- On the first run, enter your **TMDb API Key** when prompted.

### 6. Build and Run on Android
- Connect an Android device or use an emulator.
- Go to **File > Build Settings**.
- Select **Android** and click **Build & Run**.

---

## Architecture Overview

The app follows a **Model-View-Controller (MVC)** pattern for better separation of concerns:

### **1. Model (Data Handling)**
- `Movie.cs` → Stores movie details (title, overview, poster URL, etc.).
- `MovieList.cs` → Holds a list of movies from search results.

### **2. View (UI Components)**
- `SplashScreen.unity` → Displays app intro.
- `MainScene.unity` → Displays search bar and results.
- `Search/MainScreen` → Shows detailed movies information and search bar.
- `DetailScreen` → Shows detailed movie information.
  
### **3. Controller (Logic, UI & API Requests)**
- `MovieAPI.cs` → Handles API requests and responses.
- `MovieSearchController.cs` → Handles Main Search Functions and shows search results using Movie UI Items.
- `MovieDetailsController.cs` → Displaying movie information details on Movie Item Click.
- `APIResponseCache.cs` → Stores past search results for offline access.
- `DoTween & UIContentTween.cs` → Controls UI transitions and animations.
- `Movie.cs & MovieItem.cs` → Represents a movie data and its Searched Movie UI item.

---

## Design Decisions & Trade-offs

### **1. Using UnityWebRequest vs. External API Libraries**
- **Decision**: Used `UnityWebRequest` instead of external plugins.
- **Trade-off**: Simpler setup but requires manual JSON handling.

### **2. Caching with PlayerPrefs vs. Local Database**
- **Decision**: Used `PlayerPrefs` & `Application.Persistent` for quick storage.
- **Trade-off**: Limited storage; SQLite would be better for large-scale offline support.

### **3. UI Design & Responsiveness**
- **Decision**: Used Unity’s Canvas with anchors and auto-layout to allow for Portrait and Landscape Mode & Resolution.
- **Trade-off**: Scaling works well but can be challenging across different screen sizes.

### **4. CI/CD with GitHub Actions**
- **Decision**: Integrated CI/CD pipeline for building APKs automatically.
- **Trade-off**: Slower initial setup but ensures reliability and automation.

---

## Known Issues & Possible Improvements

### **Known Issues**
- **Large API responses take time**: Large search results might slow down loading.
- **Limited offline data**: Only past searches are stored; full movie details require an internet connection.
- **Animations may lag on low-end devices**: Can be optimized further.

### **Possible Improvements**
- **Use SQLite for better offline storage**: Store entire movie details instead of just search results.
- **Include Trailers**: Fetch and play movie trailers using TMDb’s video API.
- **Improve search suggestions**: Implement predictive search while typing.

---

## CI/CD Pipeline (GitHub Actions)
The project includes an automated **CI/CD pipeline** using **GitHub Actions** to:
1. **Build the project** automatically.
2. **Run unit tests** to ensure stability.
3. **Generate an APK** and attach it to a GitHub release.

#### **Pipeline YAML File (`.github/workflows/test-build.yml`)**
```yaml
name: Build and Deploy APK

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout Code
        uses: actions/checkout@v2
      
      - name: Set Up Unity
        uses: game-ci/unity-builder@v2
        with:
          unityVersion: 2021.3.11f1
      
      - name: Build APK
        run: |
          unity-editor -projectPath . -buildTarget Android -executeMethod BuildScript.PerformBuild
      
      - name: Upload APK
        uses: actions/upload-artifact@v2
        with:
          name: MovieApp.apk
          path: Build/Android/*.apk
```
