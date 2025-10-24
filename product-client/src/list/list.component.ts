import { Component, inject, signal } from "@angular/core";
import { ProductService } from "./product.service";
import { Router } from "@angular/router";
import { Product } from "./product.model";
@Component({
  selector: 'product-list',
  imports: [],
  templateUrl: './list.component.html',
})
export class ProductList {
  protected readonly title = signal('product-list');
  public products?: Product[];
  private productService = inject(ProductService);
  private router = inject(Router);

  constructor() {
    this.productService.GetAll().subscribe({
    next: (products) => {
      this.products = products;
    },
    error: (err) => {
      console.error('Error loading products:', err);
      this.router.navigate(['/error']);
    }
  });
  }
}
