using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShopifySharp;

public class StorefrontCustomerUserError
{
    /// <summary>
    /// The error code.
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    /// <summary>
    /// The path to the input field that caused the error.
    /// </summary>
    [JsonProperty("field")]
    public IEnumerable<string> Field { get; set; }

    /// <summary>
    /// The error message.
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; }
}
