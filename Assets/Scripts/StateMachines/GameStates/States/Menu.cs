using UnityEngine;

public class Menu : State
{
    DeathScreen deathScreen;

    public Menu(GameState gameState) : base(gameState)
    {

    }

    public override void Tick()
    {
    }

    public override void OnStateEnter()
    {
        deathScreen = gameState.deathScreen.GetComponent<DeathScreen>();

        deathScreen.onPlay += OnPlay;
    }

    public override void OnStateExit()
    {

        deathScreen.onPlay -= OnPlay;
    }

    public void OnPlay()
    {

        Debug.Log("Changing to Arena!");
        gameState.deathScreen.SetActive(false);
        gameState.SetState(new Arena(gameState));
    }
}