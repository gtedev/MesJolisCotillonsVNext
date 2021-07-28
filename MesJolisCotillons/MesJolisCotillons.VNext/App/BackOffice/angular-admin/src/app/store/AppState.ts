import { IAppUIArea } from './models/app/app-ui-area.model';
import { AppNavigation } from './models/app/app-navigation.model';
import { AppContainer } from './models/app/app-container.model';
import { RouterReducerState } from '@ngrx/router-store';
import { RouterStateUrl } from '../shared/custom-route-serializer';

export class AppState implements IAppUIArea {
    isLoading: false;
    name: 'Admin';
    appNavigation: AppNavigation;
    appContainer: AppContainer;
    router: RouterReducerState<RouterStateUrl>;
}
