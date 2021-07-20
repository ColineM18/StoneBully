using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : Character
{
    public string couleur;
    public SpriteRenderer spriteRenderer;
    public Sprite brun;
    public Sprite blond;
    public Sprite noir;

    public bool moving = true; //determine si il est suffisamment proche du joueur
    public GameObject limits; //spawn limits
    public Spawn spawn;
    private int nbVague; //vague d'apparition

    public Vector3 stonePos; //position où faire apparaître le joueur
    public bool isSeeking = false;
    public float timerShoot;
    public float initTimer = 9f;
    public float timerAttack = 0;
    public Vector3 startPosition;
    public float startTime;
    float distanceWithStone;
    float parcours;

    // Start is called before the first frame update
    void Start()
    {
        lifes = 1;
        timerShoot = UnityEngine.Random.Range(1f, initTimer);

        gameManager = GameManager.instance;
        limits = gameManager.limits;
        player = gameManager.player;
        spawn = gameManager.spawn;
        stonePos = new Vector3(0, 0, 0);
        nbVague = spawn.nbWave;

        if (nbVague >= 3)
        {
            munitions = 1; // les rend plus dynamiques
        }
    }

    public void SetColor(string col)
    {
        couleur = col;
        if (couleur == "brun")
            spriteRenderer.sprite = brun;
        else if (couleur == "blond")
            spriteRenderer.sprite = blond;
        else if (couleur == "noir")
            spriteRenderer.sprite = noir;
        
        animator.SetTrigger(couleur);
    }

    void Update()
    {
        SlingshotOrientation();

        if (player != null)
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
        }

        if (moving && transform.position.y >= limits.transform.position.y)
        {
            Move();
        }

        Attack();
        Shoot();

        if (munitions < 1)
            FindStone();

    }

    public void Attack()
    {
        if (distance < 2f)
        {
            timerAttack += 1 * Time.deltaTime;

            if (timerAttack >= 1f && player != null)
            {
                player.TakeDamages();
                timerAttack = 0;
            }
        }
    }

    public GameObject FindClosestStone()
    {
        Stone[] allStones;
        allStones = gameManager.stonesList.ToArray(); //si collectable!!
        GameObject closest = null;
        float previousDistance = Mathf.Infinity;
        Vector3 myPosition = transform.position;

        for (int i = 0; i < allStones.Length; i++)
        {
            if (allStones[i].collectable) { 

                Vector3 diff = allStones[i].transform.position - myPosition;
                float currentDistance = diff.sqrMagnitude;
                if (currentDistance < previousDistance)
                {
                    closest = allStones[i].gameObject;
                    previousDistance = currentDistance;
                }
            }
        }
        return closest;
    }

    public void FindStone()
    {
        if (!isSeeking && FindClosestStone() != null)
        {
            //find a stone
            startTime = Time.time;
            startPosition = transform.position;
            stonePos = FindClosestStone().transform.position; 
            distanceWithStone = Vector3.Distance(startPosition, stonePos); //distance avec la cible
            isSeeking = true;
        }

        parcours = ((Time.time - startTime) * speed)/distanceWithStone;
        transform.position = Vector3.Lerp(startPosition, stonePos, parcours);

    }

    public override void PickUp()
    {
        //ramasser
        base.PickUp();

        stonePos = new Vector3(0, 0, 0);
        isSeeking = false;
    }

    public override void TakeDamages()
    {
        base.TakeDamages();
        if (lifes <= 0)
        {
            SoundManager.PlaySound("EnemyHit");
            gameManager.lengthEnemies -= 1;
            GameObject.Destroy(this.gameObject);
        }
    }

    //enemy shoot
    public override void Shoot()
    {
        timerShoot -= Time.deltaTime;
        shootDirection = player.transform.position - transform.position;

        if (timerShoot <= 0)
        {
            timerShoot = initTimer;
            animator.SetTrigger(couleur+"tire");

            //shoot in player direction
            shootDirection = player.transform.position - transform.position;

            if (munitions > 0)
            {
                //si assez de munitions, lancer caillou
                Stone stoneThrown;

                stoneThrown = Instantiate(stone, transform.GetChild(0).transform.position, Quaternion.identity).GetComponent<Stone>();
                stoneThrown.Throw(new Vector3(shootDirection.x * Time.deltaTime, shootDirection.y * Time.deltaTime, 0), thrower);
                stoneThrown.transform.parent = gameManager.transform;
                gameManager.stonesList.Add(stoneThrown);
                
                munitions -= 1;
                stoneThrown.StopDistance(distance, transform.position);
                SoundManager.PlaySound("Fire");
                shoot = true;
            }
            else
            {
                shoot = false;
            }
        }
    }

    public override void Move()
    {
        //first move
        gameObject.transform.position += new Vector3(0, -0.5f * Time.deltaTime * speed, 0 );
    }
}
