using System;
using HarmonyLib;
using UnityEngine;

namespace AudioSplitter.HarmonyPatches
{
    [HarmonyPatch(typeof(MainCamera), "Awake")]
    internal class AudioListenerPatchInMainCamera
    {
        private static void Prefix(MainCamera __instance)
        {
            if(__instance.gameObject.GetComponent<AudioListener>() != null && __instance.gameObject.GetComponent<AudioConnector>() == null)
            {
                var obj= __instance.gameObject.AddComponent<AudioConnector>();
                obj._objectPath = Utils.GetFullPath(__instance.transform);
                Plugin.Log?.Notice($"Add AudioConnector in {Utils.GetFullPath(__instance.transform)}");
            }
        }
    }

    [HarmonyPatch(typeof(AudioListenerController), "Awake")]
    internal class AudioListenerPatchInAudioListenerController
    {
        private static void Prefix(AudioListenerController __instance)
        {
            if (__instance.gameObject.GetComponent<AudioListener>() != null && __instance.gameObject.GetComponent<AudioConnector>() == null)
            {
                var obj = __instance.gameObject.AddComponent<AudioConnector>();
                obj._objectPath = Utils.GetFullPath(__instance.transform);
                Plugin.Log?.Notice($"Add AudioConnector in {Utils.GetFullPath(__instance.transform)}");
            }
        }
    }
}
