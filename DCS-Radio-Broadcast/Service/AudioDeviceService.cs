using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ciribob.DCS.SimpleRadio.Standalone.Broadcaster.UI.MainWindow;
using NAudio.Wave;
using NLog;

namespace Ciribob.DCS.SimpleRadio.Standalone.Broadcaster
{
    public class AudioDeviceService
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Collection<AudioDeviceModel> AudioInputs { get; private set; }


        public AudioDeviceService()
        {
            AudioInputs = new Collection<AudioDeviceModel>();
           
            InitAudioInput();
         
        }

        public void InitAudioInput()
        {
            //            Logger.Info("Audio Input - Saved ID " +
            //                        _settings.GetClientSetting(SettingsKeys.AudioInputDeviceId).StringValue);

            for (var i = 0; i < WaveIn.DeviceCount; i++)
            {
                var item = WaveIn.GetCapabilities(i);

                AudioInputs.Add(new AudioDeviceModel()
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
            if (WaveIn.DeviceCount == 0)
            {
                Logger.Info("Audio Input - No audio input devices available, disabling mic preview");
            }
            else
            {
                Logger.Info("Audio Input - " + WaveIn.DeviceCount + " audio input devices available, configuring as usual");

            }

        }
    }
}
