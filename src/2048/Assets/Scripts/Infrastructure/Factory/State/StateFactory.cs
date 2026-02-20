using Infrastructure.States;
using Zenject;

namespace Infrastructure.Factory.State
{
    public class StateFactory : IStateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container)
        {
            _container = container;
        }

        public T GetState<T>() where T : class, IExitableState =>
            _container.Resolve<T>();
    }
}