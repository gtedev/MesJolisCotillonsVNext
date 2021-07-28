import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnInit {

  constructor() { }

  selectedItemIndex: number;

  @Input()
  columns: any = [];

  @Input()
  rows: any = [];

  ngOnInit() {
  }

  private getRowCells(row: any): any {

    const rowCells = [];

    this.columns.forEach(col => {
      const rowCell = {
        cellText: row[col.name],
        cellType: col.columnType ? col.columnType : 'text'
      };
      rowCells.push(rowCell);
    });

    return rowCells;
  }

  private onCheckboxClick(selectedItemIdex: number) {
    if (this.selectedItemIndex === selectedItemIdex) {
      this.selectedItemIndex = null;
    } else {
      this.selectedItemIndex = selectedItemIdex;
    }
  }
}

