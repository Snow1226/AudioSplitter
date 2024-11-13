using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;

namespace AudioSplitter.UI
{
    internal static class UIManager
    {
        internal static FlowCoordinator _mainFlow { get; private set; }
        public static void Init()
        {

            MenuButton menuButton = new MenuButton("Audio Splitter", "Mod to send audio to another device", ShowModFlowCoordinator, true);
            MenuButtons.Instance.RegisterButton(menuButton);
        }

        public static void ShowModFlowCoordinator()
        {
            if (Plugin.Instance._controller.mainFlowCoordinator == null)
                Plugin.Instance._controller.mainFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<ModMainFlowCoordinator>();
            if (Plugin.Instance._controller.mainFlowCoordinator.IsBusy) return;

            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(Plugin.Instance._controller.mainFlowCoordinator);
        }
    }
}
