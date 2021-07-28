import { IAppUIArea } from './app-ui-area.model';
import { Product } from './product.model';

export class ProductContainer implements IAppUIArea {
    isLoading: boolean;
    name: 'product-container';
    products: Product[];


    constructor() {
        this.isLoading = false;
    }
}
