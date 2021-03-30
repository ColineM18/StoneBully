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
        SceneManager.LoadScene(MapToLoad);
    }

}
