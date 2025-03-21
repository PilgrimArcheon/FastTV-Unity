using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A cache for storing API responses to improve performance by reducing the number of requests.
/// </summary>
public class APIResponseCache
{
    /// <summary>
    /// The internal cache dictionary that stores API responses.
    /// </summary>
    private Dictionary<string, string> cache = new Dictionary<string, string>();

    /// <summary>
    /// Attempts to retrieve a cached API response for the given URL.
    /// </summary>
    /// <param name="url">The URL of the API response to retrieve.</param>
    /// <param name="response">The cached API response if found, otherwise null.</param>
    /// <returns>True if the response was found in the cache, false otherwise.</returns>
    public bool TryGetResponse(string url, out string response)
    {
        return cache.TryGetValue(url, out response);
    }

    /// <summary>
    /// Stores an API response in the cache for the given URL.
    /// </summary>
    /// <param name="url">The URL of the API response to store.</param>
    /// <param name="response">The API response to store.</param>
    public void StoreResponse(string url, string response)
    {
        // Check if the cache already contains a response for the given URL
        if (!cache.ContainsKey(url))
        {
            // Store the response in the cache
            cache[url] = response;
        }
    }
}