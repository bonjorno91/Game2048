namespace Core.FiniteStateMachine
{
    public interface IStateMachine
    {
        void AddState<TState>(TState state) where TState : IStateExitable;
        void RemoveState<TState>() where TState : IStateExitable;
        void EnterState<TState>() where TState : IState;
        void EnterState<TState, TPayload>(TPayload payload) where TState : IState<TPayload>;
    }
}