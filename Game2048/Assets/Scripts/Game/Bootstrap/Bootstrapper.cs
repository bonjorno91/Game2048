using System;
using Core.Bootstrap;
using Core.FiniteStateMachine;
using Core.Services.InputService;
using Core.Services.UpdateService;
using Game.States;
using UnityEngine;

namespace Game.Bootstrap
{
    public sealed class Bootstrapper : BootstrapperBase
    {
        private InputService _inputService;
        private LoadGameState _loadGameState;

        private IStateMachine _stateMachine;
        private UpdateService _updateService;

        private void Update()
        {
            OnUpdate?.Invoke(Time.deltaTime);
        }

        public override event Action<float> OnUpdate;

        protected override void OnLoad()
        {
            _stateMachine = new StateMachine();
            _updateService = new UpdateService(this);
            _inputService = new InputService(_updateService);
            _loadGameState = new LoadGameState(_stateMachine, _inputService, _updateService);
            _stateMachine.AddState(_loadGameState);
            _stateMachine.EnterState<LoadGameState>();
        }
    }
}