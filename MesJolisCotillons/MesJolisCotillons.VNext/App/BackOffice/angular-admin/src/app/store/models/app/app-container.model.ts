import { IAppUIArea } from './app-ui-area.model';
import { ProductContainer } from './product-container.model';

export class AppContainer implements IAppUIArea {
    isLoading: boolean;
    name: 'app-container';
    currentUrl: string;

    productContainer: ProductContainer;


    constructor() {
        this.isLoading = false;
        this.productContainer = new ProductContainer();
    }
}
