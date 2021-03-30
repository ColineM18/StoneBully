using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    public GameObject Enemies;
    public GameObject Player;
    public bool moreLife;
    int nbWavesBeforeNewLife = 1;
    public float initTimerNmi;
    private float timerNmi;
    public int nbVague = 0;
    public int nbNmi = 0;
    public int moreNmiEachWave;
    public int maxNmiV; //d'une vague
    public int maxNmi; //global
    public float timeBetweenWaves;
    private float initTimeBetweenWaves;
    public bool wave;
    private bool checkNextWave = false;
    public Text waveTxt;
    

    void Start()
    {
        initTimeBetweenWaves = timeBetweenWaves;
        timerNmi = initTimerNmi;    //frequence ennemis
    }

    void Update()
    {
        waveTxt.text = "Wave : " + nbVague;


        if (GameObject.FindGameObjectsWithTag("enemy").Length == 0 && !checkNextWave)
        {
            if (timeBetweenWaves <= 0){
                // vague suivante;
                wave = true;
                nbNmi = 0;
                nbVague += 1;
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
            Player.GetComponent<PlayerCharacter>().lifes += 1;
            nbWavesBeforeNewLife = 0;
        }

        if (timerNmi > 0)
        {
            timerNmi = timerNmi - Time.deltaTime;
        }//timer entre spawn
        else if (wave && nbNmi < maxNmiV)
        {
            // spawn
            checkNextWave = false;
            string hColor;

            if (Random.Range(1,3) == 1)
            {
                hColor = "brun";
            }
            else if (Random.Range(1,3) == 2)
            {
                hColor = "noir";
            }
            else
            {
                hColor = "blond";
            }

            nbNmi += 1;
            timerNmi = initTimerNmi;
            GameObject nmi;
            nmi = GameObject.Instantiate(Enemies, new Vector3(UnityEngine.Random.Range(-7.5f,7.5f),10f,0f),Quaternion.identity);
            nmi.name = "Enemy n°" + nbNmi;
            nmi.GetComponent<Enemies>().SetColor(hColor);


            if (nbVague >= 14)
            {
                initTimerNmi = 0.25f;
            }

        }//générer vague

        else if (wave)
        {
            wave = false;
            if (maxNmiV+1 <= maxNmi) {
                maxNmiV += moreNmiEachWave;
            }
        } //fin vague

    }

}
