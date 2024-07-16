using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEffectManager : MonoBehaviour
{
    ZombieManager zombie;

    [Header("Effects")]
    public GameObject bloodSplatterFX;

    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();
    }

    public void DamageZombieHead(int damage)
    {
        if (!zombie.isDead)
        {
            zombie.isPerformingAction = true;
            zombie.animator.CrossFade("Damage_Head", 0.2f);
            zombie.zombieStatsManager.DealHeadShotDamage(damage);
        }
    }

    public void DamageZombieTorso(int damage)
    {
        if (!zombie.isDead)
        {
            zombie.isPerformingAction = true;
            zombie.animator.CrossFade("Damage_Torso", 0.2f);
            zombie.zombieStatsManager.DealTorsoDamage(damage);
        }
    }

    public void DamageZombieRightArm(int damage)
    {
        if (!zombie.isDead)
        {
            zombie.isPerformingAction = true;
            zombie.animator.CrossFade("Damage_Torso", 0.2f);
            zombie.zombieStatsManager.DealArmDamage(false, damage);
        }
    }

    public void DamageZombieLeftArm(int damage)
    {
        if (!zombie.isDead)
        {
            zombie.isPerformingAction = true;
            zombie.animator.CrossFade("Damage_Torso", 0.2f);
            zombie.zombieStatsManager.DealArmDamage(true, damage);
        }
    }

    public void DamageZombieRightLeg(int damage)
    {
        if (!zombie.isDead)
        {
            zombie.isPerformingAction = true;
            zombie.animator.CrossFade("Damage_Torso", 0.2f);
            zombie.zombieStatsManager.DealLegDamage(false, damage);
        }
    }

    public void DamageZombieLeftLeg(int damage)
    {
        if (!zombie.isDead)
        {
            zombie.isPerformingAction = true;
            zombie.animator.CrossFade("Damage_Torso", 0.2f);
            zombie.zombieStatsManager.DealLegDamage(true, damage);
        }
    }

    public void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
}
