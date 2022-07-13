using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject limits;
    public PlayerCharacter player;
    public Spawn spawn;

    public int score;
    public int enemiesCount;
    public string mapToLoad;
    public float replayTimer;

    [Header("UI")]
    public Text lifeText;
    public Text nbMunitions;
    public PauseManager pauseMenu;
    public GameObject defeat;
    public GameObject replayBouton;
    public Text endScore;
    
    public List<Stone> stonesList = new List<Stone>();

    void Awake()
    {
        #region  singleton
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this);
        }
        #endregion
    }

    public void EndGame()
    {
        StartCoroutine(EndMenu());
    }

    public IEnumerator EndMenu()
    {
        //End Panel
        defeat.SetActive(true);
        endScore.text = "Congratulations, you eliminated " + score.ToString() + " enemies. ";

        pauseMenu.gamePaused = true;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(replayTimer);
        
        replayBouton.SetActive(true);
    }

    //Reload
    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mapToLoad);
        Destroy(gameObject);
    }


}
