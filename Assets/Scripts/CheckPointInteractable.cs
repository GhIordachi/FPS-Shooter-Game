using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointInteractable : InteractableObject
{
    public int checkpointNumber = 1;

    protected override void Interact(PlayerManager player)
    {
        base.Interact(player);
        player.playerInteractionManager.CheckpointSaved(player,checkpointNumber);
    }
}
