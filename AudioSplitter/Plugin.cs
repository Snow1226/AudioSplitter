using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace AudioSplitter
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        private Harmony _harmony;
        internal AudioSplitterController _controller;

        [Init]
        public void Init(Config conf, IPALogger logger)
        {
            Instance = this;
            Log = logger;
            Log.Info("AudioSplitter initialized.");
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            _harmony = new Harmony("com.snow1226.beatsaber.audiosplitter");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            Log.Debug("OnApplicationStart");
            _controller = new GameObject("AudioSplitterController").AddComponent<AudioSplitterController>();

        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");
            _harmony.UnpatchSelf();
        }
    }
}
