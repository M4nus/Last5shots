using UnityEngine;

public class GameState : MonoBehaviour
{
    private State currentState;

    //private PlayerController playerController;
    //private Rigidbody rbJames;

    private void Start()
    {
        SetState(new Arena(this));

        //playerController = GameObject.FindGameObjectWithTag("James").GetComponent<PlayerController>();
        //rbJames = GameObject.FindGameObjectWithTag("James").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        currentState.Tick();

        //playerController.RotatePlayer(rbJames);
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