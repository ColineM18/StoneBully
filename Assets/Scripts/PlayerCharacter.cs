using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : Character
{
    public Text lifeText;
    public Text nbMunitions;
    public GameObject pauseMenu;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        slingshot = GameObject.FindGameObjectWithTag("slingshot").GetComponent<Transform>();
        animator = GetComponent<Animator>();
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

    public void Shoot()
    {
        if (!pauseMenu.GetComponent<PauseManager>().gamePaused) //empecher tir
        {
            base.Shoot("player");
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
        if (!pauseMenu.GetComponent<PauseManager>().gamePaused)
            base.SlingshotOrientation();
    }
}
