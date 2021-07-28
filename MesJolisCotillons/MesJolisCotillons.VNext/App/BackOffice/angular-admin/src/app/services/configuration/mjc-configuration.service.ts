import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MjcApiConfigurationService {

  public baseUrl = 'https://localhost:44355/api';

  public controllerUris = {
    products: this.baseUrl + '/products/?Page=1&PageSize=50'
  };
}
