using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    PursueTargetState pursueTargetState;

    [Header("Zombie Attacks")]
    public ZombieAttackAction[] zombieAttackActions;

    [Header("Potential Attacks Performable")]
    public List<ZombieAttackAction> potentialAttacks;

    [Header("Current Attack")]
    public ZombieAttackAction currentAttack;

    [Header("State Flags")]
    public bool hasPerformedAttack;

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);

        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        if(!hasPerformedAttack && zombieManager.attackCooldownTimer <= 0)
        {
            if(currentAttack == null)
            {
                GetNewAttack(zombieManager);
            }
            else
            {
                AttackTarget(zombieManager);
            }
        }

        if (hasPerformedAttack)
        {
            ResetStateFlags();
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }

    private void GetNewAttack(ZombieManager zombie)
    {
        for(int i = 0;i < zombieAttackActions.Length;i++)
        {
            ZombieAttackAction zombieAttack = zombieAttackActions[i];

            potentialAttacks.Add(zombieAttack);
        }

        int randomValue = Random.Range(0, potentialAttacks.Count);

        if(potentialAttacks.Count > 0)
        {
            currentAttack = potentialAttacks[randomValue];
            potentialAttacks.Clear();
        }
    }

    private void AttackTarget(ZombieManager zombieManager)
    {
        if(currentAttack != null)
        {
            hasPerformedAttack = true;
            zombieManager.attackCooldownTimer = currentAttack.attackCooldown;
            zombieManager.zombieCombatManager.attackDamage = currentAttack.attackDamage;
            zombieManager.zombieAnimatorManager.PlayTargetAttackAnimation(currentAttack.attackAnimation);
            SoundManager.instance.PlaySound(currentAttack.attackSound.name);
            currentAttack = null;
        }
        else
        {
            Debug.LogWarning("Warning: Zombie attempts attack without one.");
        }
    }

    private void ResetStateFlags()
    {
        hasPerformedAttack = false;
    }
}
