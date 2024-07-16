using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerCamera playerCamera;
    public InputManager inputManager;
    Animator animator;
    public WorldSaveGameManager saveManager;

    public PlayerUIManager playerUIManager;
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerEquipmentManager playerEquipmentManager;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerInventoryManager playerInventoryManager;
    public PlayerStatsManager playerStatsManager;
    public PlayerInteractionManager playerInteractionManager;

    public Rigidbody rb;

    [Header("Flags")]
    public bool disableRootMotion;
    public bool isPerformingAction;
    public bool isPerformingQuickTurn;
    public bool isAiming;
    public bool canInteract;
    public bool isDead;
    public bool pauseMenuActive;
    public bool isGrounded;

    [Header("Save Data")]
    public bool checkpoint1;
    public bool checkpoint2;
    public bool checkpoint3;
    public int losesCount;

    private void Awake()
    {
        playerUIManager = FindObjectOfType<PlayerUIManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        rb = GetComponent<Rigidbody>();
        saveManager = FindObjectOfType<WorldSaveGameManager>();
        WorldSaveGameManager.instance.player = this;
        losesCount = saveManager.LoadLosesCount();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        disableRootMotion = animator.GetBool("disableRootMotion");
        isAiming = animator.GetBool("isAiming");
        isPerformingAction = animator.GetBool("isPerformingAction");
        isPerformingQuickTurn = animator.GetBool("isPerformingQuickTurn");
    }

    private void FixedUpdate()
    {
        playerLocomotionManager.HandleAllLocomotion();
    }

    private void LateUpdate()
    {
        playerCamera.HandleAllCameraMovement();
    }

    public void UseCurrentWeapon()
    {
        if (isPerformingAction)
            return;

        if(playerEquipmentManager.weapon.remainingAmmo > 0)
        {
            playerEquipmentManager.weapon.remainingAmmo--;
            playerUIManager.currentAmmoCountText.text = playerEquipmentManager.weapon.remainingAmmo.ToString();
            playerAnimatorManager.PlayAnimationWithOutRootMotion("Pistol Shoot", true);
            playerEquipmentManager.weaponAnimator.ShootWeapon(playerCamera);
            SoundManager.instance.PlaySound(playerEquipmentManager.weapon.soundClip.name);
        }
        else
        {
            //Play a click sound effect so it is apparent you dont have ammo left
        }
    }

    public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
    {
        currentCharacterSaveData.checkpoint1 = checkpoint1;
        currentCharacterSaveData.checkpoint2 = checkpoint2;
        currentCharacterSaveData.checkpoint3 = checkpoint3;
        currentCharacterSaveData.losesCount = losesCount;
        currentCharacterSaveData.playerHealth = playerStatsManager.playerCurrentHealth;

        currentCharacterSaveData.playerSaveCoordX = transform.position.x;
        currentCharacterSaveData.playerSaveCoordY = transform.position.y;
        currentCharacterSaveData.playerSaveCoordZ = transform.position.z;
    }

    public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
    {
        checkpoint1 = currentCharacterSaveData.checkpoint1;
        checkpoint2 = currentCharacterSaveData.checkpoint2;
        checkpoint3 = currentCharacterSaveData.checkpoint3;
        losesCount = currentCharacterSaveData.losesCount;
        playerStatsManager.playerCurrentHealth = currentCharacterSaveData.playerHealth;

        transform.position = new Vector3(currentCharacterSaveData.playerSaveCoordX,
            currentCharacterSaveData.playerSaveCoordY, currentCharacterSaveData.playerSaveCoordZ);
    }
}
