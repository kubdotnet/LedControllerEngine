using LedControllerEngine.Assets;
using LedControllerEngine.Assets.Effects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace LedControllerEngine.ViewModel
{
    public class DesignMainViewModel : IMainViewModel
    {
        public string Title
        {
            get => "Main Window Title";
            set { }
        }

        public ObservableCollection<SelectablePort> Ports
        {
            get => new ObservableCollection<SelectablePort>(
                Enumerable.Range(1, 3).Select(i => new SelectablePort()
                {
                    Address = $"COM{i}",
                    IsSelected = (i % 2) == 0
                })
            );
            set { }
        }

        public IEnumerable<IEffect> FanEffects
        {
            get => new List<IEffect>() {
                new HueShift(),
                new SingleSpinner(),
                new Rainbow()
            };
            set { }
        }

        public IEffect SelectedFanEffect { get; set; }

        public ObservableCollection<LedDevice> Fans
        {
            get => new ObservableCollection<LedDevice>(
                Enumerable.Range(1, 6).Select(i => new LedDevice() {
                    Index = i,
                    Description = $"Fan {i}",
                    IsSelected = (i % 2) == 0
                })
            );
            set { }
        }

        public ObservableCollection<LedDevice> Stripes
        {
            get => new ObservableCollection<LedDevice>(
                Enumerable.Range(7, 4).Select(i => new LedDevice()
                {
                    Index = i,
                    Description = $"Stripe {i}",
                    IsSelected = (i % 2) == 0
                })
            );
            set { }
        }

        public ICommand FanToggleCommand { get; set; }
        public ICommand StripeToggleCommand { get; set; }
        public TransferMode EffectMode
        {
            get => TransferMode.Live;
            set { }
        }

        public UserControl FanConfigurationUI
        {
            get => new HueShiftSettings();
            set { }
        }
        public int SelectedFansCount { get => 4; }

        public bool IsSettingsOpen
        {
            get => true;
            set { }
        }

        public ICommand SettingsToggleCommand { get; set; }
        public Settings ApplicationSettings { get; set; }
        public ICommand TransferStageToLiveCommand { get; set; }
        public ICommand LoadMemoryToLiveCommand { get; set; }
        public ICommand SaveLiveToMemoryCommand { get; set; }

        public IEnumerable<IEffect> StripeEffects
        {
            get => new List<IEffect>() {
                new HueShift(),
                new Bpm()
            };
            set { }
        }

        public IEffect SelectedStripeEffect { get; set; }
        public UserControl StripeConfigurationUI
        {
            get => new BpmSettings();
            set { }
        }
        public int SelectedStripesCount { get => 2; }
        public ICommand PortToggleCommand { get; set; }
    }
}