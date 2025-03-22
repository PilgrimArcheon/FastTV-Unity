using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MovieAPI : MonoBehaviour
{
    // Singleton instance of the MovieAPI class
    public static MovieAPI Instance;

    // Base URL for The Movie Database API
    private const string BASE_URL = "https://api.themoviedb.org/3";

    // API key for authentication
    private string apiKey;

    // Cache for storing API responses
    private APIResponseCache cache;

    // Called when the script is initialized
    private void Awake()
    {
        // Set the singleton instance
        Instance = this;

        // Load the saved API key
        apiKey = SaveManager.Instance.state.SavedApiKey;

        // Initialize the cache
        cache = new APIResponseCache();
    }

    // Retrieves a list of now playing movies from the API
    public IEnumerator GetMovies(Action<List<Movie>> callback)
    {
        // Construct the API URL
        string url = $"{BASE_URL}/movie/now_playing?language=en-US&page=1";

        // Check if the response is cached
        if (cache.TryGetResponse(url, out string cachedResponse))
        {
            // If cached, invoke the callback with the cached response
            callback?.Invoke(JsonUtility.FromJson<MovieSearchResponse>(cachedResponse).results);
            yield break;
        }

        // Create a UnityWebRequest to send a GET request to the API
        UnityWebRequest www = UnityWebRequest.Get(url);

        // Set the request headers
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Send the request and wait for the response
        yield return www.SendWebRequest();

        // Check if the response was successful
        if (www.result == UnityWebRequest.Result.Success)
        {
            // Get the response text
            string responseText = www.downloadHandler.text;

            // Store the response in the cache
            cache.StoreResponse(url, responseText);

            // Invoke the callback with the response
            callback?.Invoke(JsonUtility.FromJson<MovieSearchResponse>(responseText).results);
        }
        else
        {
            // Log an error if the response was not successful
            Debug.LogError($"API Error: {www.error}");

            // Invoke the callback with null
            callback?.Invoke(null);
        }
    }

    // Searches for movies by query from the API
    public IEnumerator SearchMovies(string query, Action<List<Movie>> callback)
    {
        // Construct the API URL
        string url = $"{BASE_URL}/search/movie?query={query}&include_adult=false&language=en-US&page=1";

        // Check if the response is cached
        if (cache.TryGetResponse(url, out string cachedResponse))
        {
            // If cached, invoke the callback with the cached response
            callback?.Invoke(JsonUtility.FromJson<MovieSearchResponse>(cachedResponse).results);
            yield break;
        }

        // Create a UnityWebRequest to send a GET request to the API
        UnityWebRequest www = UnityWebRequest.Get(url);

        // Set the request headers
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Send the request and wait for the response
        yield return www.SendWebRequest();

        // Check if the response was successful
        if (www.result == UnityWebRequest.Result.Success)
        {
            // Get the response text
            string responseText = www.downloadHandler.text;

            // Store the response in the cache
            cache.StoreResponse(url, responseText);

            // Invoke the callback with the response
            callback?.Invoke(JsonUtility.FromJson<MovieSearchResponse>(responseText).results);
        }
        else
        {
            // Log an error if the response was not successful
            Debug.LogError($"API Error: {www.error}");

            // Invoke the callback with null
            callback?.Invoke(null);
        }
    }

    // Retrieves the details of a movie by ID from the API
    public IEnumerator GetMovieDetails(int movieId, Action<MovieDetails> callback)
    {
        // Construct the API URL
        string url = $"{BASE_URL}/movie/{movieId}?language=en-US";

        // Check if the response is cached
        if (cache.TryGetResponse(url, out string cachedResponse))
        {
            // If cached, invoke the callback with the cached response
            callback?.Invoke(JsonUtility.FromJson<MovieDetails>(cachedResponse));
            yield break;
        }

        // Create a UnityWebRequest to send a GET request to the API
        UnityWebRequest www = UnityWebRequest.Get(url);

        // Set the request headers
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Send the request and wait for the response
        yield return www.SendWebRequest();

        // Check if the response was successful
        if (www.result == UnityWebRequest.Result.Success)
        {
            // Get the response text
            string responseText = www.downloadHandler.text;

            MovieDetails movieDetails = new MovieDetails();
            movieDetails = JsonUtility.FromJson<MovieDetails>(responseText);

            StartCoroutine(GetCastDetails(movieId, (castDetails) =>
            {
                movieDetails.cast = castDetails;
                // Store the response in the cache
                cache.StoreResponse(url, JsonUtility.ToJson(movieDetails));

                // Invoke the callback with the response
                callback?.Invoke(movieDetails);
            }));
        }
        else
        {
            // Log an error if the response was not successful
            Debug.LogError($"API Error: {www.error}");

            // Invoke the callback with null
            callback?.Invoke(null);
        }
    }

    public IEnumerator GetCastDetails(int movieId, Action<List<Cast>> callback)
    {
        // Construct the API URL
        string url = $"{BASE_URL}/movie/{movieId}/credits?language=en-US";

        // Create a UnityWebRequest to send a GET request to the API
        UnityWebRequest www = UnityWebRequest.Get(url);

        // Set the request headers
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Send the request and wait for the response
        yield return www.SendWebRequest();

        // Check if the response was successful
        if (www.result == UnityWebRequest.Result.Success)
        {
            // Get the response text
            string responseText = www.downloadHandler.text;

            Debug.Log("Cast Response: " + responseText);

            // Store the response in the cache
            cache.StoreResponse(url, responseText);
            // Deserialize the JSON response into a CastResponse object
            CastResponse castResponse = JsonUtility.FromJson<CastResponse>(responseText);

            // Invoke the callback with the response
            callback?.Invoke(castResponse.cast);
        }
        else
        {
            // Log an error if the response was not successful
            Debug.LogError($"API Error: {www.error}");

            // Invoke the callback with null
            callback?.Invoke(null);
        }
    }

    public IEnumerator LoadImage(Movie movie, Image targetImage)
    {
        // Get the file path for the cached movie poster image
        string path = GetImagePath(movie.id);

        // Check if the cached image exists
        if (File.Exists(path))
        {
            // Read the image data from the file
            byte[] imageData = File.ReadAllBytes(path);
            // Create a new Texture2D to load the image data
            Texture2D texture = new Texture2D(2, 2);
            // Load the image data into the texture
            texture.LoadImage(imageData);

            // Create a sprite from the texture and assign it to the target image
            targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // Log a message to indicate that the cached image was loaded successfully
            Debug.Log($"Loaded cached image for Movie ID: {movie.id}");
            // Exit the coroutine since the image was loaded successfully
            yield break;
        }

        // If the cached image does not exist, download and cache the image
        yield return StartCoroutine(DownloadAndCacheImage(movie, targetImage));
    }

    // Loads the movie poster image from the specified URL
    IEnumerator DownloadAndCacheImage(Movie movie, Image targetImage)
    {
        // Create a UnityWebRequest to load the image
        string url = "https://image.tmdb.org/t/p/w500" + movie.poster_path;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        // Wait for the request to complete
        yield return www.SendWebRequest();

        // Check if the request was successful
        if (www.result == UnityWebRequest.Result.Success)
        {
            // Get the loaded texture
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            // Create a sprite from the texture
            targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // Save image to local storage
            byte[] imageBytes = texture.EncodeToPNG();
            File.WriteAllBytes(GetImagePath(movie.id), imageBytes);
            Debug.Log($"Cached image for Movie ID: {movie.id}");
        }
        else
        {
            // If the request failed, use the default sprite
            targetImage.sprite = MovieDetailsController.Instance.defSprite;
            Debug.LogError($"Failed to download image for Movie ID {movie.id}: {www.error}");
        }
    }

    private string GetImagePath(int movieId)
    {
        // Returns the file path for the cached movie poster image
        return Path.Combine(Application.persistentDataPath, $"{movieId}.png");
    }
}