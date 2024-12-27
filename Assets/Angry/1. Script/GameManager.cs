using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject _mapUI;
    public GameObject _QUI;
    public void OnButtonStart()
    {
        _mapUI.SetActive(true);
        AudioManager.Instance.MainBGM(); 
    }

    public void OnButtonBack()
    {
        SceneManager.LoadScene("Intro Scene");
        AudioManager.Instance.IntroBGM(); 
    }

    public void Level1Btn()
    {
        SceneManager.LoadScene("Level1");
        AudioManager.Instance.MainBGM();
    }

    public void Level2Btn()
    {
        SceneManager.LoadScene("Level2");
        AudioManager.Instance.MainBGM();
    }

    public void QBtn()
    {
        _QUI.SetActive(true);
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
    public void OnRestart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1f;         // 게임 재개
        AudioManager.Instance.MainBGM(); 
    }

    public void OnNextBtn()
    {
        SceneManager.LoadScene("Level2");
        AudioManager.Instance.MainBGM();
    }

}
