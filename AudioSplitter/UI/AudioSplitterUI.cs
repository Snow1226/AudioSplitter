using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using AudioSplitter.Configuration;
using NAudio.Wave;
using HMUI;

namespace AudioSplitter.UI
{
    internal class AudioSplitterUI : BSMLAutomaticViewController
    {
        private string[] _channelList = new string[32];
        private List<string> _asioDeviceList = new List<string>();
        private List<string> _asioChannelList = new List<string>();
        public ModMainFlowCoordinator mainFlowCoordinator { get; set; }
        public void SetMainFlowCoordinator(ModMainFlowCoordinator mainFlowCoordinator)
        {
            this.mainFlowCoordinator = mainFlowCoordinator;
        }
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            InitializeList();
            InitializeChannelList();
        }
        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }
        [UIComponent("DeviceList")]
        public CustomListTableData deviceList = null;
        [UIAction("select-devicecell")]
        private void DeviceSelect(TableView table, int row)
        {
            PluginConfig.Instance.AudioDevice = _asioDeviceList[row];
            Plugin.Instance._controller.RestartASIO();
            InitializeChannelList();
        }

        [UIComponent("ChannelList")]
        public CustomListTableData channelList = null;
        [UIAction("select-channelcell")]
        private void ChannelSelect (TableView table, int row)
        {
            PluginConfig.Instance.OutputChannel = row * 2;
            Plugin.Instance._controller.RestartASIO();
        }

        [UIValue("DefaultOutput")]
        private bool defaultOutput = PluginConfig.Instance.DefaultDeviceOutput;
        [UIAction("OnDefaultOutputChange")]
        private void OnDefaultOutputChange(bool value)
        {
            PluginConfig.Instance.DefaultDeviceOutput = defaultOutput = value;
        }

        private void InitializeList()
        {
            deviceList.TableView.ClearSelection();
            deviceList.Data.Clear();

            _asioDeviceList.Clear();

            foreach (string name in AsioOut.GetDriverNames())
            {
                var data1 = new CustomListTableData.CustomCellInfo(name);
                deviceList.Data.Add(data1);
                _asioDeviceList.Add(name);
            }
            deviceList.TableView.ReloadData();
            for(int i = 0; i < _asioDeviceList.Count; i++)
            {
                if(_asioDeviceList[i] == PluginConfig.Instance.AudioDevice)
                {
                    deviceList.TableView.SelectCellWithIdx(i);
                    break;
                }
            }
        }
        private void InitializeChannelList()
        {
            channelList.TableView.ClearSelection();
            channelList.Data.Clear();
            _asioChannelList.Clear();

            Plugin.Log?.Debug($"ASIO OutputChannel {Plugin.Instance._controller.asioOut?.DriverOutputChannelCount}");
            for (int i=0;i< Plugin.Instance._controller.asioOut?.DriverOutputChannelCount; i += 2)
            {
                string name = $"Channel {i+1}/{i+2}";
                var data1 = new CustomListTableData.CustomCellInfo(name);
                channelList.Data.Add(data1);
                _asioChannelList.Add(name);
            }
            channelList.TableView.ReloadData();
            if(Plugin.Instance._controller.asioOut?.DriverOutputChannelCount>=2)
                channelList.TableView.SelectCellWithIdx((int)(PluginConfig.Instance.OutputChannel/ 2));
        }
    }
}
