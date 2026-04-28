import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MessageService } from '../message-service';
import { HeroService } from '../hero-service';
import { RouterLink } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-hero-creator',
  imports: [FormsModule, RouterLink],
  templateUrl: './hero-creator.html',
  styleUrl: './hero-creator.scss',
})
export class HeroCreator {
  constructor(private heroService: HeroService, private messageService: MessageService) { }
  name = '';

  create(): void {
    const trimmedName = this.name.trim();
    if (!trimmedName) {
      this.messageService.add('Hero name is required.');
      return;
    }

    const newHero = { id: uuidv4(), name: trimmedName };
    this.heroService.createHero(newHero).subscribe({
      next: hero => {
        console.log(`Hero created with id=${hero.id} and name=${hero.name}`);
        this.messageService.add(`Hero created with id=${hero.id} and name=${hero.name}`);
      },
      error: err => console.error('Error creating hero:', err),
      complete: () => console.debug('Create hero request completed.')
    });
    this.name = '';
  }
}
