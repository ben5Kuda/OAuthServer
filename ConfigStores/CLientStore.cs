namespace IdentityServer.Web.ConfigStores
{
  using System.Linq;
  using IdentityServer4.Models;
  using IdentityServer4.Stores;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.Logging;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using IdentityServer.Web.ConfigStores.Models;

  public class ClientStore : IClientStore
  {

    private readonly ILogger _logger;
    IConfiguration Configuration;

    public ClientStore(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
      Configuration = configuration;
      _logger = loggerFactory.CreateLogger("ClientStore");
    }

    public Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
    {
      _logger.LogInformation("Creating client store");

      List<ClientStoreContext> Hybrid = new List<ClientStoreContext>();
      List<ClientStoreContext> Implicit = new List<ClientStoreContext>();
      List<ClientStoreContext> ResourceOwner = new List<ClientStoreContext>();

      Hybrid.Add(Configuration.GetSection("hybrid").Get<ClientStoreContext>());
      Implicit.Add(Configuration.GetSection("implicit").Get<ClientStoreContext>());
      ResourceOwner.Add(Configuration.GetSection("resourceOwner").Get<ClientStoreContext>());

      _logger.LogInformation("Reading values from config");

      var ImplicitStore = (new List<ClientStoreContext>(Implicit)).ToArray();
      var ImplicitClients = ImplicitStore[0].ImplicitClients;

      _logger.LogInformation("Setting up implicit clients");

      var ResourceOwnerStore = (new List<ClientStoreContext>(ResourceOwner)).ToArray();
      var ResourceOwnerClients = ResourceOwnerStore[0].ResourceOwnerClients;

      _logger.LogInformation("Setting up resource owner clients");

      var HybridStore = (new List<ClientStoreContext>(Hybrid)).ToArray();
      var HybridClients = HybridStore[0].HybridClients;

      _logger.LogInformation("Setting up hybrid clients");

      var ClientsStore = ResourceOwnerClients.Union(ImplicitClients).Union(HybridClients);

      _logger.LogInformation("Combining all clients");

      var client = (from c in ClientsStore
                    where c.ClientId == clientId
                    select new IdentityServer4.Models.Client
                    {
                      ClientId = c.ClientId,
                      ClientName = c.Name,
                      AccessTokenLifetime = c.AccessTokenLifetime,
                      AllowOfflineAccess = c.AllowOfflineAccess, //refresh tokens
                      AccessTokenType = AccessTokenType.Jwt,
                      UpdateAccessTokenClaimsOnRefresh = true,
                      AbsoluteRefreshTokenLifetime = c.AbsoluteRefreshTokenLifetime,
                      AllowedGrantTypes = c.AllowedGrantTypes?.Select(x => x.Name).ToList(),
                      AllowedCorsOrigins = c.AllowedCorsOrigins?.Select(x => x.Name).ToList(),
                      AlwaysIncludeUserClaimsInIdToken = true,
                      FrontChannelLogoutSessionRequired = false,
                      BackChannelLogoutSessionRequired = false,
                      AllowRememberConsent = false,
                      Enabled = true,
                      RefreshTokenUsage = c.RefreshTokenUsage == TokenUsage.ReUse.ToString() ? TokenUsage.ReUse : TokenUsage.OneTimeOnly,
                      RequireConsent = c.RequireConsent,//remove consent screen
                      IdentityTokenLifetime = c.IdentityTokenLifetime,
                      RedirectUris = c.RedirectUris?.Select(x => x.Name).ToList(),
                      AllowedScopes = c.AllowedScopes?.Select(x => x.Name).ToList(),
                      SlidingRefreshTokenLifetime = c.SlidingRefreshTokenLifetime,
                      RefreshTokenExpiration = TokenExpiration.Absolute,
                      AllowAccessTokensViaBrowser = c.AllowAccessTokensViaBrowser,
                      PostLogoutRedirectUris = c.PostLogoutRedirectUris?.Select(x => x.Name).ToList(),
                      ClientSecrets = new List<Secret>
                      {
                        new Secret
                        {
                           Value = c.ClientSecret.Sha256()
                        }
                      }
                    }).FirstOrDefault();

      _logger.LogInformation("Mapping clients and adding to Client store");

      return Task.FromResult(client);
    }


  }
}
