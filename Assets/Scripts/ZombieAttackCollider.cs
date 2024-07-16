using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackCollider : MonoBehaviour
{
    ZombieManager zombie;
    public Collider attackCollider;
    public bool isRightHandedAttackCollider;

    private void Awake()
    {
        zombie = GetComponentInParent<ZombieManager>();
        attackCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player.isPerformingAction)
                {
                    player.playerAnimatorManager.PlayAnimation("Get Hit", false);
                    player.playerStatsManager.GetDamaged(zombie.zombieCombatManager.attackDamage);
                }
            }
        }
    }
}
