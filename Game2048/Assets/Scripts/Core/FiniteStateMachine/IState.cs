namespace Core.FiniteStateMachine
{
    public interface IState : IStateExitable
    {
        void OnStateEnter();
    }
    
    public interface IState<TPayload> : IStateExitable
    {
        void OnStateEnter(TPayload payloadData);
    }
}