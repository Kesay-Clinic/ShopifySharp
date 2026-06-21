using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ShopifySharp.Credentials;
using ShopifySharp.Infrastructure;
using ShopifySharp.Utilities;

namespace ShopifySharp;

/// <summary>
/// A service for working with Shopify Storefront API customers.
/// </summary>
public class StorefrontCustomerService : StorefrontService, IStorefrontCustomerService
{
    private const string CustomerAccessTokenCreateMutation =
        """
        mutation customerAccessTokenCreate($input: CustomerAccessTokenCreateInput!) {
          customerAccessTokenCreate(input: $input) {
            customerUserErrors { code field message }
            customerAccessToken {
              accessToken
              expiresAt
            }
          }
        }
        """;

    /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
    /// <param name="storefrontAccessToken">A Storefront API access token for the shop.</param>
    public StorefrontCustomerService(string myShopifyUrl, string storefrontAccessToken) : base(myShopifyUrl, storefrontAccessToken) { }

#nullable enable
    internal StorefrontCustomerService(ShopifyApiCredentials shopifyApiCredentials, IShopifyDomainUtility? shopifyDomainUtility = null)
        : base(shopifyApiCredentials, shopifyDomainUtility) { }

    internal StorefrontCustomerService(ShopifyApiCredentials shopifyApiCredentials, IServiceProvider serviceProvider)
        : base(shopifyApiCredentials, serviceProvider) { }
#nullable restore

    internal StorefrontCustomerService(string shopDomain, string storefrontAccessToken, IShopifyDomainUtility shopifyDomainUtility)
        : base(shopDomain, storefrontAccessToken, shopifyDomainUtility) { }

    /// <inheritdoc />
    public virtual async Task<StorefrontCustomerAccessTokenCreatePayload> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var req = BuildRequestUri("graphql.json");
        var content = new JsonContent(new
        {
            query = CustomerAccessTokenCreateMutation,
            variables = new
            {
                input = new
                {
                    email,
                    password
                }
            }
        });

        var response = await ExecuteRequestAsync<StorefrontCustomerAccessTokenCreatePayload>(
            req,
            HttpMethod.Post,
            cancellationToken,
            content,
            "data.customerAccessTokenCreate");

        return response.Result;
    }
}
