using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject enemy;
    public GameObject player;
    public bool moreLife;
    int nbWavesBeforeNewLife = 1;
    public float initTimerEnemy;
    private float timerEnemy;
    public int nbWave = 0;
    public int nbEnemies = 0;
    public int moreNmiEachWave;
    public int numberEnemiesThisWave; //d'une vague
    public int maxNumberEnemy; //global
    public float initTimeBetweenWaves;
    private float timeBetweenWaves;
    public bool wave;
    private bool checkNextWave = false;
    public Text waveTxt;
    

    void Start()
    {
        timerEnemy = initTimerEnemy;    //frequence ennemis
        timeBetweenWaves = initTimeBetweenWaves;    //pause entre les vagues
        gameManager = GameManager.instance;
    }

    void Update()
    {
        waveTxt.text = "Wave : " + nbWave;


        if (gameManager.lengthEnemies == 0 && !checkNextWave)
        {
            if (timeBetweenWaves <= 0){
                // vague suivante;
                wave = true;
                nbEnemies = 0;
                nbWave += 1;
                nbWavesBeforeNewLife += 1;
                checkNextWave = true;
                timeBetweenWaves = initTimeBetweenWaves;
            }
            else
            {
                timeBetweenWaves -= Time.deltaTime;
            }
        }//lancer vague

        if (nbWavesBeforeNewLife == 5)
        {
            //au bout de 5 niveaux, récupérer une vie
            player.GetComponent<PlayerCharacter>().lifes += 1;
            nbWavesBeforeNewLife = 0;
        }

        if (timerEnemy > 0)
        {
            timerEnemy = timerEnemy - Time.deltaTime;
        }//timer entre spawn
        else if (wave && nbEnemies < numberEnemiesThisWave)
        {
            // spawn
            checkNextWave = false;
            string hairColor;

            if (Random.Range(1,3) == 1)
            {
                hairColor = "brun";
            }
            else if (Random.Range(1,3) == 2)
            {
                hairColor = "noir";
            }
            else
            {
                hairColor = "blond";
            }

            nbEnemies += 1;
            gameManager.lengthEnemies += 1;
            timerEnemy = initTimerEnemy;

            GameObject newEnemy;
            newEnemy = GameObject.Instantiate(enemy, new Vector3(UnityEngine.Random.Range(-7.5f,7.5f),10f,0f),Quaternion.identity);
            newEnemy.name = "Enemy n°" + nbEnemies;
            newEnemy.GetComponent<Enemies>().SetColor(hairColor);
            newEnemy.transform.parent = transform; //set spawner as parent

            if (nbWave >= 14)
            {
                initTimerEnemy = 0.25f;
            }

        }//générer vague

        else if (wave)
        {
            wave = false;
            if (numberEnemiesThisWave+1 <= maxNumberEnemy) {
                numberEnemiesThisWave += moreNmiEachWave;
            }
        } //fin vague

    }

}
