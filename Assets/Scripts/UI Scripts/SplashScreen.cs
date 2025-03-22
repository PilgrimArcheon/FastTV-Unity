using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScreen : MonoBehaviour
{
    // Reference to the loading text UI element
    [SerializeField] TMP_Text loadingText;

    // Reference to the API key prompt UI element
    [SerializeField] UIContentTween apiKeyPrompt;

    // Reference to the API key input field UI element
    [SerializeField] TMP_InputField apiKeyInput;
    // Reference to the error text UI element, used to display error messages to the user
    [SerializeField] TMP_Text errorText;

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
            apiKeyPrompt.TweenToPositionY(0);
            apiKeyButton.onClick.AddListener(() => SaveApiKey());
            apiKeyInput.onValueChanged.AddListener(var => errorText.text = "");
        }
        // If an API key is saved, load the main scene
        else StartCoroutine(LoadMainScene());
    }

    // Called when the API key button is clicked
    public void SaveApiKey()
    {
        StartCoroutine(ConfirmAPIKey(apiKeyInput.text, (isValid) =>
        {
            if (isValid)
            {
                // Save the API key input by the user
                SaveManager.Instance.state.SavedApiKey = apiKeyInput.text;
                SaveManager.Instance.Save();

                // Hide the API key prompt
                apiKeyPrompt.ResetTweenPos();
                // Play the UI audio click sound effect
                AudioManager.Instance.PlayUIAudioClick();

                // Load the main scene
                StartCoroutine(LoadMainScene());
            }
            else
            {
                errorText.text = "Invalid! Enter Valid API Key";
            }
        }));

    }

    public IEnumerator ConfirmAPIKey(string apiKey, Action<bool> callback)
    {
        // Construct the API URL
        string url = $"https://api.themoviedb.org/3/authentication";

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

            AuthResponse authResponse = JsonUtility.FromJson<AuthResponse>(responseText);
            // Invoke the callback with the response
            callback?.Invoke(authResponse.success);
        }
        else
        {
            // Log an error if the response was not successful
            Debug.LogError($"API Auth Error: {www.error}");

            // Invoke the callback with null
            callback?.Invoke(false);
        }
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

[System.Serializable]
public class AuthResponse
{
    public bool success;
}