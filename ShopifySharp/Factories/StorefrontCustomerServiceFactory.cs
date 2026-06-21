#nullable enable
using System;
using JetBrains.Annotations;
using ShopifySharp.Credentials;
using ShopifySharp.Infrastructure;
using ShopifySharp.Utilities;

namespace ShopifySharp.Factories;

public interface IStorefrontCustomerServiceFactory : IServiceFactory<IStorefrontCustomerService>;

[PublicAPI]
public class StorefrontCustomerServiceFactory : IStorefrontCustomerServiceFactory
{
    private readonly IShopifyDomainUtility? _shopifyDomainUtility;
    private readonly IRequestExecutionPolicy? _requestExecutionPolicy;
    private readonly IServiceProvider? _serviceProvider;

    public StorefrontCustomerServiceFactory(IRequestExecutionPolicy? requestExecutionPolicy, IShopifyDomainUtility? shopifyDomainUtility = null)
    {
        _shopifyDomainUtility = shopifyDomainUtility;
        _requestExecutionPolicy = requestExecutionPolicy;
    }

    public StorefrontCustomerServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _requestExecutionPolicy = InternalServiceResolver.GetService<IRequestExecutionPolicy>(serviceProvider);
    }

    /// <inheritDoc />
    public virtual IStorefrontCustomerService Create(string shopDomain, string accessToken) =>
        Create(new ShopifyApiCredentials(shopDomain, accessToken));

    /// <inheritDoc />
    public virtual IStorefrontCustomerService Create(ShopifyApiCredentials credentials)
    {
        IStorefrontCustomerService service = _serviceProvider is not null
            ? new StorefrontCustomerService(credentials, _serviceProvider)
            : new StorefrontCustomerService(credentials, _shopifyDomainUtility);

        if (_requestExecutionPolicy is not null)
            service.SetExecutionPolicy(_requestExecutionPolicy);

        return service;
    }
}
