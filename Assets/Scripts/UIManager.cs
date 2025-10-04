using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Transform lifeBar;
    [SerializeField] GameObject lifeIcon;
    [SerializeField] TextMeshProUGUI waveTMP;
    [SerializeField] TextMeshProUGUI munitionsTMP;
    [SerializeField] Transform replayButton;
    [SerializeField] Transform defeatScreen;
    [SerializeField] TextMeshProUGUI endScoreTMP;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void Pause()
    {
        if (gameManager.gamePaused == false)
        {
            Time.timeScale = 0;
            gameManager.gamePaused = true;
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
            gameManager.gamePaused = false;
            Time.timeScale = 1;
        }
    }

    public void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        gameManager.gamePaused = false;
        Time.timeScale = 1;
    }

    public void SetWave(int nbWave)
    {
        waveTMP.text = "Wave : " + nbWave;
    }

    public void SetLifes(int lifes)
    {
        //gameManager.lifeText.text = "Lifes : " + lifes.ToString();

        for (int i = lifeBar.childCount - 1; i > 0; i--)
        {
            Destroy(lifeBar.GetChild(i).gameObject);
        }

        for(int i = 0; i < lifes; i++)
        {
            Instantiate(lifeIcon,lifeBar);
        }
    }

    public void SetStones(int munitions)
    {
        munitionsTMP.text = "Stones : " + munitions + "/" + gameManager.maxMunitions;
    }

    public void Defeat(int score)
    {
        defeatScreen.gameObject.SetActive(true);
        endScoreTMP.text = "Congratulations, <br> you eliminated " + score.ToString() + " enemies.";
    }

    public void ShowReplayButton()
    {
        replayButton.gameObject.SetActive(true);
    }   
}
