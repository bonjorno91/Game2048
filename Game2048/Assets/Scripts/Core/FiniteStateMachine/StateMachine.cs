using System;
using System.Collections.Generic;

namespace Core.FiniteStateMachine
{
    public class StateMachine : IStateMachine
    {
        private Dictionary<Type, IStateExitable> _states = new Dictionary<Type, IStateExitable>(10);
        private IStateExitable _activeState;
        public StateMachine()
        {
            
        }

        public void AddState<TState>(TState state) where TState : IStateExitable
        {
            _states.Add(typeof(TState),state);
        }

        public void RemoveState<TState>() where TState : IStateExitable
        {
            if (_states.ContainsKey(typeof(TState)))
                _states.Remove(typeof(TState));
        }
        
        public void EnterState<TState>() where TState : IState
        {
            SetState<TState>().OnStateEnter();
        }

        public void EnterState<TState, TPayload>(TPayload payload) where TState : IState<TPayload>
        {
            SetState<TState>().OnStateEnter(payload);
        }

        private TState SetState<TState>() where TState : IStateExitable
        {
            _activeState?.OnStateExit();
            _activeState = GetState<TState>();
            return (TState)_activeState;
        }

        private TState GetState<TState>() where TState : IStateExitable
        {
            return (TState)_states[typeof(TState)];
        }
        
    }
}