using Newtonsoft.Json;
using System;

namespace Online_Cinema_Core.Settings.Models
{
    public class AzureSettingsModel
    {
        [JsonProperty("AadEndpoint")] private string _aadEndpoint;
        [JsonIgnore] public Uri AadEndpoint { get { return new Uri(_aadEndpoint); } set { } }
        [JsonProperty("ArmEndpoint")] public string _armEndpoint { get; set; }
        [JsonIgnore] public Uri ArmEndpoint { get { return new Uri(_armEndpoint); } set { } }
        [JsonProperty("ArmAadAudience")] private string _armAadAudience { get; set; }
        [JsonIgnore] public Uri ArmAadAudience { get { return new Uri(_armAadAudience); } set { } }

        [JsonProperty("AadClientId")] public string AadClientId { get; set; }
        [JsonProperty("AadSecret")] public string AadSecret { get; set; }
        [JsonProperty("AadTenantId")] public string AadTenantId { get; set; }
        [JsonProperty("AccountName")] public string AccountName { get; set; }
        [JsonProperty("ResourceGroup")] public string ResourceGroup { get; set; }
        [JsonProperty("SubscriptionId")] public string SubscriptionId { get; set; }
    }
}
