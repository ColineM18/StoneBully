using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : Character
{
    public Text lifeText;
    public Text nbMunitions;
    public PauseManager pauseMenu;

    void Start()
    {
        gameManager = GameManager.instance;
        slingshot = gameManager.slingshot;
    }

    void Update()
    {
        SlingshotOrientation();
        Move();
        Shoot(); // check shoot
        lifeText.text = "Lifes : " + lifes.ToString();
        nbMunitions.text = "Munitions : " + munitions + "/" + maxMunitions;
        if (lifes <= 0)
        {
            gameManager.EndGame();
        }
    }

    public override void PickUp()
    {
        base.PickUp();
    }

    public override void TakeDamages()
    {
        if (lifes > 0)
        {
            base.TakeDamages();
            SoundManager.PlaySound("PlayerHit");
            animator.SetTrigger("Hit");
        }
    }

    public override void Shoot()
    {
        if (!pauseMenu.gamePaused) //empecher tir
        {
            //shoot in mouse direction
            mouseDirection = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
            mouseDirection -= transform.position;
            shootDirection = mouseDirection;
            

            if (Input.GetMouseButtonDown(0) && !shoot && munitions > 0)
            {
                //si assez de munitions, lancer caillou
                Stone stoneThrown;

                stoneThrown = Instantiate(stone, transform.GetChild(0).transform.position, Quaternion.identity).GetComponent<Stone>();
                stoneThrown.Throw(new Vector3(shootDirection.x * Time.deltaTime, shootDirection.y * Time.deltaTime, 0), thrower);
                stoneThrown.transform.parent = gameManager.transform;
                gameManager.stonesList.Add(stoneThrown);

                //shoot
                munitions -= 1;
                animator.SetTrigger("Shoot");
                SoundManager.PlaySound("Fire");
                shoot = true;
            }
            else
            {
                shoot = false;
            }
        }
    }

    //player move
    public override void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 velocity = new Vector3(horizontal, vertical, 0f).normalized * Time.deltaTime * speed;
        transform.position += velocity;
    }

    public override void SlingshotOrientation()
    {
        if (!pauseMenu.gamePaused)
            base.SlingshotOrientation();
    }
}
