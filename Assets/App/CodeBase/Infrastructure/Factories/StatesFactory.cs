using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class StatesFactory
    {
        private IInstantiator instantiator;

        public StatesFactory(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public TState Create<TState>() where TState : IExitableState
        {
            return instantiator.Instantiate<TState>();
        }
    }
}