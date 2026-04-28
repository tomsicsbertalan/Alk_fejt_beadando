import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { HeroesComponent } from './heroes';
import { HeroService } from '../hero-service';
import { MessageService } from '../message-service';

describe('HeroesComponent', () => {
  let component: HeroesComponent;
  let fixture: ComponentFixture<HeroesComponent>;

  beforeEach(async () => {
    const heroServiceMock = {
      getHeroes: () => of([]),
    };

    await TestBed.configureTestingModule({
      imports: [HeroesComponent],
      providers: [
        MessageService,
        { provide: HeroService, useValue: heroServiceMock },
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeroesComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
