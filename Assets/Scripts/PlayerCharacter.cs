using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerCharacter : Character
{
    public Camera mainCamera;
    public ControlType target;
    public VirtualJoystick virtualJoystick;
    public enum ControlType
    {
        PC,
        Console,
        Mobile
    }

    public Vector3 mouseDirection
    {
        get
        {
            Vector3 inputPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z);
            return mainCamera.ScreenToWorldPoint(inputPosition) - transform.position;
        }
    }

    public Vector3 joystickDirection
    {
        get
        {
            if (virtualJoystick.Horizontal != 0 && virtualJoystick.Horizontal != 0)
                return new Vector3(virtualJoystick.Horizontal, virtualJoystick.Vertical, 0f).normalized;
            else
                return shootDirection;
        }
    }

    protected override void Start()
    {
        base.Start();
        shootDirection = new Vector3(0,90,0);
    }

    void Update()
    {
        if (!gameManager.gamePaused)
        {
            OrientSlingshot();
            Move();
            TryShoot(); // check shoot
            gameManager.ui.SetLifes(lifes);
            gameManager.ui.SetStones(munitions);
        }
    }

    public override void Shoot()
    {
        shootDirection = mouseDirection;

        if (munitions > 0)
        {
            //if enough munition, throw stone
            munitions--;

            Stone stoneThrown = Instantiate(stone, slingshot.position, Quaternion.identity).GetComponent<Stone>();
            stoneThrown.Throw(shootDirection, CharacterType.Player);

            animator.SetTrigger("Shoot");
            SoundManager.PlaySound("Fire");
        }
    }
    void TryShoot()
    {
        if (target == ControlType.PC)
            ShootToMouse();
        else if (target == ControlType.Mobile)
            shootDirection = joystickDirection;
    }
    void ShootToMouse()
    {
        //shoot in mouse direction
        shootDirection = mouseDirection;

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    public void ShootTo(Vector2 direction)
    {
        shootDirection = direction;
        Shoot();
    }

    public override void TakeDamages()
    {
        if (lifes > 0)
        {
            base.TakeDamages();
            SoundManager.PlaySound("PlayerHit");
            animator.SetTrigger("Hit");
        }
        else
            gameManager.EndGame();
    }

    public override void Move()
    {
        switch (target)
        {
            case ControlType.PC:
                MoveDefault();
                break;
            case ControlType.Mobile:
                MoveMobileJoystick();
                break;
        }
    }

    void MoveDefault()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 velocity = new Vector3(horizontal, vertical, 0f).normalized * Time.deltaTime * speed;
        transform.position += velocity;
    }

    void MoveMobileJoystick()
    {
        Vector3 velocity = new Vector3(virtualJoystick.Horizontal, virtualJoystick.Vertical, 0f).normalized * Time.deltaTime * (speed*1.375f);
        transform.position += velocity;

        /*/Vector3 direction = Vector3.forward * virtualJoystick.Vertical + Vector3.right * virtualJoystick.Horizontal;
        //rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);*/
    }

    internal void Heal()
    {
        if(lifes < 4)
        lifes += 1;
    }
}
