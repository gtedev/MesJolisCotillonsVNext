import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/AppState';
import { AppNavigation } from 'src/app/store/models/app/app-navigation.model';
import { NavigationItem } from 'src/app/store/models/app/navigation-item.model';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NavigateService {

  constructor(private store: Store<AppState>, private routerService: Router) { }

  public getNavigateItems(): Observable<Array<NavigationItem>> {
    return this.store.select(store => store.appNavigation.navigationItems);
  }

  public navigateTo(navigationItem: NavigationItem): void {
    this.routerService.navigateByUrl(navigationItem.routeUrl, { replaceUrl: true });
  }
}

