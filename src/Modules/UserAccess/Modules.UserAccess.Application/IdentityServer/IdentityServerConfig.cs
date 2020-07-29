namespace Modules.UserAccess.Application.IdentityServer
{
    public class IdentityServerConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ScopeName { get; set; }
        public string ScopeDisplayName { get; set; }
        public string Authority { get; set; }
    }
}