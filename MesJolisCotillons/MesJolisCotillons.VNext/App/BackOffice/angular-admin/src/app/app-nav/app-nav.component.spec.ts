import { async, ComponentFixture, TestBed, inject } from '@angular/core/testing';

import { AppNavComponent } from './app-nav.component';
import { NavMainComponent } from './nav-main/nav-main.component';
import { NavFooterComponent } from './nav-footer/nav-footer.component';
import { NavigateService } from '../services/navigate/navigate.service';
import { NavigationItem } from '../store/models/app/navigation-item.model';
import { Observable } from 'rxjs';

const mockNavigateService = {
  getNavigateItems() { return new Observable<NavigationItem[]>(); },
  navigateTo(navigationItem: NavigationItem) { }
};

describe('AppNavComponent', () => {
  let component: AppNavComponent;
  let fixture: ComponentFixture<AppNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AppNavComponent, NavMainComponent, NavFooterComponent],
      providers: [
        {
          provide: NavigateService,
          useValue: mockNavigateService
        }]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', async(inject([NavigateService], (navigateService: NavigateService) => {
    expect(component).toBeTruthy();
  })));
});
