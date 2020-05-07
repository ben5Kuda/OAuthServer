namespace IdentityServer.Web.ConfigStores.Models
{
  using System.Collections.Generic;

  public class ClientStoreContext
  {
    public ICollection<Client> ResourceOwnerClients { get; set; }
    public ICollection<Client> ImplicitClients { get; set; }
    public ICollection<Client> HybridClients { get; set; }
  }
  public class Client
  {
    public string Name { get; set; }
    public bool AllowAccessTokensViaBrowser { get; set; }
    public bool AllowOfflineAccess { get; set; }
    public ICollection<Values> AllowedGrantTypes { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public int AccessTokenLifetime { get; set; }
    public int IdentityTokenLifetime { get; set; }
    public int AbsoluteRefreshTokenLifetime { get; set; }
    public int SlidingRefreshTokenLifetime { get; set; }
    public string RefreshTokenUsage { get; set; }
    public string RefreshTokenExpiration { get; set; }
    public bool RequireConsent { get; set; }
    public string LogoutUri { get; set; }
    public bool LogoutSessionRequired { get; set; }
    public ICollection<Values> AllowedScopes { get; set; }
    public ICollection<Values> RedirectUris { get; set; }
    public ICollection<Values> AllowedCorsOrigins { get; set; }
    public ICollection<Values> PostLogoutRedirectUris { get; set; }
  }

  public class Values
  {
    public string Name { get; set; }
  }
}

