namespace Core.FiniteStateMachine
{
    public interface IState : IStateExitable
    {
        void OnStateEnter(IStateMachine stateMachine);
    }
    
    public interface IState<TPayload> : IStateExitable
    {
        void OnStateEnter(IStateMachine stateMachine, TPayload payload);
    }
}