using Online_Cinema_Core.Settings.Interfaces;
using Online_Cinema_Core.Settings.Managers.Base;
using Online_Cinema_Core.Settings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Settings.Managers
{
    public class AzureSettingsManager : BaseSettingsManager<AzureSettingsModel>, IAzureSettingsManager
    {
        public AzureSettingsManager() : base("AzureSettings.json") { }
    }
}
