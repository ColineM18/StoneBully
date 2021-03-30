using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject stone;
    public float distance;
    public int munitions;
    public int maxMunitions;
    protected bool checkShoot = false;
    public int lifes;
    public string thrower; //lanceur
    public Camera mainCamera;
    public GameObject player;
    public float speed = 1;
    public Vector3 mouseDirection;
    public Vector3 shootDirection;
    public bool shoot = false;
    public Transform slingshot; //lancePierre;
    public GameObject startSlingshot; //position lance-pierre
    public Animator animator;

    public Character() : base()
    {

    }
    
    void Start()
    {
        munitions = 3;
        maxMunitions = 3;
    }
    
    void Update()
    {
        Move();
    }

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
    public virtual void Shoot(string shooter)
    {
        if (shooter == "player")
        {
                //shoot in mouse direction
                mouseDirection = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
                mouseDirection -= transform.position;
                shootDirection = mouseDirection;
        }
        else if (player != null)
        {
            //shoot in player direction (enemy)
                shootDirection = player.transform.position - transform.position;
        }

        if (Input.GetMouseButtonDown(0) && !shoot && player.gameObject.GetComponent<PlayerCharacter>().munitions > 0 && shooter == "player" || shooter == "enemy" && this.gameObject.GetComponent<Enemies>().munitions > 0)
        {
            //si assez de munitions, lancer caillou
            GameObject stoneThrown;

            stoneThrown = GameObject.Instantiate(stone, transform.GetChild(0).transform.position, Quaternion.identity);

            stoneThrown.GetComponent<Stone>().Throw(new Vector3(shootDirection.x * Time.deltaTime, shootDirection.y * Time.deltaTime, 0), thrower);

            SoundManager.PlaySound("Fire");

            if (shooter == "player")
            {
                //shoot
                GetComponent<PlayerCharacter>().munitions -= 1;
                animator.SetTrigger("Shoot");
            }
            else
            {
                GetComponent<Enemies>().munitions -= 1;
                stoneThrown.GetComponent<Stone>().StopDistance(distance, transform.position);
            }

            shoot = true;
        }
        else
        {
                shoot = false;
        }
    }

    public virtual void Move() { }

    //Rotation lance pierre
    public virtual void SlingshotOrientation()
    {
        float zRotation = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotation - 90));
    }
}
