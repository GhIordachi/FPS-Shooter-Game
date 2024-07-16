using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Zombie/Actions/Attack Action")]
public class ZombieAttackAction : ScriptableObject
{
    [Header("Attack Animation")]
    public string attackAnimation;

    [Header("Attack Cooldown")]
    public float attackCooldown = 5f; //The time before zombie can make another attack

    [Header("Attack Damage")]
    public int attackDamage = 10;

    [Header("Audio Clip")]
    public AudioClip attackSound;
}
