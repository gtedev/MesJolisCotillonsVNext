import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../store/AppState';

@Component({
  selector: 'app-container',
  templateUrl: './app-container.component.html',
  styleUrls: ['./app-container.component.scss']
})
export class AppContainerComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }
}
