﻿using GalaSoft.MvvmLight;
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

        private IEnumerable<IEffect> _effects;
        public IEnumerable<IEffect> Effects
        {
            get
            {
                return _effects;
            }
            internal set
            {
                _effects = value;
                RaisePropertyChanged(() => Effects);
            }
        }

        private IEffect _selectedEffect;
        public IEffect SelectedEffect
        {
            get
            {
                return _selectedEffect;
            }
            set
            {
                _selectedEffect = value;
                RaisePropertyChanged(() => SelectedEffect);

                LoadEffectSettingsEditor(value);
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

        private UserControl _configurationUI;
        public UserControl ConfigurationUI
        {
            get
            {
                return _configurationUI;
            }
            set
            {
                _configurationUI = value;
                RaisePropertyChanged(() => ConfigurationUI);
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

        #endregion

        #region commands

        public ICommand FanToggleCommand { get; set; }
        public ICommand StripeToggleCommand { get; set; }
        public ICommand SendEffectCommand { get; set; }
        public ICommand SettingsToggleCommand { get; set; }
        public ICommand TransferStageToLiveCommand { get; set; }

        #endregion

        public MainViewModel()
        {
            ApplicationSettings = GetApplicationSettings();
            ApplicationSettings.PropertyChanged += ApplicationSettings_PropertyChanged;

            Title = GetAssemblyTitle();
            Effects = GetAvailableEffects();
            Fans = GetAvailableFans();
            Stripes = GetAvailableStripes();
            EffectMode = TransferMode.Stage;

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
                () => IsSettingsOpen = !IsSettingsOpen
            );

            TransferStageToLiveCommand = new RelayCommand(
                () => {
                    EnsureInteropInitialized();
                    _interop.SendCommand("<");
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

            if (!System.IO.File.Exists(_settingsPath))
            {
                return new Settings()
                {
                    PortList = portList,
                    Rate = 115200,
                    RateList = rateList,
                    FansCount = 6
                };
            }

            // load settings from file
            var _settings = Settings.LoadFromFile(_settingsPath);
            _settings.PortList = portList;
            _settings.RateList = rateList;
            return _settings;
        }

        private Settings GetDefaultSettings()
        {
            return new Settings()
            {
                PortList = System.IO.Ports.SerialPort.GetPortNames().ToList(),
                RateList = new List<int> { 4800, 9600, 19200, 38400, 57600, 115200, 230400 }
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
            return Fans.Where(f => f.IsSelected).Count() > 0
                || Stripes.Where(s => s.IsSelected).Count() > 0
                && SelectedEffect != null;
        }

        /// <summary>
        /// Sends the effect settings.
        /// </summary>
        private void SendEffectSettings()
        {
            EnsureInteropInitialized();

            var fans = Fans.Where(f => f.IsSelected).Select(f => f.Index);
            var stripes = Stripes.Where(s => s.IsSelected).Select(s => s.Index);
            _interop.SendSettings(SelectedEffect.GetSettingsValues(), fans.Concat(stripes), EffectMode);

            // save current effect settings
            var existingEffect = ApplicationSettings.Effects.FirstOrDefault(e => SelectedEffect.Id == e.Id);
            if (existingEffect != null)
            {
                ApplicationSettings.Effects.Remove(existingEffect);
            }
            ApplicationSettings.Effects.Add(SelectedEffect);
            ApplicationSettings.Save(_settingsPath);
        }

        /// <summary>
        /// Loads the effect settings editor.
        /// </summary>
        /// <param name="effect">The effect.</param>
        private void LoadEffectSettingsEditor(IEffect effect)
        {
            var control = Activator.CreateInstance(effect.SettingsControl) as UserControl;
            var existingEffect = ApplicationSettings.Effects.FirstOrDefault(e => SelectedEffect.Id == e.Id);
            if (existingEffect != null)
            {
                effect.SettingsModel = existingEffect.SettingsModel;
            }

            control.DataContext = effect.SettingsModel;
            ConfigurationUI = control;
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
                _interop = new Arduino.LedInterop(ApplicationSettings.Port, ApplicationSettings.Rate);
            }
        }
    }
}
