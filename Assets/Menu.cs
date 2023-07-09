using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public GameObject lvlSelectionGO;
    public GameObject MenuGO;

    public void SetLvlSelection(bool LvlGo)
    {
        if(LvlGo == true)
        {
            lvlSelectionGO.SetActive(true);
            MenuGO.SetActive(false);
        }
        else
        {
            lvlSelectionGO.SetActive(false);
            MenuGO.SetActive(true);


        }
    }
    public void StartGame(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
