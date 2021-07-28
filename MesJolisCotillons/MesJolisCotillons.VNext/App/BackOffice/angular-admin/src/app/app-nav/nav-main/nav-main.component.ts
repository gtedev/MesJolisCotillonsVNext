import { Component, OnInit } from '@angular/core';
import { NavigationItem } from 'src/app/store/models/app/navigation-item.model';
import { NavigateService } from 'src/app/services/navigate/navigate.service';

@Component({
  selector: 'nav-main',
  templateUrl: './nav-main.component.html',
  styleUrls: ['./nav-main.component.scss']
})
export class NavMainComponent implements OnInit {

  navigationItems: Array<NavigationItem>;

  constructor(private navigateService: NavigateService) { }

  ngOnInit() {
    this.navigateService.getNavigateItems().subscribe((navigationItems: Array<NavigationItem>) => {
      this.navigationItems = navigationItems;
    });
  }

  navigateTo(navigationItem: NavigationItem) {
    this.navigateService.navigateTo(navigationItem);
  }
}
