namespace Platform.Shared.Models.Externals
{
    public class AzureFunctionRequest
    {
        [Newtonsoft.Json.JsonProperty("resourcegroup")]
        public string Resourcegroup { get; set; }

        [Newtonsoft.Json.JsonProperty("action")]
        public string Action { get; set; }

        [Newtonsoft.Json.JsonProperty("subscriptionid")]
        public string SubscriptionId { get; set; }

        [Newtonsoft.Json.JsonProperty("tenantid")]
        public string Tenantid { get; set; }

      
    }
}
