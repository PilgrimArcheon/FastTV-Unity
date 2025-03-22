using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

// Represents a movie item in the UI
public class MovieItem : MonoBehaviour
{
    // Default sprite to display when no image is available
    [SerializeField] Sprite defSprite;
    // Image component to display the movie poster
    [SerializeField] Image posterImage;
    // Text component to display the movie title
    [SerializeField] TMP_Text titleText;
    // Canvas group to control the visibility and alpha of the movie item
    [SerializeField] CanvasGroup canvasGroup;
    // Button component to handle clicks on the movie item
    [SerializeField] Button button;
    // Movie data associated with this movie item
    [SerializeField] Movie movie;

    // Called when the script is initialized
    private void Awake()
    {
        // Add a click event listener to the button
        button.onClick.AddListener(() => OnClick());
        // Get the CanvasGroup component if it's not already assigned in the inspector
        canvasGroup = GetComponent<CanvasGroup>();
        // Initialize the alpha to 0 to hide the movie item
        canvasGroup.alpha = 0;
    }

    // Sets the movie data for this movie item
    public void SetMovie(Movie movieData)
    {
        // Store the movie data
        movie = movieData;
        // Set the title text
        titleText.text = movie.title;
        // Set the default sprite as a placeholder
        posterImage.sprite = MovieDetailsController.Instance.defSprite;
        // Start loading the movie poster image

        StartCoroutine(MovieAPI.Instance.LoadImage(movie, posterImage));
        // Start fading in the movie item
        StartCoroutine(FadeIn());
    }

    // Fades in the movie item
    IEnumerator FadeIn()
    {
        // Gradually increase the alpha until it reaches 1
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    // Handles a click on the movie item
    public void OnClick()
    {
        // Start the click animation
        StartCoroutine(AnimateClick());
    }

    // Animates the click on the movie item
    IEnumerator AnimateClick()
    {
        // Scale down the movie item slightly
        transform.localScale = Vector3.one * 0.9f;
        // Play the UI audio click sound effect
        AudioManager.Instance.PlayUIAudioClick();
        // Wait for a short duration
        yield return new WaitForSeconds(0.1f);
        // Scale back up to the original size
        transform.localScale = Vector3.one;
        // Show the movie details
        MovieDetailsController.Instance.ShowMovieDetails(movie);
    }
}