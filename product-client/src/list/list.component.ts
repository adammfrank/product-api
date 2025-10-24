import { Component, inject, signal } from "@angular/core";
import { ProductService } from "./product.service";
@Component({
  selector: 'product-list',
  imports: [],
  templateUrl: './list.component.html',
})
export class ProductList {
  protected readonly title = signal('product-list');
  public products?: any[];
  private productService = inject(ProductService);

  constructor() {
    this.productService.GetAll().subscribe(products => this.products = products);
  }
}
