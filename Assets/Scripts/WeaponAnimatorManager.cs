using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorManager : MonoBehaviour
{
    PlayerManager playerManager;
    Animator weaponAnimator;

    [Header("Weapon FX")]
    public GameObject weaponMuzzleFlashFX;
    public GameObject weaponBulletCaseFX;

    [Header("Weapon FX Transforms")]
    public Transform weaponMuzzleFlashTransform;
    public Transform weaponBulletCaseTransform;

    [Header("Weapon Bullet Range")]
    public float bulletRange = 100f;

    [Header("Shootable Layers")]
    public LayerMask shootableLayers;

    protected Vector3 contactPoint;

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
    }

    public void ShootWeapon(PlayerCamera playerCamera)
    {
        weaponAnimator.Play("Shoot");

        if (weaponMuzzleFlashFX != null)
        {
            GameObject muzzleFlash = Instantiate(weaponMuzzleFlashFX, weaponMuzzleFlashTransform);
            muzzleFlash.transform.parent = null;
        }

        if (weaponBulletCaseFX != null)
        {
            GameObject bulletCase = Instantiate(weaponBulletCaseFX, weaponBulletCaseTransform);
            bulletCase.transform.parent = null;
        }

        RaycastHit hit;
        if(Physics.Raycast(playerCamera.cameraObject.transform.position, playerCamera.cameraObject.transform.forward, out hit, bulletRange, shootableLayers))
        {
            ZombieEffectManager zombie = hit.collider.gameObject.GetComponentInParent<ZombieEffectManager>();
            contactPoint = hit.collider.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            if (zombie != null)
            {
                zombie.PlayBloodSplatterFX(contactPoint);
                if(hit.collider.gameObject.layer == 8)
                {
                    zombie.DamageZombieHead(playerManager.playerEquipmentManager.weapon.damage);
                }
                else if(hit.collider.gameObject.layer == 9)
                {
                    zombie.DamageZombieTorso(playerManager.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 10)
                {
                    zombie.DamageZombieRightArm(playerManager.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 11)
                {
                    zombie.DamageZombieLeftArm(playerManager.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 12)
                {
                    zombie.DamageZombieRightLeg(playerManager.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 13)
                {
                    zombie.DamageZombieLeftLeg(playerManager.playerEquipmentManager.weapon.damage);
                }
            }
        }
    }
}
