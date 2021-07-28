import { NavigationItem } from './navigation-item.model';
import { IAppUIArea } from './app-ui-area.model';

export class AppNavigation implements IAppUIArea {
    id: string;
    isLoading: false;
    name: 'app-navigation';
    navigationItems: NavigationItem[];

    constructor() {
        this.navigationItems = [
            {
                displayName: 'Tableau de bord',
                isSelected: false,
                routeUrl: '/dashboard'
            },
            {
                displayName: 'Produits',
                isSelected: false,
                routeUrl: '/products'
            }];
    }
}

