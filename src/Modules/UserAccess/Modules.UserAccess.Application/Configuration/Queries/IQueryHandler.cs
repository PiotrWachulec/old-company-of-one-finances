using MediatR;
using Modules.UserAccess.Application.Contracts;

namespace Modules.UserAccess.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult>
        : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {

    }
}