using System;
using Core.FiniteStateMachine;
using Core.Services;
using Game;
using Game.LoadState;
using UnityEngine;

namespace Core.Bootstrappers
{
    public sealed class Bootstrapper : BootstrapperBase
    {
        public event Action<float> OnUpdate;
        
        private IStateMachine _stateMachine;
        private UpdateService _updateService;
        private InputService _inputService;
        
        protected override void OnLoad()
        {
            _updateService = new UpdateService(this);
            _inputService = new InputService(_updateService);
            _stateMachine = new StateMachine();
            _stateMachine.AddState(new LoadGameState(_inputService,_updateService));
            _stateMachine.EnterState<LoadGameState>();
        }

        private void Update()
        {
            OnUpdate?.Invoke(Time.deltaTime);
        }
    }
}