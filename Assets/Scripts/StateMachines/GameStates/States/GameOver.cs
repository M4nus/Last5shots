using UnityEngine;
using System.Collections;

public class GameOver : State
{
    public GameOver(GameState gameState) : base(gameState)
    {

    }

    public override void Tick()
    {

    }

    public override void OnStateEnter()
    {
        gameState.StartCoroutine(LerpTime());
        if(DeathScreen.instance.gameObject != null)
        DeathScreen.instance.gameObject.SetActive(true);
        DeathScreen.instance.onRetry += OnRetry;
        DeathScreen.instance.onMenu += OnMenu;
    }

    public override void OnStateExit()
    {

        DeathScreen.instance.onRetry -= OnRetry;
        DeathScreen.instance.onMenu -= OnMenu;
    }

    public void OnRetry()
    {

        Debug.Log("Reloading!");
        Time.timeScale = 1f;
        gameState.deathScreen.SetActive(false);
        gameState.SetState(new Arena(gameState));
    }

    public void OnMenu()
    {

        Debug.Log("Changing to Menu!");
        Time.timeScale = 1f;
        gameState.deathScreen.SetActive(false);
        gameState.SetState(new Menu(gameState));
    }

    IEnumerator LerpTime()
    {
        float currentTime = 1f;
        while(currentTime > 0f)
        {
            Time.timeScale = currentTime;
            currentTime -= Time.unscaledDeltaTime;
            yield return null;
        }
    }
}