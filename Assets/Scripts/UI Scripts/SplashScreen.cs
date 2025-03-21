using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    // Reference to the loading text UI element
    [SerializeField] TMP_Text loadingText;
    
    // Reference to the API key prompt UI element
    [SerializeField] GameObject apiKeyPrompt;
    
    // Reference to the API key input field UI element
    [SerializeField] TMP_InputField apiKeyInput;
    
    // Reference to the API key button UI element
    [SerializeField] Button apiKeyButton;
    
    // Speed of the loading animation
    private float animationSpeed = 1f;

    // Called when the script is initialized
    private void Start()
    {
        // Get the saved API key from the SaveManager
        string savedKey = SaveManager.Instance.state.SavedApiKey;
        
        // If no API key is saved, show the API key prompt
        if (string.IsNullOrEmpty(savedKey))
        {
            apiKeyPrompt.SetActive(true);
            apiKeyButton.onClick.AddListener(() => SaveApiKey());
        }
        // If an API key is saved, load the main scene
        else StartCoroutine(LoadMainScene());
    }

    // Called when the API key button is clicked
    public void SaveApiKey()
    {
        // Save the API key input by the user
        SaveManager.Instance.state.SavedApiKey = apiKeyInput.text;
        SaveManager.Instance.Save();
        
        // Hide the API key prompt
        apiKeyPrompt.SetActive(false);
        
        // Load the main scene
        StartCoroutine(LoadMainScene());
    }

    // Coroutine to load the main scene with a loading animation
    private IEnumerator LoadMainScene()
    {
        // Initialize the loading progress
        int currentProgress = 0;

        // Loop until the loading progress is complete
        while (currentProgress < 2)
        {
            // Set the loading text
            loadingText.text = "Loading";
            
            // Add dots to the loading text with a delay
            for (int i = 0; i < 3; i++)
            {
                loadingText.text += ".";
                
                // Wait for the animation speed before adding the next dot
                yield return new WaitForSeconds(animationSpeed);
            }
            currentProgress++;
        }

        // Load the main scene
        SceneManager.LoadScene("MainScene");
    }
}