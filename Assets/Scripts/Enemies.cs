using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : Character
{
    public string couleur;
    public Sprite brun;
    public Sprite blond;
    public Sprite noir;

    public bool moving = true; //determine si il est suffisamment proche du joueur
    public GameObject limits; //spawn limits
    public GameObject spawn;
    private int nbVague; //vague d'apparition

    public Vector3 stonePos; //position où faire apparaître le joueur
    public bool checkSeek = false;
    public float timerShoot;
    public float initTimer = 9f;
    public float timerATK = 0;
    public Vector3 startPosition;
    public float startTime;
    float distSeek;
    float parcours;

    // Start is called before the first frame update
    void Start()
    {
        lifes = 1;
        timerShoot = UnityEngine.Random.Range(1f, initTimer);
        limits = GameObject.FindGameObjectWithTag("limits");
        player = GameObject.Find("Player");
        spawn = GameObject.Find("Spawn Enemies");
        stonePos = new Vector3(0, 0, 0);
        nbVague = spawn.GetComponent<Spawn>().nbVague;
        if (nbVague >= 3)
        {
            munitions = 1; // les rend plus dynamiques
        }
    }

    public void SetColor(string col)
    {
        couleur = col;
        if (couleur == "brun")
            GetComponent<SpriteRenderer>().sprite = brun;
        else if (couleur == "blond")
            GetComponent<SpriteRenderer>().sprite = blond;
        else if (couleur == "noir")
            GetComponent<SpriteRenderer>().sprite = noir;
        
        animator.SetTrigger(couleur);
    }

    // Update is called once per frame
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
            timerATK += 1 * Time.deltaTime;

            if (timerATK >= 1f && player != null)
            {
                player.GetComponent<PlayerCharacter>().TakeDamages();
                timerATK = 0;
            }
        }
    }

    public GameObject FindClosestStone()
    {
        GameObject[] tab;
        tab = GameObject.FindGameObjectsWithTag("stone"); //si collectable!!
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        for (int i = 0; i < tab.Length; i++)
        {
            if (tab[i].GetComponent<Stone>().collectable == true) { 

                Vector3 diff = tab[i].transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = tab[i];
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    public void FindStone()
    {
        if (!checkSeek && FindClosestStone() != null)
        {
            //find a stone
            startTime = Time.time;
            startPosition = transform.position;
            stonePos = FindClosestStone().transform.position; 
            distSeek = Vector3.Distance(startPosition, stonePos); //distance avec la cible
            checkSeek = true;
        }

        parcours = ((Time.time - startTime) * speed)/distSeek;
        transform.position = Vector3.Lerp(startPosition, stonePos, parcours);

    }

    public override void PickUp()
    {
        //ramasser
        base.PickUp();

        stonePos = new Vector3(0, 0, 0);
        checkSeek = false;
    }

    public override void TakeDamages()
    {
        base.TakeDamages();
        if (lifes <= 0)
        {
            SoundManager.PlaySound("EnemyHit");
            GameObject.Destroy(this.gameObject);
        }
    }

    //enemy shoot
    public void Shoot()
    {
        timerShoot -= Time.deltaTime;
        shootDirection = player.transform.position - transform.position;
        if (timerShoot <= 0)
        {
            timerShoot = initTimer;
            animator.SetTrigger(couleur+"tire");
            base.Shoot("enemy");
        }
    }

    public override void Move()
    {
        //first move
        gameObject.transform.position += new Vector3(0, -0.5f * Time.deltaTime * speed, 0 );
    }
}
