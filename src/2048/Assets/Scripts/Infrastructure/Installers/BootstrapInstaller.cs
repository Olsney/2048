using Data;
using Infrastructure.Factory.Game;
using Infrastructure.Factory.State;
using Infrastructure.States;
using Services.CubePools;
using Services.CubeSpawnerProviders;
using Services.GameOver;
using Services.GameRestart;
using Services.InputHandlerProviders;
using Services.Inputs;
using Services.Merge;
using Services.Randoms;
using Services.SpawnPointProviders;
using Services.StaticData;
using UI.Composition;
using UI.Factory;
using UI.Services.Windows;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            BindCoroutine();
            BindFactories();
            BindStates();
            BindServices();
            BindSceneLoader();
        }
        
        private void BindCoroutine() => 
            Container.Bind<ICoroutineRunner>().FromInstance(this).AsSingle();
        
        private void BindFactories()
        {
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<IStateFactory>().To<StateFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
        }
        
        private void BindServices()
        {
            BindInputService();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IPlayerInputHandlerProvider>().To<PlayerInputHandlerProvider>().AsSingle();
            Container.Bind<ICubeSpawnPointProvider>().To<CubeSpawnPointProvider>().AsSingle();
            Container.Bind<IMergeService>().To<MergeService>().AsSingle();
            Container.Bind<IRandomService>().To<RandomService>().AsSingle();
            Container.Bind<ICubeSpawnerProvider>().To<CubeSpawnerProvider>().AsSingle();
            Container.Bind<IGameOverService>().To<GameOverService>().AsSingle();
            Container.Bind<IGameRestartService>().To<GameRestartService>().AsSingle();
            Container.Bind<IWorldData>().To<WorldData>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind<ICubePool>().To<CubePool>().AsSingle();
            Container.Bind<IUIPresenterFactory>().To<UIPresenterFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<CubeValuePresenterLifecycleService>().AsSingle();
        }
        
        private void BindInputService()
        {
            if (Application.isEditor)
                Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
            else
                Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
        }
        
        private void BindSceneLoader() => 
            Container.Bind<SceneLoader>().AsSingle();
    }
}
