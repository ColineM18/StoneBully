using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class Spawn : MonoBehaviour
{ 
    [HideInInspector]
    public GameObject enemyPrefab;
    [HideInInspector]
    public GameObject player;

    public Transform spawnBorderL;
    public Transform spawnBorderR;

    public UIManager ui;

    [Header("Settings")]
    public int maxNumberEnemy; //global
    public int enemyCount;
    public int nbEnemiesToAddEachTurn;

    public float timeBetweenWaves;

    [SerializeField]
    private float spawnTimeEnnemy = 1f; 

    GameManager gameManager;

    [Header("Monitor")]
    [SerializeField]
    bool waveStarted;
    [SerializeField]
    int nbWave;
    [SerializeField]
    int numberEnemiesThisWave;
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    int nbEnemies;
    float timeRemainingBeforeNextWave;
    int nbWavesBeforeNewLife;

    void Start()
    {
        spawnTime = spawnTimeEnnemy;    //frequency
        timeRemainingBeforeNextWave = timeBetweenWaves;
        numberEnemiesThisWave = enemyCount;
        gameManager = GameManager.instance;
        nbWavesBeforeNewLife = gameManager.nbWavesBetweenNewLife+1;
    }

    void Update()
    {
        if (gameManager.enemiesCount == 0 && !waveStarted)
            StartWave();

        if(waveStarted)
            StartSpawn();
    }

    public void StartWave()
    {
        timeRemainingBeforeNextWave -= Time.deltaTime; //delay

        if (timeRemainingBeforeNextWave <= 0)
        {
            // Next wave ready
            ui.SetWave(nbWave);

            waveStarted = true;
            nbEnemies = 0;
            nbWave++;
             
            timeRemainingBeforeNextWave = timeBetweenWaves; //reset delay

            if (nbWavesBeforeNewLife == 0)
            {
                Heal();
            }
            nbWavesBeforeNewLife--;
        }
    }

    public void StartSpawn()
    {
        spawnTime -= Time.deltaTime; //delay between spawns

        if (spawnTime <= 0)
        {
            if(nbEnemies < numberEnemiesThisWave)
            {
                spawnTime = spawnTimeEnnemy;

                nbEnemies++;
                gameManager.enemiesCount++;

                SpawnEnemy();

                if (nbWave >= 14)
                    spawnTimeEnnemy = 0.25f; //increase difficulty
            }
            else //end wave
            {
                waveStarted = false;
                if (numberEnemiesThisWave < maxNumberEnemy)
                    numberEnemiesThisWave += nbEnemiesToAddEachTurn;
            }
        } 

        
    }

    public void Heal()
    {
        //get a bonus life every 5 levels
        gameManager.player.Heal();
        nbWavesBeforeNewLife = gameManager.nbWavesBetweenNewLife;
    }

    void SpawnEnemy()
    {
        Enemy newEnemy;
        Vector3 spawnRange = new Vector3(Random.Range(spawnBorderL.position.x, spawnBorderR.position.x), transform.position.y, 0f);
        newEnemy = Instantiate(enemyPrefab, spawnRange, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.name = "Enemy n°" + nbEnemies;
        newEnemy.transform.parent = transform; //set spawner as parent
        newEnemy.nbWave = nbWave;
        SetHairColor(newEnemy);
        
    }

    public void SetHairColor(Enemy enemy)
    {
        if (Random.Range(1, 3) == 1)
            enemy.hairColor = Character.HairColor.Blond;

        else if (Random.Range(1, 3) == 2)
            enemy.hairColor = Character.HairColor.Brown;

        else
            enemy.hairColor = Character.HairColor.Black;

        enemy.spriteRenderer.sprite = enemy.spriteList.First(sprite => sprite.hairColor == enemy.hairColor).sprite;

        enemy.animator.SetTrigger(enemy.hairColor.ToString());
    }

}
