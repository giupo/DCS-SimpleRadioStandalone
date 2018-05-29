using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Ciribob.DCS.SimpleRadio.Standalone.Broadcaster.UI.MainWindow
{
    public class AudioDeviceModel
    {
        public WaveInCapabilities AudioDevice { get; set; }
        public int DeviceIndex { get; set; }

        public string Name
        {
            get { return AudioDevice.ProductName; }
        }

        public override string ToString()
        {
            return AudioDevice.ProductName;
        }
    }
}
