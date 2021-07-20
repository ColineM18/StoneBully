using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject stone;
    public float distance;
    public int munitions;
    public int maxMunitions;
    public int lifes;
    public string thrower; //lanceur
    public Camera mainCamera;
    public PlayerCharacter player;
    public float speed = 1;
    public Vector3 mouseDirection;
    public Vector3 shootDirection;
    public bool shoot = false;
    public Transform slingshot; //lancePierre;
    public GameObject startSlingshot; //position lance-pierre
    public Animator animator;

    public Character() : base()   { }
    

    public virtual void PickUp()
    {
        //ramasser munitions
        munitions += 1;
    }

    public virtual void TakeDamages()
    {
        //dégâts
        lifes -= 1;
    }

    //Manage shoot
    public virtual void Shoot()
    {

    }

    public virtual void Move() { }

    //Rotation lance pierre
    public virtual void SlingshotOrientation()
    {
        float zRotation = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotation - 90));
    }
}
