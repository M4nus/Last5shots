using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private State currentState;
    public GameObject James;
    public GameObject deathScreen;
    

    private void Awake()
    {
        if(SceneManager.GetSceneByName("Menu") == SceneManager.GetActiveScene())
            SetState(new Menu(this));
        else if(SceneManager.GetSceneByName("Arena") == SceneManager.GetActiveScene())
            SetState(new Arena(this));

        //DontDestroyOnLoad(gameObject);
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
        {
            currentState.OnStateEnter();
        }
    }
}