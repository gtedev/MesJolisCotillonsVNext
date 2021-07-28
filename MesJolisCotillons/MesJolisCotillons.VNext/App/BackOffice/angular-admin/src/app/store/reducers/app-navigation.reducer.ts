import { AppNavigation } from '../models/app/app-navigation.model';
import { NavigationItem } from '../models/app/navigation-item.model';

export const initialAppNavigation: AppNavigation = new AppNavigation();

export function AppNavigationReducer(state: AppNavigation = initialAppNavigation, action: any) {

    if (action.type === '@ngrx/router-store/navigation') {
        const newNavigationItems = state.navigationItems.map(navItem => {
            const isSelectedResult = action.payload.routerState.url === navItem.routeUrl;
            return { ...navItem, isSelected: isSelectedResult } as NavigationItem;
        });

        return { ...state, navigationItems: newNavigationItems };
    }

    return state;
}
