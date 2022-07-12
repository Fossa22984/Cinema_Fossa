using Online_Cinema_Core.Settings.Interfaces;
using Online_Cinema_Core.Settings.Managers.Base;
using Online_Cinema_Core.Settings.Models;

namespace Online_Cinema_Core.Settings.Managers
{
    public class AzureSettingsManager : BaseSettingsManager<AzureSettingsModel>, IAzureSettingsManager
    {
        public AzureSettingsManager() : base("AzureSettings.json") { }
    }
}
