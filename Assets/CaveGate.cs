using System.Collections;
using System.Collections.Generic;
using Gameplay.Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;  // Add this for scene management

public class CaveGate : Interactable
{
    public string nextSceneName;  // The name of the scene to load

    // Override the Interact method from the base class
    public override void Interact()
    {
        SwitchScene();
    }

    // This method switches to the next scene
    void SwitchScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Resume the game time in case it was paused
        Time.timeScale = 0;

        // Load the specified scene by its name
        SceneManager.LoadScene(nextSceneName);
        Time.timeScale = 1; 
    }
}
