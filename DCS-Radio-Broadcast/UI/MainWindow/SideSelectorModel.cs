using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciribob.DCS.SimpleRadio.Standalone.Broadcaster.UI.MainWindow
{
    public class SideSelectorModel
    {
        public string Name { get; set; }
        public int Side { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
