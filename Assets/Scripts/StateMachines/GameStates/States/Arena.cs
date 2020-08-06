using UnityEngine;
using System.Collections;

public class Arena : State
{
    public Arena(GameState gameState) : base(gameState)
    {

    }

    public override void Tick()
    {
        
    }

    public override void OnStateEnter()
    {
        gameState.StartCoroutine(WaitForInitialization());
    }

    void Start()
    {
        gameState.deathScreen.SetActive(false);
    }

    public override void OnStateExit()
    {
        PlayerController.instance.onJamesDeath -= OnPlayerDeath;
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Changing to GameOver!");
        gameState.SetState(new GameOver(gameState));
    }

    public IEnumerator WaitForInitialization()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.instance.onJamesDeath += OnPlayerDeath;
    }
}