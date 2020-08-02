using UnityEngine;

public class Menu : State
{
    public Menu(GameState gameState) : base(gameState)
    {

    }

    public override void Tick()
    {
        //gameState.SetState(new Intro(GameState));
    }

    public override void OnStateEnter()
    {

    }

    public override void OnStateExit()
    {

    }
}