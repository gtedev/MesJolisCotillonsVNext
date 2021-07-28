import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import { ProductService } from 'src/app/services/product/product.service';
import { ActionTypes } from '../actions/types.actions';

@Injectable()
export class ProductEffects {

    loadProducts$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ActionTypes.PRODUCT_CONTAINER_LOADING),
            mergeMap(() => this.productService.getProducts()
                .pipe(
                    map(products => ({ type: ActionTypes.PRODUCT_CONTAINER_LOADED, payload: products })),
                    catchError(() => of({ type: '[Product API] Products Loaded Error' }))
                )
            )
        )
    );

    constructor(
        private actions$: Actions,
        private productService: ProductService
    ) { }
}
