using Infrastructure.States;

namespace Infrastructure.Factory.State
{
    public interface IStateFactory
    {
        T GetState<T>() where T : class, IExitableState;
    }
}