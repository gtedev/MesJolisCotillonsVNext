import { AppContainer } from '../models/app/app-container.model';
import { ActionTypes } from '../actions/types.actions';

export const initialAppContainer: AppContainer = new AppContainer();

export function AppContainerReducer(state: AppContainer = initialAppContainer, action: any) {

    const newState = { ...state };

    switch (action.type) {

        case ActionTypes.PRODUCT_CONTAINER_LOADING:
            newState.productContainer.isLoading = true;
            return newState;

        case ActionTypes.PRODUCT_CONTAINER_LOADED:
            newState.productContainer.isLoading = false;
            newState.productContainer.products = action.payload;
            return newState;

        default:
            return state;
    }
}
