using Core.FiniteStateMachine;
using Core.Services.SaveLoad;
using Game.Configs;
using UnityEditor;
using UnityEngine;

namespace Game.States
{
    public class ExitGameState : IState<GameData>
    {
        private readonly ISaveLoadServiceProvider<GameData> _saveLoadService;

        public ExitGameState(ISaveLoadServiceProvider<GameData> saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void OnStateExit()
        {
            // void
        }

        public void OnStateEnter(GameData gameData)
        {
            _saveLoadService.Save(gameData);
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}