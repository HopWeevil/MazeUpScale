using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _gameStateMachine = gameStateMachine;
            _statesFactory = statesFactory;
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            RegisterStates();

            _gameStateMachine.Enter<BootstrapState>();
        }

        private void RegisterStates()
        {
            _gameStateMachine.RegisterState(_statesFactory.Create<BootstrapState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<LoadLevelState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameLoopState>());
        }
    }
}