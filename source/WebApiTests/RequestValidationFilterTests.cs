using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using WebApi.Filters;

namespace WebApiTests;

public class RequestValidationFilterTests
{
    // AAA testing pattern -> Arrange, Act, Assert

    [Test]
    public async Task InvokeAsync_ReturnsBadRequestAndDoesNotCallNext_WhenGuardReturnsErrors()
    {
        // ARRANGE: set up the test data, mocks, and the system under test (sut)
        var hero = CreateHero();
        var errors = new[] { "Name is required", "Name must be at least 3 characters" };

        // Create a mock implementation of IHeroGuard using NSubstitute.
        var guard = Substitute.For<IHeroGuard>(); // We can mock interfaces, that's why it's a best practice to depend on abstractions (interfaces) instead of concrete implementations (classes).

        // Set up the mock to return the specified errors when the Against method is called with the test hero.
        guard.Against(hero).Returns(errors);

        var context = CreateContext(hero);
        var next = Substitute.For<EndpointFilterDelegate>(); // We can also mock delegates, which are essentially function pointers, allowing us to verify if they were called or not.
        var sut = new RequestValidationFilter(guard);

        // ACT: call the method under test
        var result = await sut.InvokeAsync(context, next);

        // ASSERT: verify the results
        var badRequest = result as BadRequest<IEnumerable<string>>;
        await Assert.That(badRequest is not null).IsTrue();
        var returnedErrors = badRequest?.Value?.ToArray() ?? [];
        await Assert.That(returnedErrors.SequenceEqual(errors)).IsTrue();
        guard.Received(1).Against(hero);
        _ = next.DidNotReceive().Invoke(Arg.Any<EndpointFilterInvocationContext>());
    }

    [Test]
    public async Task InvokeAsync_CallsNextAndReturnsItsResult_WhenGuardReturnsNoErrors()
    {
        // ARRANGE
        var hero = CreateHero();
        var expectedResult = new { Success = true };
        var guard = Substitute.For<IHeroGuard>();
        guard.Against(hero).Returns(Array.Empty<string>());
        var context = CreateContext(hero);
        var next = Substitute.For<EndpointFilterDelegate>();
        next.Invoke(context).Returns(new ValueTask<object?>(expectedResult));
        var sut = new RequestValidationFilter(guard);

        // ACT
        var result = await sut.InvokeAsync(context, next);

        // ASSERT
        await Assert.That(ReferenceEquals(result, expectedResult)).IsTrue();
        guard.Received(1).Against(hero);
        _ = next.Received(1).Invoke(context);
    }

    [Test]
    public async Task InvokeAsync_Throws_WhenHeroArgumentIsMissing()
    {
        // ARRANGE
        var guard = Substitute.For<IHeroGuard>();
        var context = CreateContext("not-a-hero");
        var next = Substitute.For<EndpointFilterDelegate>();
        var sut = new RequestValidationFilter(guard);
        var threw = false;

        // ACT
        try
        {
            await sut.InvokeAsync(context, next);
        }
        catch (InvalidOperationException)
        {
            threw = true;
        }

        // ASSERT
        await Assert.That(threw).IsTrue();
        guard.DidNotReceive().Against(Arg.Any<Hero>());
        _ = next.DidNotReceive().Invoke(Arg.Any<EndpointFilterInvocationContext>());
    }

    [Test]
    public async Task InvokeAsync_Throws_WhenMultipleHeroArgumentsArePresent()
    {
        // ARRANGE
        var guard = Substitute.For<IHeroGuard>();
        var context = CreateContext(CreateHero("One"), CreateHero("Two"));
        var next = Substitute.For<EndpointFilterDelegate>();
        var sut = new RequestValidationFilter(guard);
        var threw = false;

        // ACT
        try
        {
            await sut.InvokeAsync(context, next);
        }
        catch (InvalidOperationException)
        {
            threw = true;
        }

        // ASSERT
        await Assert.That(threw).IsTrue();
        guard.DidNotReceive().Against(Arg.Any<Hero>());
        _ = next.DidNotReceive().Invoke(Arg.Any<EndpointFilterInvocationContext>());
    }


    #region Helper functions
    private static EndpointFilterInvocationContext CreateContext(params object?[] args)
    {
        var context = Substitute.For<EndpointFilterInvocationContext>(); // We can mock abstract classes as well, just like we mock interfaces.
        context.Arguments.Returns(args);
        return context;
    }

    private static Hero CreateHero(string name = "Batman")
    {
        return new Hero
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
    #endregion
}

/***
FYI:
It's also possible to mock concrete classes with NSubstitute, but based on the docs: 
"Be careful when specifying a class, as all non-virtual members will actually be executed. Only virtual members can be recorded or have return values specified."

Anyway, that's generally an anti-pattern to depend on concrete implementations instead of abstractions, as it makes the code less flexible and harder to test. 
Mocking concrete classes can lead to tests that are tightly coupled to the implementation details of those classes, which can make refactoring more difficult and can lead to brittle tests that break easily when the implementation changes.
***/
