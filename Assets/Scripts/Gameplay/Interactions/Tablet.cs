using System.Collections;
using System.Collections.Generic;
using Gameplay.Interactions;
using UnityEngine;

public class Tablet : Interactable
{
    public GameObject uiCanvas;  // The UI canvas that contains the note and button

    private bool isUIActive = false;  // Tracks if the UI is currently active

    void Start()
    {
        // Ensure the UI starts hidden
        uiCanvas.SetActive(false);
    }

    // Override the Interact method from the base class
    public override void Interact()
    {
        // Check if the UI is not already active
        if (!isUIActive)
        {
            ShowNote();
        }
    }

    // This method shows the UI and pauses the game
    void ShowNote()
    {
        // Set the UI Canvas active
        uiCanvas.SetActive(true);

        // Pause the game by setting timeScale to 0
        Time.timeScale = 0;

        // Set the UI active flag to true
        isUIActive = true;

        // Optional: lock the cursor or disable player movement while the note is open
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // This method hides the UI and resumes the game
    public void HideNote()
    {
        // Hide the UI Canvas
        uiCanvas.SetActive(false);

        // Resume the game by setting timeScale to 1
        Time.timeScale = 1;

        // Set the UI active flag to false
        isUIActive = false;

        // Optional: lock the cursor or enable player movement again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
