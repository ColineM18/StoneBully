using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy infos")]
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    public List<HairColorSprite> spriteList;

    private Spawn spawn => gameManager.spawn;
    private GameObject spawnLimits => gameManager.limits; //spawn limits
    public int nbWave; //apparition wave

    //seek & shoot timers
    public float timerShoot;
    public float timerCloseAttack;
    private float initTimer = 9f;
    private float startStoneQuestTime;

    //Targets infos
    public PlayerCharacter player => gameManager.player;
    private float playerDistance => Vector3.Distance(player.transform.position, transform.position);

    public bool isSeekingStone;
    private Vector3? stonePos => FindClosestStone()?.transform.position;
    private Vector3 startPositionToSeek;
    private float distanceWithStone => Vector3.Distance(startPositionToSeek, stonePos.Value);


    protected override void Start()
    {
        base.Start();

        timerShoot = UnityEngine.Random.Range(1f, initTimer); //strat shooting at a random moment

        if (nbWave >= 3)
            munitions = 1; // it makes them more dynamic
    }

    void Update()
    {
        OrientSlingshot();
        Move();

        if (playerDistance < 2f)
            AttackClosePlayer();

        timerShoot -= Time.deltaTime;
        if (timerShoot <= 0)
            Shoot();

        if (munitions == 0)
            FindStone();
    }

    public override void Shoot()
    {
        timerShoot = initTimer;      

        //shoot in player direction
        shootDirection = player.transform.position - transform.position;

        if (munitions > 0)
        {
            //if enough munition, throw stone
            munitions -= 1;

            Stone stoneThrown;
            stoneThrown = Instantiate(stone, slingshot.position, Quaternion.identity).GetComponent<Stone>();
            stoneThrown.Throw(shootDirection, CharacterType.Enemy);

            stoneThrown.StopDistance(playerDistance, transform.position);
            SoundManager.PlaySound("Fire");
            animator.SetTrigger(hairColor + "_Shooter");
        }
    }

    public void AttackClosePlayer()
    {
        timerCloseAttack += Time.deltaTime;

        if (timerCloseAttack >= 1f)
        {
            player.TakeDamages();
            timerCloseAttack = 0;
        }
    }

    public void FindStone()
    {
        if (stonePos.HasValue) //has find a stone
        {
            if (!isSeekingStone)
            {
                //find a stone
                startStoneQuestTime = Time.time;
                startPositionToSeek = transform.position;
                isSeekingStone = true;
            }

            Vector3 direction = (stonePos.Value - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    public GameObject FindClosestStone()
    {
        Vector3 myPosition = transform.position;
        List<Stone> collectablesStones = gameManager.stonesList.Where(stone => stone.collectable).ToList();
        float previousDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (Stone stone in collectablesStones)
        {
            float currentDistance = (stone.transform.position - myPosition).sqrMagnitude;
            if (currentDistance < previousDistance)
            {
                previousDistance = currentDistance;
                closest = stone.gameObject;            
            }
        }

        return closest;
    }

    public override void PickUp()
    {
        base.PickUp();
        isSeekingStone = false;
    }

    public override void TakeDamages()
    {
        base.TakeDamages();
        if (lifes <= 0)
        {
            SoundManager.PlaySound("EnemyHit");
            gameManager.enemiesCount -= 1;
            Destroy(this.gameObject);
        }
    }

    public override void Move() => MoveDown();
    private void MoveDown()
    {
        //first move
        if (spawnLimits.transform.position.y <= transform.position.y)
            gameObject.transform.position += new Vector3(0, -0.5f * Time.deltaTime * speed, 0 );

        shootDirection = player.transform.position - transform.position; //orientation
    }

    [Serializable]
    public class HairColorSprite
    {
        public HairColor hairColor;
        public Sprite sprite;
    }
}
