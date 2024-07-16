using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Checkpoints")]
    public bool checkpoint1;
    public bool checkpoint2;
    public bool checkpoint3;

    [Header("Lose Count")]
    public int losesCount;

    [Header("Player Health")]
    public int playerHealth;

    [Header("Player Save Coordonates")]
    public float playerSaveCoordX;
    public float playerSaveCoordZ;
    public float playerSaveCoordY;

    public CharacterSaveData()
    {

    }
}
