import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppContainerComponent } from './app-container/app-container.component';
import { AppHeaderComponent } from './app-header/app-header.component';
import { AppNavComponent } from './app-nav/app-nav.component';
import { HeaderLogoComponent } from './app-header/header-logo/header-logo.component';
import { HeaderTitleComponent } from './app-header/header-title/header-title.component';
import { NavMainComponent } from './app-nav/nav-main/nav-main.component';
import { NavFooterComponent } from './app-nav/nav-footer/nav-footer.component';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { AppNavigationReducer } from './store/reducers/app-navigation.reducer';
import { AppContainerReducer } from './store/reducers/app-container.reducer';
import { NavigateService } from './services/navigate/navigate.service';
import { StoreRouterConnectingModule, routerReducer, RouterStateSerializer } from '@ngrx/router-store';
import { ProductContainerComponent } from './product/product-container/product-container.component';
import { CustomSerializer } from './shared/custom-route-serializer';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoadingMaskComponent } from './loading-mask/loading-mask.component';
import { GridComponent } from './grid/grid.component';
import { HttpClientModule } from '@angular/common/http';
import { ProductService } from './services/product/product.service';
import { MjcApiConfigurationService } from './services/configuration/mjc-configuration.service';
import { EffectsModule } from '@ngrx/effects';
import { ProductEffects } from './store/effects/products.effects';

@NgModule({
  declarations: [
    AppComponent,
    AppContainerComponent,
    AppHeaderComponent,
    AppNavComponent,
    HeaderLogoComponent,
    HeaderTitleComponent,
    NavMainComponent,
    NavFooterComponent,
    ProductContainerComponent,
    DashboardComponent,
    LoadingMaskComponent,
    GridComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    StoreModule.forRoot({
      appNavigation: AppNavigationReducer,
      appContainer: AppContainerReducer,
      route: routerReducer
    }),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: environment.production }),
    // Connects RouterModule with StoreModule
    StoreRouterConnectingModule.forRoot({
      stateKey: 'router'
    }),
    EffectsModule.forRoot([ProductEffects])
  ],
  providers: [
    NavigateService,
    ProductService,
    MjcApiConfigurationService,
    { provide: RouterStateSerializer, useClass: CustomSerializer },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
