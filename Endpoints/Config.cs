namespace IdSrvr4Demo.Endpoints
{
  using IdentityModel;
  using IdentityServer4;
  using IdentityServer4.Models;
  using IdentityServer4.Test;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Security.Claims;
  using System.Threading.Tasks;

  public class Config
  {
    // scopes define the resources in your system
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
      //Add custom claims
      var customProfile = new IdentityResource(
         name: "custom.profile",
         displayName: "Custom profile",
         claimTypes: new[] { "unique_name", "given_name", "family_name", "email" });

      return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                customProfile
            };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
      //Add custom permissions/scopes
      return new List<ApiResource>
            {
                new ApiResource{
                  Scopes = { new Scope("access") },
                  Enabled = true,

                   UserClaims = {
                      "unique_name",
                      JwtClaimTypes.Email,
                      JwtClaimTypes.BirthDate,
                      JwtClaimTypes.PhoneNumber,
                      JwtClaimTypes.Role,
                      JwtClaimTypes.GivenName,
                      JwtClaimTypes.FamilyName,
                      JwtClaimTypes.PreferredUserName },
                  },
                new ApiResource("read", "Read identity"),
                new ApiResource("roles", "Read roles")
            };
    }

    // clients want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients()
    {
      // client credentials client
      return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                   // RedirectUris 
                    
                   //  ResponseType = "code id_token",
                   //SaveTokens = true,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = {
                       "access",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         },


                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 3600,
                    Claims = new List<Claim>
                    {},
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        "access",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                  }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "Core API",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    FrontChannelLogoutSessionRequired = false,
                    BackChannelLogoutSessionRequired = false,
                  //  RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "https://localhost:44362/swagger/oauth2-redirect.html", "https://localhost:5051/swagger/oauth2-redirect.html", "http://localhost:5050/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "http://localhost:53186/signout-callback-oidc" },


                    AllowedScopes =
                    {
                        "access",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true //important for implicit flow
                },

                //https://localhost:44305
                new Client
                {
                    ClientId = "marketplace",
                    ClientName = "Market Place",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    ClientSecrets =
                    {
                        new Secret("secret")
                    },

                    RedirectUris = { "https://localhost:44305/swagger/oauth2-redirect.html", "https://localhost:5094/swagger/oauth2-redirect.html"},
                    PostLogoutRedirectUris = { "https://localhost:44305/swagger/oauth2-redirect.html" },

                    AllowedScopes =
                    {
                        "access",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true //important for implicit flow
                },
                    new Client
                {
                    ClientId = "api.ticket.marketplace",
                    ClientName = "Market Place Azure",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    ClientSecrets =
                    {
                        new Secret("secret")
                    },

                    RedirectUris = { "https://marketplaceticketingapibeta.azurewebsites.net/swagger/oauth2-redirect.html", "https://localhost:5094/swagger/oauth2-redirect.html"},
                    PostLogoutRedirectUris = { "https://marketplaceticketingapibeta.azurewebsites.net/swagger/oauth2-redirect.html" },

                    AllowedScopes =
                    {
                        "access",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true //important for implicit flow
                },
                        new Client
                {
                    ClientId = "web.ticket.marketplace",
                    ClientName = "Web Ticket SPA",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    ClientSecrets =
                    {
                        new Secret("secret")
                    },

                    RedirectUris = { "https://localhost:4200", "https://localhost:4200/auth-callback", "http://localhost:4200", "http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = { "https://localhost:4200", "https://localhost:4200" },

                    AllowedScopes =
                    {
                        "access",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true //important for implicit flow
                },
                 new Client
                {
                    ClientId = "ticket.marketplace",
                    ClientName = "Market Place",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    ClientSecrets =
                    {
                        new Secret("secret")
                    },

                    RedirectUris = { "https://localhost:44372/swagger/oauth2-redirect.html", "https://localhost:5094/swagger/oauth2-redirect.html"},
                    PostLogoutRedirectUris = { "https://localhost:44372/swagger/oauth2-redirect.html" },

                    AllowedScopes =
                    {
                        "access",
                        "offline_access",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true //important for implicit flow
                },

                   new Client
                {
                    ClientId = "ticketweb",
                    ClientName = "Ticket Web",

                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                         new Secret("secret".Sha256())
                    },

                    RedirectUris = { "https://localhost:44302/signin-oidc", "https://localhost:5077/signin-oidc"},
                    PostLogoutRedirectUris = { "https://localhost:44302" },

                    AllowedScopes =
                    {
                        "read",
                        "roles",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true //important for implicit flow
                },


            };
    }

    public static List<TestUser> GetUsers()
    {
      return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("office", "office_number"),
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
    }
  }
}