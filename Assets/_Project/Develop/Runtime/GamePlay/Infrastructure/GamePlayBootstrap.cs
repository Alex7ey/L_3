using System;
using System.Collections;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.GamePlay.Infrastructure
{
    public class GamePlayBootstrap : Bootstrap
    {
        private DIContainer _container;
        private GameProcess _gameProcess;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs inputSceneArgs)
        {
            if (inputSceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(inputSceneArgs)} is not match with {typeof(GameplayInputArgs)} type");
         
            GameplayContextRegistration.Process(container, gameplayInputArgs);
           
            _container = container;
            _container.Initialize();
        }

        public override IEnumerator Initialize()
        {
            _gameProcess = _container.Resolve<GameProcess>();
            _gameProcess.Initialize();

            yield break;
        }

        public override void Run()
        { }

    }
}