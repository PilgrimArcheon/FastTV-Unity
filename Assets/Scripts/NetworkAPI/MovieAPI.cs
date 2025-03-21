using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

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

            // Store the response in the cache
            cache.StoreResponse(url, responseText);

            // Invoke the callback with the response
            callback?.Invoke(JsonUtility.FromJson<MovieDetails>(responseText));
        }
        else
        {
            // Log an error if the response was not successful
            Debug.LogError($"API Error: {www.error}");

            // Invoke the callback with null
            callback?.Invoke(null);
        }
    }
}