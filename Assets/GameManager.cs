using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject looseP;
    public GameObject WinP;

    public void LostTheGame()
    {
        looseP.gameObject.SetActive(true);

    }
    public void WonTheGame()
    {
        WinP.gameObject.SetActive(true);
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (sceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
        {
            LoadMenuScene();

        }
    }
}
