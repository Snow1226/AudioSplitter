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
        private List<string> _asioDeviceList = new List<string>();
        public ModMainFlowCoordinator mainFlowCoordinator { get; set; }
        public void SetMainFlowCoordinator(ModMainFlowCoordinator mainFlowCoordinator)
        {
            this.mainFlowCoordinator = mainFlowCoordinator;
        }
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            InitializeList();
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
        }
        [UIValue("ChannelOffset")]
        private int channelOffset = PluginConfig.Instance.OutputChannel;
        [UIAction("OnChannelOffsetChange")]
        private void OnChannelOffsetChange(int value)
        {
            channelOffset = value;
            PluginConfig.Instance.OutputChannel = channelOffset;
            Plugin.Instance._controller.RestartASIO();
        }

        [UIValue("DefaultOutput")]
        private bool defaultOutput = PluginConfig.Instance.DefaultDeviceOutput;
        [UIAction("OnDefaultOutputChange")]
        private void OnDefaultOutputChange(bool value)
        {
            PluginConfig.Instance.DefaultDeviceOutput = defaultOutput = value;
        }
        [UIAction("OnBufferReset")]
        private void OnBufferReset()
        {
            Plugin.Instance._controller.asioBuffer.ClearBuffer();
        }
        private void InitializeList()
        {
            deviceList.tableView.ClearSelection();
            deviceList.data.Clear();

            _asioDeviceList.Clear();

            foreach (string name in AsioOut.GetDriverNames())
            {
                var data1 = new CustomListTableData.CustomCellInfo(name);
                deviceList.data.Add(data1);
                _asioDeviceList.Add(name);
            }
            deviceList.tableView.ReloadData();
            for(int i = 0; i < _asioDeviceList.Count; i++)
            {
                if(_asioDeviceList[i] == PluginConfig.Instance.AudioDevice)
                {
                    deviceList.tableView.SelectCellWithIdx(i);
                    break;
                }
            }
        }
    }
}
