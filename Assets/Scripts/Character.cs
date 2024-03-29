﻿using UnityEngine;

public class Character : MonoBehaviour
{   
    [Header("General")]
    public GameManager gameManager;
    public Animator animator;
    public GameObject stone;

    [Header("Characters infos")]
    public HairColor hairColor;
    public int lifes;
    public int munitions;
    public int maxMunitions;

    [Header("Slingshot infos")]
    public Transform slingshot;
    public float speed;
    protected Vector3 shootDirection;

    public virtual void Shoot() { }
    public virtual void Move() { }
    public virtual void PickUp() => munitions += 1;
    public virtual void TakeDamages() => lifes -= 1;

    public virtual void OrientSlingshot()
    {
        float zRotation = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotation - 90));
    }

    public enum HairColor
    {
        Black,
        Brown,
        Blond,
        Red
    }

    public enum CharacterType
    {
        Player,
        Enemy
    }


}
