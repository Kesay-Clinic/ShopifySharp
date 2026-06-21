using System;
using Newtonsoft.Json;

namespace ShopifySharp;

public class StorefrontCustomerAccessToken
{
    /// <summary>
    /// The token used to identify the customer.
    /// </summary>
    [JsonProperty("accessToken")]
    public string AccessToken { get; set; }

    /// <summary>
    /// The date and time when the customer access token expires.
    /// </summary>
    [JsonProperty("expiresAt")]
    public DateTimeOffset? ExpiresAt { get; set; }
}
