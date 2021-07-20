using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string MapToLoad;


    public void LaunchLevel()
    {
        Time.timeScale = 1;
        GameManager GM = GameManager.instance;
        if (GM != null)
            Destroy(GM.gameObject);
        SceneManager.LoadScene(MapToLoad);
    }

    public void ShowButtons()
    {
        gameObject.SetActive(false);
    }

}
