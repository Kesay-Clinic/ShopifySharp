using System.Net.Http;
using System.Text.Json;
using ShopifySharp.Infrastructure;

namespace ShopifySharp.Tests.Services;

[Trait("Category", "StorefrontCustomer")]
public class StorefrontCustomerServiceTests
{
    [Fact]
    public async Task LoginAsync_SendsCustomerAccessTokenCreateMutationToStorefrontApi()
    {
        const string storefrontAccessToken = "storefront-token";
        const string email = "customer@example.com";
        const string password = "customer-password";
        var policy = new CapturingExecutionPolicy(
            """
            {
              "data": {
                "customerAccessTokenCreate": {
                  "customerUserErrors": [],
                  "customerAccessToken": {
                    "accessToken": "customer-access-token",
                    "expiresAt": "2026-07-18T12:34:56Z"
                  }
                }
              }
            }
            """);
        var service = new StorefrontCustomerService("example.myshopify.com", storefrontAccessToken);
        service.SetExecutionPolicy(policy);

        var result = await service.LoginAsync(email, password);

        policy.Method.Should().Be(HttpMethod.Post);
        policy.RequestUri.Should().Be(new Uri("https://example.myshopify.com/api/2026-01/graphql.json"));
        policy.Headers.Should().ContainKey(ShopifyService.REQUEST_HEADER_STOREFRONT_ACCESS_TOKEN)
            .WhoseValue.Should().ContainSingle(storefrontAccessToken);
        policy.Headers.Should().NotContainKey(ShopifyService.REQUEST_HEADER_ACCESS_TOKEN);

        using var body = JsonDocument.Parse(policy.Body);
        body.RootElement.GetProperty("query").GetString().Should().Contain("customerAccessTokenCreate");
        body.RootElement.GetProperty("variables").GetProperty("input").GetProperty("email").GetString().Should().Be(email);
        body.RootElement.GetProperty("variables").GetProperty("input").GetProperty("password").GetString().Should().Be(password);

        result.CustomerAccessToken.AccessToken.Should().Be("customer-access-token");
        result.CustomerAccessToken.ExpiresAt.Should().Be(DateTimeOffset.Parse("2026-07-18T12:34:56Z"));
        result.CustomerUserErrors.Should().BeEmpty();
    }

    [Fact]
    public async Task LoginAsync_ReturnsCustomerUserErrors()
    {
        var policy = new CapturingExecutionPolicy(
            """
            {
              "data": {
                "customerAccessTokenCreate": {
                  "customerUserErrors": [
                    {
                      "code": "UNIDENTIFIED_CUSTOMER",
                      "field": ["input", "email"],
                      "message": "Unidentified customer"
                    }
                  ],
                  "customerAccessToken": null
                }
              }
            }
            """);
        var service = new StorefrontCustomerService("example.myshopify.com", "storefront-token");
        service.SetExecutionPolicy(policy);

        var result = await service.LoginAsync("customer@example.com", "bad-password");

        result.CustomerAccessToken.Should().BeNull();
        result.CustomerUserErrors.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new StorefrontCustomerUserError
            {
                Code = "UNIDENTIFIED_CUSTOMER",
                Field = new[] { "input", "email" },
                Message = "Unidentified customer"
            });
    }

    private sealed class CapturingExecutionPolicy(string responseJson) : IRequestExecutionPolicy
    {
        public Uri RequestUri { get; private set; }
        public HttpMethod Method { get; private set; }
        public string Body { get; private set; }
        public Dictionary<string, IEnumerable<string>> Headers { get; } = new();

        public async Task<RequestResult<T>> Run<T>(
            CloneableRequestMessage baseRequestMessage,
            ExecuteRequestAsync<T> executeRequestAsync,
            CancellationToken cancellationToken,
            int? graphqlQueryCost = null)
        {
            RequestUri = baseRequestMessage.RequestUri;
            Method = baseRequestMessage.Method;
            Body = baseRequestMessage.Content is null
                ? string.Empty
                : await baseRequestMessage.Content.ReadAsStringAsync(cancellationToken);

            foreach (var header in baseRequestMessage.Headers)
            {
                Headers[header.Key] = header.Value;
            }

            return (RequestResult<T>)(object)Utils.MakeRequestResult(responseJson);
        }
    }
}
