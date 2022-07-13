using UnityEngine.UI;
using UnityEngine;

public class PlayerCharacter : Character
{
    public Camera mainCamera;
    public Vector3 mouseDirection
    {
        get
        {
            Vector3 inputPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z);
            return mainCamera.ScreenToWorldPoint(inputPosition) - transform.position;
        }
    }
    public bool shoot;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (!gameManager.pauseMenu.gamePaused)
        {
            OrientSlingshot();
            Move();
            Shoot(); // check shoot
            gameManager.lifeText.text = "Lifes : " + lifes.ToString();
            gameManager.nbMunitions.text = "Munitions : " + munitions + "/" + maxMunitions;
        }
    }

    public override void Shoot()
    {
        //shoot in mouse direction
        shootDirection = mouseDirection;

        if (Input.GetMouseButtonDown(0) && munitions > 0)
        {
            //if enough munition, throw stone
            munitions -= 1;

            Stone stoneThrown;
            stoneThrown = Instantiate(stone, slingshot.position, Quaternion.identity).GetComponent<Stone>();
            stoneThrown.Throw(shootDirection, CharacterType.Player);

            animator.SetTrigger("Shoot");
            SoundManager.PlaySound("Fire");
        }

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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 velocity = new Vector3(horizontal, vertical, 0f).normalized * Time.deltaTime * speed;
        transform.position += velocity;
    }
}
