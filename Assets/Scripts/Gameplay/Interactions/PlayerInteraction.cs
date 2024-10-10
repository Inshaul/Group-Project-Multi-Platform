using System.Collections;
using System.Collections.Generic;
using Gameplay.Interactions;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;  // Max distance to interact
    private Interactable interactable; // Reference to interactable object

    void Update()
    {
        CheckForInteractable();

        // Check if the F key is pressed
        if (Input.GetKeyDown(KeyCode.F) && interactable != null)
        {
            interactable.Interact();
        }
    }

    void CheckForInteractable()
    {
        // Cast a ray from the player's position forward
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Check if the object hit has an Interactable component
            Interactable hitInteractable = hit.collider.GetComponent<Interactable>();

            if (hitInteractable != null)
            {
                interactable = hitInteractable;
            }
            else
            {
                interactable = null;
            }
        }
        else
        {
            interactable = null;
        }
    }
}
