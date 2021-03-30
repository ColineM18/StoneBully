using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public int score = 0;
    public GameObject defeat;
    public GameObject replayBouton;
    public Text endScore;
    public string mapToLoad;
    public float replayTimer;
    public PauseManager pause;

    
    void Start()
    {
        Time.timeScale = 1; //éviter problème de reload
    }

    //End Panel
    public void EndGame()
    {
        defeat.SetActive(true);
        ///endScore.text = "Félicitation, vous avez éliminé " + score.ToString() + " ennemis.";
        endScore.text = "Congratulations, you eliminated "+ score.ToString () +" enemies. ";
        pause.gamePaused = true;
        Time.timeScale = 0;
        replayTimer -= Time.fixedDeltaTime;
        
        if (replayTimer <= 0)
        {
            replayBouton.SetActive(true);
        }
    }

    //Reload
    public void Replay()
    {
        SceneManager.LoadScene(mapToLoad);
    }

}
