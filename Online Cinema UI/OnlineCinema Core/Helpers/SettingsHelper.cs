using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema_Core.Helpers
{
    public class SettingsHelper
    {
        private static readonly SettingsHelper _instance = new SettingsHelper();
        public static SettingsHelper Current => _instance;

        public long MaxRequestLenghts { get; private set; } = 4294967295; // 4Gb
    }
}
