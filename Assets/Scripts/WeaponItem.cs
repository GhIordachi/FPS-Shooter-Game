using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    [Header("Icon")]
    public Sprite weaponIcon;

    [Header("Weapon Animation")]
    public AnimatorOverrideController weaponAnimator;

    [Header("Weapon Damage")]
    public int damage = 20;

    [Header("Ammo")]
    public int remainingAmmo = 6;
    public int maxAmmo = 6;
    public AmmoType ammoType;

    [Header("Sound FX")]
    public AudioClip soundClip;
}
