using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen instance;
    public UnityAction onRetry;
    public UnityAction onMenu;
    public UnityAction onPlay;

    public void Awake()
    {

        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
        
        if(SceneManager.GetSceneByName("Arena") == SceneManager.GetActiveScene())
            gameObject.SetActive(false);
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if(onRetry != null)
            onRetry.Invoke();
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene("Menu");
        if(onMenu != null)
        onMenu.Invoke();
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Arena");
        if(onPlay != null)
            onPlay.Invoke();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
