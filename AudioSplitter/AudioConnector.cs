using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using NAudio.Wave;
using AudioSplitter.Configuration;

namespace AudioSplitter
{
    internal class AudioConnector : MonoBehaviour
    {
        internal string _objectPath = String.Empty;

        private void Start()
        {
            if (_objectPath == String.Empty)
            {
                Destroy(this.gameObject.GetComponent<AudioConnector>());
                return;
            }
            Plugin.Instance._controller.asioBuffer?.ClearBuffer();
            Plugin.Log?.Notice($"AudioConnecotr Enable {_objectPath}");
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            try
            {
                Int16[] intData = new Int16[data.Length];
                var byteData = new byte[data.Length * 2];
                int rescaleFactor = 32767;
                for (int i = 0; i < data.Length; i++)
                {
                    intData[i] = (short)(data[i] * rescaleFactor);
                    var byteArr = new byte[2];
                    byteArr = BitConverter.GetBytes(intData[i]);
                    byteArr.CopyTo(byteData, i * 2);
                }
                Plugin.Instance._controller.asioBuffer?.AddSamples(byteData, 0, byteData.Length);
                if (!PluginConfig.Instance.DefaultDeviceOutput)
                {
                    for (int i = 0; i < data.Length; i++)
                        data[i] = data[i] * 0;
                }
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error($"Error {ex.Message}");
            }
        }
    }
}
