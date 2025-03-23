using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MovieSearchController : MonoBehaviour
{
    // Reference to the search input field
    [SerializeField] TMP_InputField searchInput;

    // References to the search and close buttons
    [SerializeField] Button closeButton;

    // Reference to the container for search results
    [SerializeField] Transform resultsContainer;

    // Reference to the prefab for a movie item
    [SerializeField] GameObject movieItemPrefab;

    // Called when the script is initialized
    private void Start()
    {
        // Add listeners for search input submission and value changes
        searchInput.onSubmit.AddListener(var => StartCoroutine(SearchMovies()));
        // Add listener for search input value changes to trigger a search or retrieve the movie list
        searchInput.onValueChanged.AddListener(var => TrySearch());

        // Add listener for close button click
        closeButton.onClick.AddListener(() => StartCoroutine(GetMovies()));

        // Initialize the movie list
        StartCoroutine(GetMovies());
    }

    void TrySearch()
    {
        // Check if the search input is empty
        if (string.IsNullOrEmpty(searchInput.text)) StartCoroutine(GetMovies()); // If empty, retrieve the list of movies
        //else StartCoroutine(SearchMovies()); // If not empty, search for movies based on the input query
    }

    // Called every frame
    void Update()
    {
        // Enable or disable the close button based on the search input text
        closeButton.gameObject.SetActive(!string.IsNullOrEmpty(searchInput.text));
    }

    // Coroutine to retrieve the list of movies
    private IEnumerator GetMovies()
    {
        // Clear the search input text
        searchInput.text = "";

        // Retrieve the list of movies from the API
        yield return MovieAPI.Instance.GetMovies((movies) =>
        {
            // Clear the existing search results
            foreach (Transform child in resultsContainer)
            {
                Destroy(child.gameObject);
            }

            // Instantiate and populate the movie items with the top 12 movies
            for (int i = 0; i < 12; i++)
            {
                GameObject item = Instantiate(movieItemPrefab, resultsContainer);
                item.GetComponent<MovieItem>().SetMovie(movies[i]);
            }
        });
    }

    // Coroutine to search for movies based on the input query
    private IEnumerator SearchMovies()
    {
        // Get the trimmed search query
        string query = searchInput.text.Trim();

        // If the query is empty, exit the coroutine
        if (string.IsNullOrEmpty(query)) yield break;

        // Search for movies using the API
        yield return MovieAPI.Instance.SearchMovies(query, (movies) =>
        {
            // Clear the existing search results
            foreach (Transform child in resultsContainer)
            {
                Destroy(child.gameObject);
            }

            // Instantiate and populate the movie items
            foreach (var movie in movies)
            {
                GameObject item = Instantiate(movieItemPrefab, resultsContainer);
                item.GetComponent<MovieItem>().SetMovie(movie);
            }
        });
    }
}