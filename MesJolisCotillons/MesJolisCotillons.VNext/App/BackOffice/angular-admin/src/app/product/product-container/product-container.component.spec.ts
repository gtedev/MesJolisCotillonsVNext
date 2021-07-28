import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductContainerComponent } from './product-container.component';
import { LoadingMaskComponent } from 'src/app/loading-mask/loading-mask.component';
import { GridComponent } from 'src/app/grid/grid.component';

describe('ProductContainerComponent', () => {
  let component: ProductContainerComponent;
  let fixture: ComponentFixture<ProductContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoadingMaskComponent, GridComponent,  ProductContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  // Comment temporarily unit test for Product Container
  // it('should create', () => {
  //   expect(component).toBeTruthy();
  // });
});
