import { Hero } from './domain/hero';

describe('Hero model', () => {
  it('stores a string id and a name', () => {
    const hero: Hero = {
      id: 'hero-1',
      name: 'Batman',
    };

    expect(hero.id).toBe('hero-1');
    expect(hero.name).toBe('Batman');
  });
});
