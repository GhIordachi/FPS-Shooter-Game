using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStatsManager : MonoBehaviour
{
    ZombieManager zombie;
    KillCount killCount;

    [Header("Damage Modifiers")]
    public float headshotDamageModifier = 2f;
    public float armsDamageModifier = 0.5f;
    public float legsDamageModifier = 0.75f;

    [Header("Zombie Health")]
    public int overallHealth = 100;
    public int headHealth = 100;
    public int torsoHealth = 100;
    public int leftArmHealth = 100;
    public int rightArmHealth = 100;
    public int leftLegHealth = 100;
    public int rightLegHealth = 100;

    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();
        killCount = FindObjectOfType<KillCount>();
    }

    public void DealHeadShotDamage(int damage)
    {
        headHealth -= Mathf.RoundToInt(damage * headshotDamageModifier);
        overallHealth -= Mathf.RoundToInt(damage * headshotDamageModifier);
        CheckForDeath();
    }

    public void DealTorsoDamage(int damage)
    {
        torsoHealth -= damage;
        overallHealth -= damage;
        CheckForDeath();
    }

    public void DealArmDamage(bool leftArmDamage, int damage)
    {
        if(leftArmDamage)
        {
            leftArmHealth -= Mathf.RoundToInt(damage * armsDamageModifier);
            overallHealth -= Mathf.RoundToInt(damage * armsDamageModifier);
        }
        else
        {
            rightArmHealth -= Mathf.RoundToInt(damage * armsDamageModifier);
            overallHealth -= Mathf.RoundToInt(damage * armsDamageModifier);
        }
        CheckForDeath();
    }

    public void DealLegDamage(bool leftLegDamage, int damage)
    {
        if(leftLegDamage)
        {
            leftLegHealth -= Mathf.RoundToInt(damage * legsDamageModifier);
            overallHealth -= Mathf.RoundToInt(damage * legsDamageModifier);
        }
        else
        {
            rightLegHealth -= Mathf.RoundToInt(damage * legsDamageModifier);
            overallHealth -= Mathf.RoundToInt(damage * legsDamageModifier);
        }
        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if (overallHealth <= 0)
        {
            overallHealth = 0;
            zombie.isDead = true;
            zombie.zombieAnimatorManager.PlayTargetActionAnimation("Zombie_Death_01");
            zombie.zombieCombatManager.CloseAttackColliders();
            killCount.count++;
            PlayerUIManager uiManager = FindObjectOfType<PlayerUIManager>();
            uiManager.UpdateKillCount();
            Destroy(zombie.GetComponent<Collider>());
            Destroy(zombie.GetComponent<Rigidbody>());
        }
    }
}
