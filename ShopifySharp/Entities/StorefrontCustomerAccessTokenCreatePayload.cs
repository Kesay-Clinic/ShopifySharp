using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShopifySharp;

public class StorefrontCustomerAccessTokenCreatePayload
{
    /// <summary>
    /// The created customer access token, if login succeeds.
    /// </summary>
    [JsonProperty("customerAccessToken")]
    public StorefrontCustomerAccessToken CustomerAccessToken { get; set; }

    /// <summary>
    /// A list of errors that prevented the customer access token from being created.
    /// </summary>
    [JsonProperty("customerUserErrors")]
    public IEnumerable<StorefrontCustomerUserError> CustomerUserErrors { get; set; }
}
