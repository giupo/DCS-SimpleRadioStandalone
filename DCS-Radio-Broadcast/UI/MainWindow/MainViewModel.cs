
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Ciribob.DCS.SimpleRadio.Standalone.Common;
using NAudio.Wave;
using NLog;
using LogManager = NLog.LogManager;

namespace Ciribob.DCS.SimpleRadio.Standalone.Broadcaster.UI.MainWindow
{
    public sealed class MainViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
           _eventAggregator = eventAggregator;
        
            _eventAggregator.Subscribe(this);

            DisplayName = "DCS-SRS Broadcaster - " + UpdaterChecker.VERSION;

           Logger.Info("DCS-SRS Broadcaster Running - " + UpdaterChecker.VERSION);

          //  SelectedInput = "bar";
            InitAudioInput();

            InitSideSelector();
        }

        private void InitSideSelector()
        {
            _sideSelectorList.Add(new SideSelectorModel()
            {
                Name = "Neutral",Side = 0
            });
            _sideSelectorList.Add(new SideSelectorModel()
            {
                Name = "Red",
                Side = 1
            });
            _sideSelectorList.Add(new SideSelectorModel()
            {
                Name = "Blue",
                Side = 2
            });
            SelectedSide = _sideSelectorList[0];
        }

        private void InitAudioInput()
        {
//            Logger.Info("Audio Input - Saved ID " +
//                        _settings.GetClientSetting(SettingsKeys.AudioInputDeviceId).StringValue);

            for (var i = 0; i < WaveIn.DeviceCount; i++)
            {
                var item = WaveIn.GetCapabilities(i);

                _inputSelectorList.Add(new AudioDeviceModel()
                {
                    AudioDevice = item,
                    DeviceIndex = i
                });
            
                Logger.Info("Audio Input - " + item.ProductName + " " + item.ProductGuid.ToString() + " - Name GUID" +
                            item.NameGuid + " - CHN:" + item.Channels);

//                if (item.ProductName.Trim().StartsWith(_settings.GetClientSetting(SettingsKeys.AudioInputDeviceId).StringValue.Trim()))
//                {
//                    Mic.SelectedIndex = i;
//                    Logger.Info("Audio Input - Found Saved ");
//                }
            }

            // No microphone is available - users can still connect/listen, but audio input controls are disabled and sending is prevented
            if (WaveIn.DeviceCount == 0 )
            {
                Logger.Info("Audio Input - No audio input devices available, disabling mic preview");
            }
            else
            {
                Logger.Info("Audio Input - " + WaveIn.DeviceCount + " audio input devices available, configuring as usual");
             
            }

            if (_inputSelectorList.Count > 0)
            {
                SelectedInput = _inputSelectorList[0];
            }
        }

        public void ConnectDisconnect()
        {
            //connect / disconnect
        }

        private int _port = 5002;
        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                this.NotifyOfPropertyChange(() => this._port);
            }
        }


        private ObservableCollection<SideSelectorModel> _sideSelectorList = new ObservableCollection<SideSelectorModel>();

        public ObservableCollection<SideSelectorModel> SideSelector
        {
            get => _sideSelectorList;
            set
            {
                this._sideSelectorList = value;
                this.NotifyOfPropertyChange(() => this._sideSelectorList);
            }
        }

        private SideSelectorModel _selectedSide;
        public SideSelectorModel SelectedSide
        {
            get => this._selectedSide;

            set
            {
                this._selectedSide = value;
                this.NotifyOfPropertyChange(() => this._selectedSide);
            }
        }

        private ObservableCollection<AudioDeviceModel> _inputSelectorList = new ObservableCollection<AudioDeviceModel>();
        public ObservableCollection<AudioDeviceModel> InputSelector
        {
            get => _inputSelectorList;
            set
            {
                this._inputSelectorList = value;
                this.NotifyOfPropertyChange(() => this._inputSelectorList);
            }
        }
        private AudioDeviceModel _selectedInput;
        public AudioDeviceModel SelectedInput
        {
            get => this._selectedInput;

            set
            {
                this._selectedInput = value;
                this.NotifyOfPropertyChange(() => this._selectedInput);
            }
        }
      
  
    }
}