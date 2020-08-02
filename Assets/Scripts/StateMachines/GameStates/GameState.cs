using UnityEngine;

public class GameState : MonoBehaviour
{
    private State currentState;

    private void Start()
    {
        SetState(new Arena(this));
    }

    private void Update()
    {
        currentState.Tick();
    }

    public void SetState(State state)
    {
        if(currentState != null)
            currentState.OnStateExit();

        currentState = state;

        if(currentState != null)
            currentState.OnStateEnter();
    }
}