using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance{ set; get;}//Instance Set var and Get var
    public SaveState state;//Ref Save State

    private void Awake()
    {
        if (Instance) Destroy(gameObject); 
        else Instance = this;//Instance the Script
        
        DontDestroyOnLoad(this.gameObject);

        Load();//Load all info on the save state
        Debug.Log(Helper.Serialize<SaveState>(state));
    }

    // Save the whole state of this saveState script to the player pref
    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveState>(state));

    } 

    //Load Saved PlayerPrefs
    public void Load()
    {
        // Check if a save exists in PlayerPrefs
        if (PlayerPrefs.HasKey("save"))
        {
            // Deserialize the save data from PlayerPrefs and assign it to the state
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            // If no save exists, create a new SaveState and save it
            state = new SaveState();
            Save();
        }
    }

    //Reset the whole save file
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
        SceneManager.LoadScene(0);
    }
}
