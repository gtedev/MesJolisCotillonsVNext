import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductContainerComponent } from './product/product-container/product-container.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const appRoutes: Routes = [
  { path: 'products', component: ProductContainerComponent },
  { path: 'dashboard', component: DashboardComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
