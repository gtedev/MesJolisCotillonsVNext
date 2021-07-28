import { Component, OnInit, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import { ActionTypes } from 'src/app/store/actions/types.actions';
import { AppState } from 'src/app/store/AppState';
import { Product } from 'src/app/store/models/app/product.model';

@Component({
  selector: 'product-container',
  templateUrl: './product-container.component.html',
  styleUrls: ['./product-container.component.scss']
})
export class ProductContainerComponent implements OnInit {

  @Input()
  isContainerLoading: boolean;

  private grid = {
    columns: [
      { columnType: 'check' },
      { name: 'name', displayName: 'NOM' },
      { name: 'category', displayName: 'CATEGORIE' },
      { name: 'displayName', displayName: 'NOM AFFICHE' },
      { name: 'price', displayName: 'PRIX' },
      { name: 'status', displayName: 'STATUT' },
      { name: 'action', displayName: 'ACTION' },
    ],
    rows: []
  };

  constructor(private store: Store<AppState>) { }

  ngOnInit() {
    const me = this;

    this.store.select(store => store).subscribe((store) => {
      me.isContainerLoading = store.appContainer.productContainer.isLoading;

      if (store.appContainer.productContainer.products) {
        const products = store.appContainer.productContainer.products;
        this.grid.rows = this.mapToRows(products);
      }
    });

    this.store.dispatch({ type: ActionTypes.PRODUCT_CONTAINER_LOADING });
  }

  mapToRows(products: Product[]): any {
    return products.map(p => <any> {
      ...p,
      status: '',
      action: ''
    });
  }
}

