using System;
using System.Collections.Generic;

namespace BuildingBlocks.Application
{
    public class InvalidCommandException : Exception
    {
        public List<string> Errors { get; }

        public InvalidCommandException(List<string> errors)
        {
            Errors = errors;
        }
    }
}