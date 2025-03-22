using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { set; get; }
    AudioSource uiAudioSource;
    [SerializeField] AudioClip uiAudioClick;

    private void Awake()
    {
        if (Instance) Destroy(gameObject); 
        else Instance = this;//Instance the Script

        uiAudioSource = gameObject.AddComponent<AudioSource>();
        uiAudioSource.volume = 0.5f;
        
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayUIAudioClick()
    {
        uiAudioSource.PlayOneShot(uiAudioClick);
    }
}