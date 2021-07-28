import { async, ComponentFixture, TestBed, inject } from '@angular/core/testing';

import { NavMainComponent } from './nav-main.component';
import { NavigateService } from 'src/app/services/navigate/navigate.service';
import { NavigationItem } from 'src/app/store/models/app/navigation-item.model';
import { Observable } from 'rxjs';

const mockNavigateService = {
  getNavigateItems() { return new Observable<NavigationItem[]>(); },
  navigateTo(navigationItem: NavigationItem) { }
};

describe('NavMainComponent', () => {
  let component: NavMainComponent;
  let fixture: ComponentFixture<NavMainComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NavMainComponent],
      providers: [
        {
          provide: NavigateService,
          useValue: mockNavigateService
        }]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', async(inject([NavigateService], (navigateService: NavigateService) => {
    expect(component).toBeTruthy();
  })));
});
