using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using NAudio.Wave;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using AudioSplitter.UI;
using AudioSplitter.Configuration;

namespace AudioSplitter
{
    internal class AudioSplitterController : MonoBehaviour
    {
        public static AudioSplitterController Instance { get; private set; }
        private ModMainFlowCoordinator mainFlowCoordinator;
        public AsioOut asioOut = null;
        public BufferedWaveProvider asioBuffer = null;

        public int gameAudioSampleRate = 48000;

        private void Awake()
        {
            if (Instance != null)
            {
                Plugin.Log?.Warn($"Instance of {GetType().Name} already exists, destroying.");
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this); // Don't destroy this object on scene changes
            Instance = this;
            Plugin.Log?.Debug($"{name}: Awake()");

            MenuButton menuButton = new MenuButton("Audio Splitter", "Mod to send audio to another device", ShowModFlowCoordinator, true);
            MenuButtons.instance.RegisterButton(menuButton);
        }

        public void Start()
        {

            if (PluginConfig.Instance.AudioDevice != null && PluginConfig.Instance.AudioDevice != String.Empty)
            {
                gameAudioSampleRate = AudioSettings.outputSampleRate;
                Plugin.Log?.Notice($"InGame SamplingRate : {gameAudioSampleRate}");

                try
                {
                    asioBuffer = new BufferedWaveProvider(new WaveFormat(PluginConfig.Instance.SamplingRate, PluginConfig.Instance.Bits, 2));
                    asioBuffer.DiscardOnBufferOverflow = true;

                    if (PluginConfig.Instance.AudioDevice == string.Empty)
                    {
                        asioOut = new AsioOut(AsioOut.GetDriverNames()[0]);
                        PluginConfig.Instance.AudioDevice = AsioOut.GetDriverNames()[0];
                    }
                    else
                        asioOut = new AsioOut(PluginConfig.Instance.AudioDevice);
                    asioOut.ChannelOffset = PluginConfig.Instance.OutputChannel;
                    asioBuffer.ClearBuffer();
                    asioOut.Init(asioBuffer);
                    asioOut.Play();
                }
                catch (Exception ex)
                {
                    Plugin.Log?.Warn($"Not Open ASIO, {ex.Message}");
                }
            }
        }

        public void OnDestroy()
        {
            asioOut?.Stop();
            asioOut?.Dispose();
        }

        public void RestartASIO()
        {
            
            if (PluginConfig.Instance.AudioDevice != null && PluginConfig.Instance.AudioDevice != String.Empty)
            {
                try
                {
                    asioOut?.Stop();
                    asioOut?.Dispose();
                    asioOut = null;

                    if (PluginConfig.Instance.AudioDevice == string.Empty)
                        asioOut = new AsioOut(AsioOut.GetDriverNames()[0]);
                    else
                        asioOut = new AsioOut(PluginConfig.Instance.AudioDevice);
                    asioOut.ChannelOffset = PluginConfig.Instance.OutputChannel;

                    asioBuffer.ClearBuffer();
                    asioOut.Init(asioBuffer);
                    asioOut.Play();
                    Plugin.Log?.Debug($"Open ASIO, {asioOut.DriverName}");
                }
                catch (Exception ex)
                {
                    Plugin.Log?.Warn($"Not Open ASIO, {ex.Message}");
                }
            }
        }

        public void ShowModFlowCoordinator()
        {
            if (this.mainFlowCoordinator == null)
                this.mainFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<ModMainFlowCoordinator>();
            if (mainFlowCoordinator.IsBusy) return;

            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(mainFlowCoordinator);
        }
    }
}
