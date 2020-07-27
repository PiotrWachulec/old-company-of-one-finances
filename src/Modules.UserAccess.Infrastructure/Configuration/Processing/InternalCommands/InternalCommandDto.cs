using System;

namespace Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    internal class InternalCommandDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }
    }
}