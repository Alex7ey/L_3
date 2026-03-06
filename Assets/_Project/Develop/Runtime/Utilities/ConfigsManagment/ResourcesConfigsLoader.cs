using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets._Project.Develop.Runtime.Utilities.AssetsLoader;
using Assets._Project.Develop.Runtime.Configs;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagment
{
    public class ResourcesConfigsLoader : IConfigsLoader
    {
        private readonly ResourcesAssetsLoader _resources = new();

        private Dictionary<Type, string> _configsResourcesPaths = new()
        {
            {typeof(CombinationConfig), "Configs/CombinationConfig" },
            {typeof(StartWalletConfig), "Configs/StartWalletConfig" },
            {typeof(RewardsAndCostsConfig),"Configs/RewardsAndCostsConfig"}
        };

        public ResourcesConfigsLoader(ResourcesAssetsLoader resources)
        {
            _resources = resources;
        }

        public IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded)
        {
            Dictionary<Type, object> loadedConfigs = new();

            foreach (var configsResourcesPaths in _configsResourcesPaths)
            {
                ScriptableObject config = _resources.Load<ScriptableObject>(configsResourcesPaths.Value);
                loadedConfigs.Add(configsResourcesPaths.Key, config);
                yield return null;
            }

            onConfigsLoaded?.Invoke(loadedConfigs);
        }

    }
}
