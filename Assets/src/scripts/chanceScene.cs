using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chanceScene : MonoBehaviour
{
    // Change scene with button
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Close the game
    public void CloseGame()
    {
        Application.Quit();
    }

}
