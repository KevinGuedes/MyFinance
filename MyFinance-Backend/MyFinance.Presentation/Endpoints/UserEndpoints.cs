using MediatR;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;
using MyFinance.Presentation.Interfaces;

namespace MyFinance.Presentation.Endpoints;

public class UserEndpoints : IEndpointGroupDefinition
    //EndpointGroupDefinition (has the methods from ApiController)
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        var userGroup = builder
            .MapGroup("/user")
            .WithTags("User")
            .WithSummary("Endpoints related to user management")
            .WithDisplayName("user")
            .WithDescription("Endpoints related to user management");

        userGroup.MapPost("/sign-in", SignInAsync)
            .WithSummary("Signs in a user")
            .WithDescription("Signs in a user")
            .Produces<TooManyFailedSignInAttemptsResponse>(StatusCodes.Status429TooManyRequests)
            .Produces<SignInResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> SignInAsync(
        SignInRequest request, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(UserMapper.RTC.Map(request), cancellationToken);
        return TypedResults.Ok(response.Value);
    }
}
