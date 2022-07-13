using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class Spawn : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject enemyPrefab;
    public GameObject player;
    public Text waveTxt;

    private Vector3 spawnRange => new Vector3(Random.Range(-7.5f, 7.5f), 10f, 0f);

    public int maxNumberEnemy; //global
    public int nbWavesBetweenNewLife;
    private int nbWavesBeforeNewLife = 1;

    private float spawnTimeFrequency = 1f;
    private float spawnTime;

    public int nbEnemiesToAddEachTurn;
    public int numberEnemiesThisWave;
    public int nbWave;
    private int nbEnemies;
        
    public float initTimeBetweenWaves;
    private float timeBetweenWaves;

    public bool waveStarted;

    void Start()
    {
        spawnTime = spawnTimeFrequency;    //frequency
        timeBetweenWaves = initTimeBetweenWaves;
        gameManager = GameManager.instance;
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
        timeBetweenWaves -= Time.deltaTime; //delay

        if (timeBetweenWaves <= 0)
        {
            // Next wave ready
            waveTxt.text = "Wave : " + nbWave;

            waveStarted = true;
            nbEnemies = 0;
            nbWave++;
            nbWavesBeforeNewLife++;   
            timeBetweenWaves = initTimeBetweenWaves; //reset delay

            Heal();
        }
    }
    public void Heal()
    {
        if (nbWavesBeforeNewLife == nbWavesBetweenNewLife)
        {
            //get a bonus life every 5 levels
            gameManager.player.lifes += 1;
            nbWavesBeforeNewLife = 0;
        }
    }

    public void StartSpawn()
    {
        spawnTime -= Time.deltaTime; //delay between spawns

        if (spawnTime <= 0)
        {
            if(nbEnemies < numberEnemiesThisWave)
            {
                spawnTime = spawnTimeFrequency;

                nbEnemies++;
                gameManager.enemiesCount++;

                SpawnEnemy();

                if (nbWave >= 14)
                    spawnTimeFrequency = 0.25f; //increase difficulty
            }
            else //end wave
            {
                waveStarted = false;
                if (numberEnemiesThisWave < maxNumberEnemy)
                    numberEnemiesThisWave += nbEnemiesToAddEachTurn;
            }
        } 

        
    }

    void SpawnEnemy()
    {
        GameObject newEnemy;
        newEnemy = Instantiate(enemyPrefab, spawnRange, Quaternion.identity);
        newEnemy.name = "Enemy n°" + nbEnemies;
        newEnemy.transform.parent = transform; //set spawner as parent
        SetHairColor(newEnemy.GetComponent<Enemy>());
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
