using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Numerics;

public class MovieDetailsController : MonoBehaviour
{
    // Singleton instance of the MovieDetailsController
    public static MovieDetailsController Instance;

    // UI components for displaying movie details
    [SerializeField] UIContentTween movieDetailsPanel;
    // UI component for holding the movie details content
    [SerializeField] RectTransform detailsHolder;
    // The initial anchored position of the details holder on the y-axis
    [SerializeField] float detailsAnchoredStart = 500f; 
    // UI component for the close button
    [SerializeField] Button closeButton;
    // UI components for displaying movie details
    [SerializeField]
    TMP_Text titleText, // Text component for displaying the movie title
    overviewText, // Text component for displaying the movie overview
    ratingText, // Text component for displaying the movie rating
    castingText, // Text component for displaying the movie casting information
    genreText; // Text component for displaying the movie genre information
    [SerializeField] Image posterImage; // Image component for displaying the movie poster
    [SerializeField] public Sprite defSprite; // Default sprite to display when no poster is available

    // Current movie details
    private MovieDetails currentMovie;

    // Initialize the singleton instance in the Awake method
    void Awake()
    {
        Instance = this;
        // Add a listener to the close button's onClick event to close the movie details panel
        closeButton.onClick.AddListener(() => CloseDetails());
    }

    // Reset the movie details to their default values
    void ResetDetails()
    {
        currentMovie = null;
        titleText.text = "MOVIE-TITLE";
        overviewText.text = "MOVIE-OVERVIEW";
        ratingText.text = "Rating: 0.0";
        genreText.text = "Genres: ";
        castingText.text = "Cast: \n";
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
                titleText.text = currentMovie.title;
                overviewText.text = currentMovie.overview;
                ratingText.text = "Rating: " + currentMovie.vote_average;

                // Start a coroutine to load the movie poster image
                StartCoroutine(MovieAPI.Instance.LoadImage(movie, posterImage));

                // Populate the genre list with the given genre data
                PopulateGenre(currentMovie.genres);

                // Populate the cast list with the received cast data
                PopulateCast(currentMovie.cast);
            }

            // Animate the movie details panel to its visible position
            movieDetailsPanel.TweenToPositionX(0);
            detailsHolder.anchoredPosition = new UnityEngine.Vector2(0, detailsAnchoredStart);
        }));
    }

    // Populate the cast list with the given cast data
    private void PopulateCast(List<Cast> cast)
    {
        // Initialize the cast text with a header
        castingText.text = "Cast: \n";

        // Check if the cast list is not empty
        if (cast.Count > 0)
        {
            // Iterate over the first 5 cast members and add them to the cast text
            for (int i = 0; i <= 5; i++)
            {
                castingText.text += $"{cast[i].name} as {cast[i].character}";

                if (i + 1 < cast.Count && i + 1 < 5) castingText.text += ", \n";

                if (i + 1 == cast.Count || i + 1 >= 5)
                {
                    // Add a period at the end of the cast text
                    castingText.text += ".";
                    break;
                }
            }
        }
    }

    // Populate the genre list with the given cast data
    private void PopulateGenre(List<Genre> genres)
    {
        // Initialize the genre text with a header
        genreText.text = "Genre: ";

        // Check if the genre list is not empty
        if (genres.Count > 0)
        {
            // Iterate over the first 3 genres and add them to the genre text
            for (int i = 0; i <= 3; i++)
            {
                // Add the genre name to the genre text
                genreText.text += $"{genres[i].name}";

                // Add a comma after each genre except the last one
                if (i + 1 < genres.Count && i + 1 < 3) genreText.text += ", ";

                // Check if we've reached the last genre in the list
                // If so, break out of the loop to avoid unnecessary iterations
                if (i + 1 == genres.Count || i + 1 >= 3)
                {
                    // Add a period at the end of the genre text
                    genreText.text += ".";
                    break;
                }
            }
        }
    }

    private void CloseDetails()
    {
        // Play the UI audio click sound effect
        AudioManager.Instance.PlayUIAudioClick();
        // Reset the movie details panel to its initial position
        movieDetailsPanel.ResetTweenPos();
    }
}