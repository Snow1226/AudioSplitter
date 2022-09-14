using BeatSaberMarkupLanguage;
using HMUI;
using AudioSplitter.Configuration;

namespace AudioSplitter.UI
{
    internal class ModMainFlowCoordinator : FlowCoordinator
    {
        private const string _titleString = "AudioSplitter";
        private AudioSplitterUI _audioSplitterUI;
        public bool IsBusy { get; set; }

        private void Awake()
        {
            this._audioSplitterUI = BeatSaberUI.CreateViewController<AudioSplitterUI>();
            this._audioSplitterUI.mainFlowCoordinator = this;
        }
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetTitle(_titleString);
            this.showBackButton = true;

            var viewToDisplay = _audioSplitterUI;

            this.IsBusy = true;
            ProvideInitialViewControllers(viewToDisplay);
            this.IsBusy = false;
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            if (this.IsBusy) return;
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}
