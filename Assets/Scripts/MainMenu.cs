using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame(){
        Debug.Log("Game closed!");
        Application.Quit();
    }

    public void BackToMenu(){
        SceneManager.LoadScene("Menu");
    }

    public void PlayScene2(){
        SceneManager.LoadScene("Level2");
    }

    public void PlayScene3(){
        SceneManager.LoadScene("Level3");
    }
}
