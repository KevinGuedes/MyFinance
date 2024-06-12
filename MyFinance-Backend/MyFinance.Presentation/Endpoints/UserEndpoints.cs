using MediatR;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app
            .MapGroup("/user")
            .WithTags("User")
            .WithSummary("Endpoints related to user management")
            .WithDisplayName("user")
            .WithDescription("Endpoints related to user management")
            .AllowAnonymous();

        userGroup.MapPost(
            "/sign-in",
            async (
                [SwaggerRequestBody("User's sign in credentials", Required = true)]
                SignInRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(UserMapper.RTC.Map(request), cancellationToken);
            return TypedResults.Ok(response.Value);

        })
            .WithSummary("Signs in a user")
            .WithDescription("Signs in a user")
            .Produces<SignInResponse>(StatusCodes.Status200OK)
            .Produces<TooManyFailedSignInAttemptsResponse>(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();
    }
}
