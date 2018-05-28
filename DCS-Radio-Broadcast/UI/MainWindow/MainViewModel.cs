
using Caliburn.Micro;
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

            DisplayName = "DCS-SRS Broadcaster - ";// + UpdaterChecker.VERSION;

       //     Logger.Info("DCS-SRS Broadcaster Running - " + UpdaterChecker.VERSION);
        }

    
  
    }
}