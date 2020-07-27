using System.Reflection;

namespace Modules.UserAccess.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
    }
}