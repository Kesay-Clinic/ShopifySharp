#nullable enable
using System;
using ShopifySharp.Credentials;
using ShopifySharp.Infrastructure;
using ShopifySharp.Utilities;

namespace ShopifySharp;

/// <summary>
/// Base service for Shopify's Storefront API.
/// </summary>
public abstract class StorefrontService : ShopifyService
{
    protected override string AccessTokenHeaderName => REQUEST_HEADER_STOREFRONT_ACCESS_TOKEN;

    protected StorefrontService(string shopDomain, string storefrontAccessToken) : base(shopDomain, storefrontAccessToken) { }

    protected StorefrontService(ShopifyApiCredentials shopifyApiCredentials, IShopifyDomainUtility? domainUtility)
        : base(shopifyApiCredentials, domainUtility) { }

    internal StorefrontService(ShopifyApiCredentials shopifyApiCredentials, IServiceProvider serviceProvider)
        : base(shopifyApiCredentials, serviceProvider) { }

    internal StorefrontService(string shopDomain, string storefrontAccessToken, IShopifyDomainUtility shopifyDomainUtility)
        : base(shopDomain, storefrontAccessToken, shopifyDomainUtility) { }

    /// <summary>
    /// Builds a <see cref="RequestUri"/> by merging the shop domain with Shopify's Storefront API path.
    /// </summary>
    protected override RequestUri BuildRequestUri(string path)
    {
        var ub = new UriBuilder(_ShopUri)
        {
            Scheme = Uri.UriSchemeHttps,
            Port = 443,
            Path = $"api/{APIVersion}/{path}"
        };

        return new RequestUri(ub.Uri);
    }
}
