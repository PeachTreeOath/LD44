using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GotoTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void GotoGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GotoTutorial()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
