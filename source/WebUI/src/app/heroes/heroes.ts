import { Component, OnInit, signal } from '@angular/core';
import { Hero } from '../domain/hero';
import { HeroDetail } from '../hero-detail/hero-detail';
import { HeroService } from '../hero-service';
import { MessageService } from '../message-service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-heroes',
  templateUrl: './heroes.html',
  styleUrls: ['./heroes.scss'],
  imports: [HeroDetail, CommonModule]
})
export class HeroesComponent implements OnInit {
  private readonly pageSize = 5;

  constructor(private heroService: HeroService, private messageService: MessageService) { }

  heroes = signal<Hero[]>([]);
  selectedHero = signal<Hero | undefined>(undefined);
  currentPage = signal(1);

  ngOnInit(): void {
    this.getHeroes();
  }

  paginatedHeroes(): Hero[] {
    const startIndex = (this.currentPage() - 1) * this.pageSize;
    return this.heroes().slice(startIndex, startIndex + this.pageSize);
  }

  totalPages(): number {
    return Math.max(1, Math.ceil(this.heroes().length / this.pageSize));
  }

  canGoToPreviousPage(): boolean {
    return this.currentPage() > 1;
  }

  canGoToNextPage(): boolean {
    return this.currentPage() < this.totalPages();
  }

  goToPreviousPage(): void {
    if (this.canGoToPreviousPage()) {
      this.currentPage.update(page => page - 1);
    }
  }

  goToNextPage(): void {
    if (this.canGoToNextPage()) {
      this.currentPage.update(page => page + 1);
    }
  }

  getHeroes(): void {
    this.heroService.getHeroes()
      .subscribe(
        {
          next: heroes => {
            this.heroes.set(heroes);
            this.currentPage.set(1);
          },
          error: error => console.error(error),
          complete: () => console.log('Completed fetching heroes')
        });
  }

  onSelect(hero: Hero): void {
    if (this.selectedHero() === hero) {
      this.selectedHero.set(undefined); // Clear the selected hero if the same hero is clicked again
      return;
    }
    this.selectedHero.set(hero);
    this.messageService.add(`HeroesComponent: ${hero ? `Selected hero id=${hero.id}` : 'Cleared selected hero'}`);
  }

  onDelete(deletedHeroId: string): void {
    this.heroes.update(heroes => heroes.filter(hero => hero.id !== deletedHeroId));
    if (this.selectedHero()?.id === deletedHeroId) {
      this.selectedHero.set(undefined); // Clear the selected hero if it was deleted
    }
    this.ensureCurrentPageIsValid();
  }

  onCancel(): void {
    this.selectedHero.set(undefined);
  }

  onSaved(updatedHero: Hero): void {
    this.heroes.update(heroes =>
      heroes.map(hero => hero.id === updatedHero.id ? updatedHero : hero)
    );
    this.selectedHero.set(updatedHero);
  }

  private ensureCurrentPageIsValid(): void {
    const lastPage = this.totalPages();
    if (this.currentPage() > lastPage) {
      this.currentPage.set(lastPage);
    }
  }

}
