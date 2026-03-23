using Domain;

namespace WebApi.Filters;

public interface IHeroGuard
{
    IEnumerable<string> Against(Hero hero);
}
