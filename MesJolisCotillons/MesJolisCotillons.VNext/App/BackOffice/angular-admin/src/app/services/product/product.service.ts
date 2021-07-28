import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MjcApiConfigurationService } from '../configuration/mjc-configuration.service';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Product } from 'src/app/store/models/app/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private apiConfig: MjcApiConfigurationService, private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get(this.apiConfig.controllerUris.products)
      .pipe(map(response => this.mapToProduct(response)));
  }

  private mapToProduct(response: any): Product[] {
    return response.products.map(p => <Product> {
      ...p
    });
  }
}
