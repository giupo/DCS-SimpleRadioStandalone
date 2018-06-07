using System;
using Caliburn.Micro;
using NLog;

namespace Ciribob.DCS.SimpleRadio.Standalone.Broadcaster.UI.MainWindow.FrequencyBlock
{
    public class FrequencyBlockViewModel :Screen
    {
        private readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private bool _modulation;

        public Boolean Modulation
        {
            get => _modulation;
            set
            {
                _modulation = value;
                if (_modulation)
                {
                    ModulationText = "FM";
                }
                else
                {
                    ModulationText = "AM";
                }
            }
        }

        public string ModulationText { get; set; }
        public double Frequency { get; set; }

        public FrequencyBlockViewModel()
        {
            Logger.Info("TADA");
            ModulationText = "AM";
            Frequency = 251.0;

            //todo use events 
        }
      

    }
}
