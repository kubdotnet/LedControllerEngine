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

        public IEnumerable<IEffect> Effects
        {
            get => new List<IEffect>() {
                new HueShift(),
                new SingleSpinner(),
                new Rainbow()
            };
            set { }
        }

        public IEffect SelectedEffect { get; set; }

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

        public UserControl ConfigurationUI
        {
            get => new HueShiftSettings();
            set { }
        }

        public bool IsSettingsOpen
        {
            get => true;
            set { }
        }

        public ICommand SettingsToggleCommand { get; set; }
        public Settings ApplicationSettings { get; set; }
    }
}