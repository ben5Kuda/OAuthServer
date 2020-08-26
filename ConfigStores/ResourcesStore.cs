namespace IdentityServer.Web.ConfigStores
{
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using IdentityServer.Web.ConfigStores.Models;

    public class ResourcesStore : IResourceStore
    {

        private readonly ILogger _logger;
        IConfiguration Configuration;

        public ResourcesStore(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _logger = loggerFactory.CreateLogger("ApiResourcesStore");
        }


        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var identityResources =
                GetApiResources().IdentityResources.Where(sc => scopeNames.ToList().Contains(sc.Name));
            return Task.FromResult(identityResources);
        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var apiScope = GetApiResources().ApiScopes.Where(sc => scopeNames.ToList().Contains(sc.Name));
            return Task.FromResult(apiScope);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var apiResources = GetApiResources().ApiResources
                .Where(sc => sc.Scopes.Any(x => scopeNames.ToList().Contains(x)));
            return Task.FromResult(apiResources);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var apiResources = GetApiResources().ApiResources.Where(sc => apiResourceNames.ToList().Contains(sc.Name));
            return Task.FromResult(apiResources);
        }

        public Task<IdentityServer4.Models.Resources> GetAllResourcesAsync()
        {
            var apiResources = GetApiResources().ApiResources.Select(api => api);

            var identityResources = GetApiResources().IdentityResources.Select(id => id);

            var scopeResources = GetApiResources().ApiScopes.Select(id => id);

            var resources = new IdentityServer4.Models.Resources(identityResources, apiResources, scopeResources);
            return Task.FromResult(resources);
        }

        public Resource GetApiResources()
        {
            List<ApiResourcesStoreContext> ApiResources = new List<ApiResourcesStoreContext>();
            List<Identity> IdentityResources = new List<Identity>();

            _logger.LogInformation("Creating resource store");

            ApiResources.Add(Configuration.GetSection("apiResources").Get<ApiResourcesStoreContext>());
            IdentityResources.Add(Configuration.GetSection("identityResources").Get<Identity>());

            _logger.LogInformation("Reading from the config");

            var ApiResourcesStore = (new List<ApiResourcesStoreContext>(ApiResources)).ToArray();
            var Scopes = ApiResourcesStore[0].Scopes;

            _logger.LogInformation("Setting up the scopes");
            //Add custom permissions/scopes       
            var apiResources = from x in Scopes
                select new ApiResource
                {
                    Name = x.Name,
                    Enabled = true,
                    UserClaims = x.Claims != null
                        ? (from c in x.Claims
                            select c?.Name).ToList()
                        : null,
                    Scopes = new List<string>
                    {
                        x.Name,
                      
                    }
                };

            _logger.LogInformation("Mapping api resources");

            var identityResources = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
            _logger.LogInformation("Mapping identity reosurces");

            var resources = new Resource
            {
                ApiResources = apiResources,
                IdentityResources = identityResources
            };

            _logger.LogInformation("Mapping resources and adding to Resource store");
            return resources;
        }
    }

    public class Resource
    {
        public IEnumerable<ApiResource> ApiResources { get; set; }
        public IEnumerable<IdentityResource> IdentityResources { get; set; }
        public ICollection<ApiScope> ApiScopes { get; internal set; }
    }
}