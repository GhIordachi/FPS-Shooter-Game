using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    Animator animator;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerManager playerManager;
    PlayerUIManager playerUIManager;

    [Header("Player Movement")]
    public float verticalMovementInput;
    public float horizontalMovementInput;
    private Vector2 movementInput;

    [Header("Camera Rotation")]
    public float verticalCameraInput;
    public float horizontalCameraInput;
    private Vector2 cameraInput;

    [Header("Button Inputs")]
    public bool runInput;
    public bool quickTurnInput;
    public bool jumpInput;
    public bool aimingInput;
    public bool shootInput;
    public bool reloadInput;
    public bool interactInput;
    public bool consumeInput;
    public bool rifleInput;
    public bool shotgunInput;
    public bool pistolInput;
    public bool escapeInput;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Run.performed += i => runInput = true;
            playerControls.PlayerMovement.Run.canceled += i => runInput = false;
            playerControls.PlayerMovement.QuickTurn.performed += i => quickTurnInput = true;
            playerControls.PlayerMovement.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Aim.performed += i => aimingInput = true;
            playerControls.PlayerActions.Aim.canceled += i => aimingInput = false;
            playerControls.PlayerActions.Shoot.performed += i => shootInput = true;
            playerControls.PlayerActions.Shoot.canceled += i => shootInput = false;
            playerControls.PlayerActions.Reload.performed += i => reloadInput = true;
            playerControls.PlayerActions.Interact.performed += i => interactInput = true;
            playerControls.PlayerActions.Consume.performed += i => consumeInput = true;
            playerControls.PlayerActions.ChangeToRifle.performed += i => rifleInput = true;
            playerControls.PlayerActions.ChangeToShotgun.performed += i => shotgunInput = true;
            playerControls.PlayerActions.ChangeToPistol.performed += i => pistolInput = true;
            playerControls.PlayerActions.Escape.performed += i => escapeInput = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleQuickTurnInput();
        HandleJumpInput();
        HandleAimingInput();
        HandleShootingInput();
        HandleReloadInput();
        HandleInteractionInput();
        HandleConsumeInput();
        HandleChangeWeaponInput();
        HandleEscapeInput();
    }

    private void HandleMovementInput()
    {
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        playerAnimatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);
    }

    private void HandleCameraInput()
    {
        horizontalCameraInput = cameraInput.x;
        verticalCameraInput = cameraInput.y;
    }

    private void HandleQuickTurnInput()
    {
        if (playerManager.isPerformingAction)
            return;

        if(quickTurnInput)
        {
            animator.SetBool("isPerformingQuickTurn", true);
            playerAnimatorManager.PlayAnimationWithOutRootMotion("Quick Turn", true);
        }
    }

    private void HandleJumpInput()
    {
        if(jumpInput && playerManager.isGrounded == true)
        {
            jumpInput = false;
            playerManager.playerLocomotionManager.HandleJumping();
            playerManager.playerAnimatorManager.ClearHandIK();
        }
    }

    private void HandleAimingInput()
    {
        if(verticalMovementInput == 2)
        {
            aimingInput = false;
            animator.SetBool("isAiming", false);
            playerUIManager.crosshair.SetActive(false);
            return;
        }

        if (aimingInput)
        {
            animator.SetBool("isAiming", true);
            playerUIManager.crosshair.SetActive(true);
        }
        else
        {
            animator.SetBool("isAiming", false);
            playerUIManager.crosshair.SetActive(false);
        }

        playerAnimatorManager.UpdateAimConstraints();
    }

    private void HandleShootingInput()
    {
        if(shootInput && aimingInput)
        {
            shootInput = false;
            playerManager.UseCurrentWeapon();
        }
    }

    private void HandleReloadInput()
    {
        if (playerManager.isPerformingAction)
            return;

        if(reloadInput)
        {
            reloadInput = false;
            //Check if the weapon is full
            if (playerManager.playerEquipmentManager.weapon.remainingAmmo == playerManager.playerEquipmentManager.weapon.maxAmmo)
                return;
            //Check if we have the right ammo
            if (playerManager.playerInventoryManager.currentAmmoInInventory != null)
            {
                if (playerManager.playerInventoryManager.currentAmmoInInventory.ammoType == playerManager.playerEquipmentManager.weapon.ammoType)
                {
                    if (playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining == 0)
                        return;

                    int amountOfAmmoToReload = 0;
                    int ammoRemainingInInventory = playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining;
                    amountOfAmmoToReload = playerManager.playerEquipmentManager.weapon.maxAmmo - playerManager.playerEquipmentManager.weapon.remainingAmmo;

                    //Handles the reloading logic, if we have more ammo than we need we reload with a full magazine, if not, we reload with what its left
                    if(ammoRemainingInInventory >= amountOfAmmoToReload)
                    {
                        playerManager.playerEquipmentManager.weapon.remainingAmmo += amountOfAmmoToReload;
                        ammoRemainingInInventory -= amountOfAmmoToReload;
                    }
                    else
                    {
                        playerManager.playerEquipmentManager.weapon.remainingAmmo = ammoRemainingInInventory;
                        ammoRemainingInInventory = 0;
                    }

                    playerManager.playerAnimatorManager.ClearHandIK();
                    playerManager.playerAnimatorManager.PlayAnimation("Reload Gun", true);
                    playerManager.playerUIManager.currentAmmoCountText.text = playerManager.playerEquipmentManager.weapon.remainingAmmo.ToString();
                    playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining = ammoRemainingInInventory;
                    playerManager.playerUIManager.reservedAmmoCountText.text = playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining.ToString();
                }
            }
        }
    }

    public void HandleConsumeInput()
    {
        if (playerManager.isPerformingAction)
            return;

        if (consumeInput)
        {
            consumeInput = false;
            if(playerManager.playerInventoryManager.currentHealthKitItem.healthKitCount > 0)
            {
                playerManager.playerInventoryManager.currentHealthKitItem.healthKitCount--;
                playerManager.playerAnimatorManager.PlayAnimationWithOutRootMotion("Consume", true);
                playerManager.playerStatsManager.HealPlayer(playerManager.playerInventoryManager.currentHealthKitItem.healthToBeRestored);
                playerManager.playerUIManager.currentHealthKitsAvailableText.text = 
                    playerManager.playerInventoryManager.currentHealthKitItem.healthKitCount.ToString();
            }
        }
    }

    public void HandleInteractionInput()
    {
        if (interactInput)
        {
            if (!playerManager.canInteract)
            {
                interactInput = false;
            }
        }
    }

    public void HandleChangeWeaponInput()
    {
        if (rifleInput)
        {
            rifleInput = false;
            playerManager.playerEquipmentManager.weapon = playerManager.playerInventoryManager.gunsInventory[0];
            playerManager.playerInventoryManager.currentAmmoInInventory = playerManager.playerInventoryManager.ammosInventory[0];
            playerManager.playerEquipmentManager.LoadCurrentWeapon();
        }
        else if (shotgunInput)
        {
            shotgunInput = false;
            playerManager.playerEquipmentManager.weapon = playerManager.playerInventoryManager.gunsInventory[1];
            playerManager.playerInventoryManager.currentAmmoInInventory = playerManager.playerInventoryManager.ammosInventory[1];
            playerManager.playerEquipmentManager.LoadCurrentWeapon();
        }
        else if (pistolInput)
        {
            pistolInput = false;
            playerManager.playerEquipmentManager.weapon = playerManager.playerInventoryManager.gunsInventory[2];
            playerManager.playerInventoryManager.currentAmmoInInventory = playerManager.playerInventoryManager.ammosInventory[2];
            playerManager.playerEquipmentManager.LoadCurrentWeapon();
        }
    }

    public void HandleEscapeInput()
    {
        if (escapeInput)
        {
            escapeInput = false;
            if(playerManager.pauseMenuActive)
            {
                playerManager.pauseMenuActive = false;
                playerManager.playerUIManager.ActivatePauseMenu();
            }
            else
            {
                playerManager.pauseMenuActive = true;
                playerManager.playerUIManager.DeactivatePauseMenu();
            }
        }
    }
}
