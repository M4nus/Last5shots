public abstract class State
{
    protected GameState gameState;

    public abstract void Tick();

    public virtual void OnStateEnter() { }

    public virtual void OnStateExit() { }

    public State(GameState gameState)
    {
        this.gameState = gameState;
    }
}