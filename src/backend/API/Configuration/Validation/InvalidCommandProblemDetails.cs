using System.Collections.Generic;
using BuildingBlocks.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Configuration.Validation
{
    public class InvalidCommandProblemDetails : ProblemDetails
    {
        public List<string> Errors { get; }
        
        public InvalidCommandProblemDetails(InvalidCommandException exception)
        {
            Title = "Command validation error";
            Status = StatusCodes.Status400BadRequest;
            Type = "https://companyofonefinances.com/validation-error";
            Errors = exception.Errors;
        }
    }
}