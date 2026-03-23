using Domain;

namespace WebApi.Filters;

public class HeroGuard : IHeroGuard
{
    // Shared state for collecting validation errors during the guard execution. 
    // This is not thread-safe, but since we register this as a Scoped dependency, it will be created (and used) once per request (thus safe to use in this context).
    // If more than one consumer would use this within the same request, we would need to change this to a thread-safe collection (e.g.: ConcurrentBag<string>).
    private readonly IList<string> errors = [];

    // Guards against Hero validation errors
    public IEnumerable<string> Against(Hero hero)
    {
        EnsureValidId(hero);
        EnsureValidName(hero);
        return errors;
    }

    private void EnsureValidId(Hero hero)
    {
        if (hero.Id == Guid.Empty)
            errors.Add("Hero ID is invalid.");
    }

    private void EnsureValidName(Hero hero)
    {
        if (string.IsNullOrWhiteSpace(hero.Name))
            errors.Add("Hero name is invalid.");
    }
}
