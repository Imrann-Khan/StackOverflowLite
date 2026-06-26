using ErrorOr;
using MediatR;

namespace StackOverflowLite.Application.Services.Interfaces;

public interface ICommand : IRequest<ErrorOr<Success>>
{
}

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>
{
}

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>
{
}