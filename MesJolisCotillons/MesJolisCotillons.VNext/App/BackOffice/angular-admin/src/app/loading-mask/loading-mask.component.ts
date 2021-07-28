import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'loading-mask',
  templateUrl: './loading-mask.component.html',
  styleUrls: ['./loading-mask.component.scss']
})
export class LoadingMaskComponent implements OnInit {

  @Input()
  isLoading: boolean;

  constructor() { }

  ngOnInit() {
  }

}
