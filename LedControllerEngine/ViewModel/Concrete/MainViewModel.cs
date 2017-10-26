using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LedControllerEngine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using LedControllerEngine.Assets.Effects;

namespace LedControllerEngine.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        #region fields

        ILogging _logging;
        private Arduino.LedInterop _interop;
        private string _settingsPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"LedControllerEngine\settings.json");

        #endregion

        #region properties

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            internal set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private TransferMode _effectMode;
        public TransferMode EffectMode
        {
            get
            {
                return _effectMode;
            }
            set
            {
                _effectMode = value;
                RaisePropertyChanged(() => EffectMode);
            }
        }

        private bool _isSettingsOpen;
        public bool IsSettingsOpen
        {
            get
            {
                return _isSettingsOpen;
            }
            set
            {
                _isSettingsOpen = value;
                RaisePropertyChanged(() => IsSettingsOpen);

                if (value == false)
                {
                    SaveSettings();
                }
            }
        }

        private Settings _applicationSettings;
        public Settings ApplicationSettings
        {
            get
            {
                return _applicationSettings;
            }
            set
            {
                _applicationSettings = value;
                RaisePropertyChanged(() => ApplicationSettings);
            }
        }

        #region fans

        private IEnumerable<IEffect> _fanEffects;
        public IEnumerable<IEffect> FanEffects
        {
            get
            {
                return _fanEffects;
            }
            internal set
            {
                _fanEffects = value;
                RaisePropertyChanged(() => FanEffects);
            }
        }
        
        private IEffect _selectedFanEffect;
        public IEffect SelectedFanEffect
        {
            get
            {
                return _selectedFanEffect;
            }
            set
            {
                _selectedFanEffect = value;
                RaisePropertyChanged(() => SelectedFanEffect);

                LoadFanEffectSettingsEditor(value);
            }
        }

        private ObservableCollection<LedDevice> _fans;
        public ObservableCollection<LedDevice> Fans
        {
            get
            {
                return _fans;
            }
            set
            {
                _fans = value;
                RaisePropertyChanged(() => Fans);
            }
        }

        private UserControl _fanConfigurationUI;
        public UserControl FanConfigurationUI
        {
            get
            {
                return _fanConfigurationUI;
            }
            set
            {
                _fanConfigurationUI = value;
                RaisePropertyChanged(() => FanConfigurationUI);
            }
        }

        #endregion

        #region stripes

        private IEnumerable<IEffect> _strpieEffects;
        public IEnumerable<IEffect> StripeEffects
        {
            get
            {
                return _strpieEffects;
            }
            internal set
            {
                _strpieEffects = value;
                RaisePropertyChanged(() => StripeEffects);
            }
        }

        private IEffect _selectedStripeEffect;
        public IEffect SelectedStripeEffect
        {
            get
            {
                return _selectedStripeEffect;
            }
            set
            {
                _selectedStripeEffect = value;
                RaisePropertyChanged(() => SelectedStripeEffect);

                LoadStripeEffectSettingsEditor(value);
            }
        }

        private ObservableCollection<LedDevice> _stripes;
        public ObservableCollection<LedDevice> Stripes
        {
            get
            {
                return _stripes;
            }
            set
            {
                _stripes = value;
                RaisePropertyChanged(() => Stripes);
            }
        }

        private UserControl _stripeConfigurationUI;
        public UserControl StripeConfigurationUI
        {
            get
            {
                return _stripeConfigurationUI;
            }
            set
            {
                _stripeConfigurationUI = value;
                RaisePropertyChanged(() => StripeConfigurationUI);
            }
        }

        #endregion

        #endregion

        #region commands

        public ICommand FanToggleCommand { get; set; }
        public ICommand StripeToggleCommand { get; set; }
        public ICommand SendEffectCommand { get; set; }
        public ICommand SettingsToggleCommand { get; set; }
        public ICommand TransferStageToLiveCommand { get; set; }
        public ICommand LoadMemoryToLiveCommand { get; set; }
        public ICommand SaveLiveToMemoryCommand { get; set; }

        #endregion

        public MainViewModel(ILogging logging)
        {
            _logging = logging;

            ApplicationSettings = GetApplicationSettings();
            ApplicationSettings.PropertyChanged += ApplicationSettings_PropertyChanged;

            Title = GetAssemblyTitle();
            Fans = GetAvailableFans();
            Stripes = GetAvailableStripes();
            EffectMode = TransferMode.Live;

            var allEffects = GetAvailableEffects();
            FanEffects = allEffects.Where(e => (e.Compatiblity & DeviceType.Fan) == DeviceType.Fan);
            StripeEffects = allEffects.Where(e => (e.Compatiblity & DeviceType.Stripe) == DeviceType.Stripe);

            // event handlers
            FanToggleCommand = new RelayCommand<LedDevice>(
               fan => ToggleDeviceSelection(fan)
            );

            StripeToggleCommand = new RelayCommand<LedDevice>(
               stripe => ToggleDeviceSelection(stripe)
            );

            SendEffectCommand = new RelayCommand(
                () => SendEffectSettings(),
                () => { return CanSendSettings(); }
            );

            SettingsToggleCommand = new RelayCommand(
                () => {
                    _logging.Info("Settings " + (IsSettingsOpen ? "closing" : "opening"));
                    IsSettingsOpen = !IsSettingsOpen;
                }
            );

            TransferStageToLiveCommand = new RelayCommand(
                () => {
                    _logging.Info("Sending command <");
                    EnsureInteropInitialized();
                    _interop.SendCommand("<");
                }
            );

            SaveLiveToMemoryCommand = new RelayCommand(
                () => {
                    _logging.Info("Sending command !");
                    EnsureInteropInitialized();
                    _interop.SendCommand("!");
                }
            );

            LoadMemoryToLiveCommand = new RelayCommand(
                () => {
                    _logging.Info("Sending command $");
                    EnsureInteropInitialized();
                    _interop.SendCommand("$");
                }
            );
        }

        private void ApplicationSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Port":
                case "Rate":
                    if (_interop != null)
                    {
                        _interop.Dispose();
                        _interop = null;
                    }
                    break;
                case "FansCount":
                    Fans = GetAvailableFans();
                    break;
                case "StripesCount":
                    Stripes = GetAvailableStripes();
                    break;
            }
        }

        private Settings GetApplicationSettings()
        {
            var portList = System.IO.Ports.SerialPort.GetPortNames().ToList();
            var rateList = new List<int> { 4800, 9600, 19200, 38400, 57600, 115200, 230400 };

            if (System.IO.File.Exists(_settingsPath))
            {
                try
                {
                    _logging.Info("Loading settings from file");

                    // load settings from file
                    var _settings = Settings.LoadFromFile(_settingsPath);
                    _settings.PortList = portList;
                    _settings.RateList = rateList;
                    return _settings;
                }
                catch (Exception ex)
                {
                    _logging.Fatal(ex);
                }
            }

            _logging.Info("Sets default settings and open settings pane");

            IsSettingsOpen = true;
            return new Settings()
            {
                PortList = portList,
                Rate = 115200,
                RateList = rateList,
                FansCount = 6
            };
        }

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <returns></returns>
        private string GetAssemblyTitle()
        {
            var attribute = Application.ResourceAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), true);
            if (attribute != null && attribute.Length > 0 && attribute[0] is AssemblyProductAttribute)
            {
                return ((AssemblyProductAttribute)attribute[0]).Product;
            }

            return "";
        }

        /// <summary>
        /// Gets all classes that implements IEffect interface
        /// </summary>
        /// <returns>List of IEffect instances</returns>
        private IEnumerable<IEffect> GetAvailableEffects()
        {
            return
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from t in assembly.GetTypes()
                where t.GetInterfaces().Contains(typeof(IEffect)) && t.GetConstructor(Type.EmptyTypes) != null
                select Activator.CreateInstance(t) as IEffect)
                    .OrderBy(e => e.ModeNumber);
        }

        /// <summary>
        /// Gets the available fans.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<LedDevice> GetAvailableFans()
        {
            return new ObservableCollection<LedDevice>(
                Enumerable.Range(1, ApplicationSettings.FansCount).Select(i => new LedDevice()
                {
                    Index = i,
                    Description = $"Fan {i}",
                    IsSelected = false
                })
            );
        }

        private ObservableCollection<LedDevice> GetAvailableStripes()
        {
            return new ObservableCollection<LedDevice>(
                Enumerable.Range(7, ApplicationSettings.StripesCount).Select(i => new LedDevice()
                {
                    Index = i,
                    Description = $"Stripe {i}",
                    IsSelected = false
                })
            );
        }

        /// <summary>
        /// Toggles the device selection.
        /// </summary>
        /// <param name="device">The device.</param>
        private void ToggleDeviceSelection(LedDevice device)
        {
            device.IsSelected = !device.IsSelected;
        }

        /// <summary>
        /// Determines whether this instance [can send settings].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can send settings]; otherwise, <c>false</c>.
        /// </returns>
        private bool CanSendSettings()
        {
            return (Fans.Where(f => f.IsSelected).Count() > 0
                && SelectedFanEffect != null)
                || (Stripes.Where(s => s.IsSelected).Count() > 0
                && SelectedStripeEffect != null);
        }

        /// <summary>
        /// Sends the effect settings.
        /// </summary>
        private void SendEffectSettings()
        {
            EnsureInteropInitialized();

            var fans = Fans.Where(f => f.IsSelected).Select(f => f.Index);
            _interop.SendSettings(SelectedFanEffect.GetSettingsValues(), fans, EffectMode);

            var stripes = Stripes.Where(s => s.IsSelected).Select(s => s.Index);
            _interop.SendSettings(SelectedStripeEffect.GetSettingsValues(), stripes, EffectMode);

            // save current effect settings
            AddEffectToSettingsStore(SelectedFanEffect);
            AddEffectToSettingsStore(SelectedStripeEffect);
            ApplicationSettings.Save(_settingsPath);
        }

        /// <summary>
        /// Adds the effect to settings store.
        /// </summary>
        /// <param name="effect">The effect.</param>
        private void AddEffectToSettingsStore(IEffect effect)
        {
            if (effect == null)
            {
                return;
            }

            var existingEffect = ApplicationSettings.Effects.FirstOrDefault(e => effect.Id == e.Id);
            if (existingEffect != null)
            {
                ApplicationSettings.Effects.Remove(existingEffect);
            }
            ApplicationSettings.Effects.Add(effect);
        }

        /// <summary>
        /// Loads the Fan effect settings editor.
        /// </summary>
        /// <param name="effect">The effect.</param>
        private void LoadFanEffectSettingsEditor(IEffect effect)
        {
            var control = Activator.CreateInstance(effect.SettingsControl) as UserControl;
            var existingEffect = ApplicationSettings.Effects.FirstOrDefault(e => SelectedFanEffect.Id == e.Id);
            if (existingEffect != null)
            {
                effect.SettingsModel = existingEffect.SettingsModel;
            }

            control.DataContext = effect.SettingsModel;
            FanConfigurationUI = control;
        }

        /// <summary>
        /// Loads the Stripe effect settings editor.
        /// </summary>
        /// <param name="effect">The effect.</param>
        private void LoadStripeEffectSettingsEditor(IEffect effect)
        {
            var control = Activator.CreateInstance(effect.SettingsControl) as UserControl;
            var existingEffect = ApplicationSettings.Effects.FirstOrDefault(e => SelectedStripeEffect.Id == e.Id);
            if (existingEffect != null)
            {
                effect.SettingsModel = existingEffect.SettingsModel;
            }

            control.DataContext = effect.SettingsModel;
            StripeConfigurationUI = control;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            var fileName = System.IO.Path.GetFileName(_settingsPath);
            var directory = System.IO.Path.GetDirectoryName(_settingsPath.Remove(_settingsPath.Length - fileName.Length));

            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(_settingsPath);
            }

            ApplicationSettings.Save(_settingsPath);
        }

        /// <summary>
        /// Ensures the interop is initialized.
        /// </summary>
        private void EnsureInteropInitialized()
        {
            if (_interop == null)
            {
                _logging.Info("Interop not initialized, initializing now");
                _interop = new Arduino.LedInterop(ApplicationSettings.Port, ApplicationSettings.Rate);
            }
        }
    }
}
