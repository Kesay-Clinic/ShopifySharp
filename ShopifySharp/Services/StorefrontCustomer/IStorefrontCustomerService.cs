using System.Threading;
using System.Threading.Tasks;

namespace ShopifySharp;

public interface IStorefrontCustomerService : IShopifyService
{
    /// <summary>
    /// Creates a customer access token from a storefront customer email and password.
    /// </summary>
    Task<StorefrontCustomerAccessTokenCreatePayload> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
