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
        IEnumerable<IEffect> Effects { get; }
        IEffect SelectedEffect { get; set; }
        ObservableCollection<LedDevice> Fans { get; set; }
        ObservableCollection<LedDevice> Stripes { get; set; }
        ICommand FanToggleCommand { get; set; }
        ICommand StripeToggleCommand { get; set; }
        TransferMode EffectMode { get; set; }
        UserControl ConfigurationUI { get; set; }
        bool IsSettingsOpen { get; set; }
        ICommand SettingsToggleCommand { get; set; }
        Settings ApplicationSettings { get; set; }
        ICommand TransferStageToLiveCommand { get; set; }
        ICommand LoadMemoryToLiveCommand { get; set; }
        ICommand SaveLiveToMemoryCommand { get; set; }
    }
}
