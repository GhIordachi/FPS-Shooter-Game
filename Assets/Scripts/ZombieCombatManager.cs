using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCombatManager : MonoBehaviour
{
    [Header("Attack Damage")]
    public int attackDamage = 20;

    ZombieAttackCollider rightHandAttackCollider;
    ZombieAttackCollider leftHandAttackCollider;

    private void Awake()
    {
        LoadAttackColliders();
    }

    private void LoadAttackColliders()
    {
        ZombieAttackCollider[] attackColliders = GetComponentsInChildren<ZombieAttackCollider>();

        foreach(var attackCollider in attackColliders)
        {
            if(attackCollider.isRightHandedAttackCollider)
            {
                rightHandAttackCollider = attackCollider;
            }
            else
            {
                leftHandAttackCollider = attackCollider;
            }
        }
    }

    public void OpenAttackColliders()
    {
        rightHandAttackCollider.attackCollider.enabled = true;
        leftHandAttackCollider.attackCollider.enabled = true;
    }

    public void CloseAttackColliders()
    {
        rightHandAttackCollider.attackCollider.enabled = false;
        leftHandAttackCollider.attackCollider.enabled = false;
    }
}
