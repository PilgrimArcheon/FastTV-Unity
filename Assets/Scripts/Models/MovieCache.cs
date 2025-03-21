using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// A class responsible for caching movie data.
/// </summary>
public class MovieCache
{
    /// <summary>
    /// The path to the cache file.
    /// </summary>
    private string cachePath;

    /// <summary>
    /// Initializes a new instance of the MovieCache class.
    /// </summary>
    public MovieCache()
    {
        // Set the cache path to the persistent data path with the file name "CachedMovies.json".
        cachePath = Application.persistentDataPath + "/CachedMovies.json";
    }

    /// <summary>
    /// Loads the cached movies from the cache file.
    /// </summary>
    /// <returns>A list of cached movies.</returns>
    public List<Movie> LoadCachedMovies()
    {
        // Check if the cache file exists.
        if (File.Exists(cachePath))
        {
            // Read the JSON data from the cache file.
            string json = File.ReadAllText(cachePath);
            // Deserialize the JSON data into a MovieSearchResponse object and return the results.
            return JsonUtility.FromJson<MovieSearchResponse>(json).results;
        }
        // If the cache file does not exist, return an empty list.
        return new List<Movie>();
    }

    /// <summary>
    /// Saves the movies to the cache file.
    /// </summary>
    /// <param name="movies">The list of movies to save.</param>
    public void SaveMovies(List<Movie> movies)
    {
        // Create a new MovieSearchResponse object with the given movies.
        string json = JsonUtility.ToJson(new MovieSearchResponse { results = movies });
        // Write the JSON data to the cache file.
        File.WriteAllText(cachePath, json);
    }
}