using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIManager ui;
    public GameObject limits;
    public PlayerCharacter player;
    public Spawn spawn;

    public int nbWavesBetweenNewLife;
    public int maxMunitions;
    public int score;

    public int enemiesCount;
    public string mapToLoad;
    public float replayTimer;

    public bool gamePaused;
    
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
        Time.timeScale = 0;
        gamePaused = true;
        ui.Defeat(score);

        yield return new WaitForSecondsRealtime(replayTimer);

        ui.ShowReplayButton();
    }

    //Reload
    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mapToLoad);
        Destroy(gameObject);
    }


}
