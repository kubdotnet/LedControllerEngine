using LedControllerEngine.Assets;
using LedControllerEngine.Assets.Effects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace LedControllerEngine.ViewModel
{
    public interface IMainViewModel
    {
        string Title { get; }
        TransferMode EffectMode { get; set; }
        bool IsSettingsOpen { get; set; }
        ICommand SettingsToggleCommand { get; set; }
        Settings ApplicationSettings { get; set; }
        ICommand TransferStageToLiveCommand { get; set; }
        ICommand LoadMemoryToLiveCommand { get; set; }
        ICommand SaveLiveToMemoryCommand { get; set; }


        #region fans

        IEnumerable<IEffect> FanEffects { get; }
        IEffect SelectedFanEffect { get; set; }
        ObservableCollection<LedDevice> Fans { get; set; }
        UserControl FanConfigurationUI { get; set; }
        ICommand FanToggleCommand { get; set; }

        #endregion

        #region stripes

        IEnumerable<IEffect> StripeEffects { get; }
        IEffect SelectedStripeEffect { get; set; }
        ObservableCollection<LedDevice> Stripes { get; set; }
        ICommand StripeToggleCommand { get; set; }
        UserControl StripeConfigurationUI { get; set; }

        #endregion
    }
}
