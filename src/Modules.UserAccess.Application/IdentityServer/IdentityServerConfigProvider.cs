using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Modules.UserAccess.Application.Contracts;

namespace Modules.UserAccess.Application.IdentityServer
{
    public static class IdentityServerConfigProvider
    {
        private static IdentityServerConfig _identityServerConfig;
        
        public static void Initialize(IdentityServerConfig identityServerConfig)
        {
            _identityServerConfig = identityServerConfig;
        }
        
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource(_identityServerConfig.ScopeName, _identityServerConfig.ScopeDisplayName)
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(CustomClaimTypes.Roles, new List<string>
                {
                    CustomClaimTypes.Roles
                })
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = _identityServerConfig.ClientId,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret(_identityServerConfig.ClientSecret.Sha256())
                    },
                    AllowedScopes = { 
                        _identityServerConfig.ScopeName,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }
    }
}