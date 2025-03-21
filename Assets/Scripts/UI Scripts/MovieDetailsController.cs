using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MovieDetailsController : MonoBehaviour
{
    // Singleton instance of the MovieDetailsController
    public static MovieDetailsController Instance;

    // UI components for displaying movie details
    [SerializeField] UIContentTween movieDetailsPanel;
    [SerializeField] TMP_Text titleText, overviewText, ratingText, castingText;
    [SerializeField] Image posterImage;
    [SerializeField] public Sprite defSprite;

    // Current movie details
    private MovieDetails currentMovie;

    // Initialize the singleton instance in the Awake method
    void Awake() => Instance = this;

    // Reset the movie details to their default values
    void ResetDetails()
    {
        currentMovie = null;
        titleText.text = "MOVIE-TITLE";
        overviewText.text = "MOVIE-OVERVIEW";
        ratingText.text = "Rating: 0.0";
        posterImage.sprite = defSprite;
    }

    // Show the movie details for the given movie
    public void ShowMovieDetails(Movie movie)
    {
        // Reset the movie details before loading new data
        ResetDetails();

        // Start a coroutine to load the movie details from the API
        StartCoroutine(MovieAPI.Instance.GetMovieDetails(movie.id, (details) =>
        {
            // Log the received movie details for debugging purposes
            Debug.Log(details);

            // Check if the movie details were received successfully
            if (details != null)
            {
                // Update the current movie details
                currentMovie = details;

                // Update the UI components with the new movie details
                titleText.text = details.title;
                overviewText.text = details.overview;
                ratingText.text = "Rating: " + details.vote_average;

                // Start a coroutine to load the movie poster image
                StartCoroutine(LoadImage("https://image.tmdb.org/t/p/w500" + details.poster_path, posterImage));

                // Populate the cast list with the received cast data
                PopulateCast(details.cast);
            }

            // Animate the movie details panel to its visible position
            movieDetailsPanel.TweenToPositionX(0);
        }));
    }

    // Populate the cast list with the given cast data
    private void PopulateCast(List<Cast> cast)
    {
        // Log the received cast data for debugging purposes
        Debug.Log(cast);

        // Initialize the cast text with a header
        castingText.text = "CASTS: \n";

        // Check if the cast list is not empty
        if (cast.Count > 0)
        {
            // Iterate over the first 5 cast members and add them to the cast text
            for (int i = 0; i <= 5; i++)
            {
                castingText.text += $"{cast[i].name} as {cast[i].character} \n";
            }
        }
    }

    // Load an image from the given URL and set it as the sprite for the target image
    private IEnumerator LoadImage(string url, Image targetImage)
    {
        // Create a UnityWebRequest to load the image
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // Wait for the request to complete
        yield return request.SendWebRequest();

        // Check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Get the loaded texture from the request
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // Create a new sprite from the texture and set it as the sprite for the target image
            targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}