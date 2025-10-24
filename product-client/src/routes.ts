import { Routes } from '@angular/router';
import { App } from './app/app';
import { ProductList } from './list/list.component';
export const routes: Routes = [
  {
    path: '',
    component: App,
  },
  {
    path: '/list',
    component: ProductList,
  },
];