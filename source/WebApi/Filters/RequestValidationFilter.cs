using Domain;

namespace WebApi.Filters;

public class RequestValidationFilter : IEndpointFilter
{
    private readonly IHeroGuard _guard;

    public RequestValidationFilter(IHeroGuard guard)
    {
        _guard = guard;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // REQUEST INTERCEPTION
        // Validate request before calling any handlers
        Hero hero = context.Arguments.OfType<Hero>().Single();
        var errors = _guard.Against(hero);
        if (errors.Any()) return TypedResults.BadRequest(errors);

        // If validation passed, call the next handler(s)
        object? result = await next(context);
        
        // RESPONSE INTERCEPTION
        // After the next handler(s) returned, we could even modify the response before it goes back to the client
        return result;
    }
}
