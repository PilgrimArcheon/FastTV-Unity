using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state of the game that can be saved.
/// </summary>
[System.Serializable]
public class SaveState
{
    /// <summary>
    /// The saved API key.
    /// </summary>
    public string SavedApiKey;
}