using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected PlayerManager player; //The player interacting with the object
    protected Collider interactableCollider; //The collider enabling/disabling the interaction
    [SerializeField] protected GameObject interactableCanvas; //The image indicating a player can interact

    protected virtual void Interact(PlayerManager player)
    {
        Debug.Log("We have interacted");
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if(player != null)
        {
            if (player.inputManager.interactInput)
            {               
                Interact(player);
                player.inputManager.interactInput = false;
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(player == null)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if(player != null)
        {
            interactableCanvas.SetActive(true);
            player.canInteract = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (player == null)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if (player != null)
        {
            interactableCanvas.SetActive(false);
            player.canInteract = false;
        }
    }
}
