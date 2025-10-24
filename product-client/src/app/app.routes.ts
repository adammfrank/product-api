import { Routes } from '@angular/router';
import { ProductList } from '../list/list.component';
import { ErrorPage } from '../error/error.component';


export const routes: Routes = [
 
  {
    path: 'list',
    component: ProductList,
  },
  {
    path: 'error',
    component: ErrorPage
  }
];
