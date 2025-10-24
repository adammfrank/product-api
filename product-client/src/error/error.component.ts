import { Component, inject, signal } from "@angular/core";
@Component({
  selector: 'product-list',
  imports: [],
  templateUrl: './error.component.html',
})
export class ErrorPage {
  protected readonly title = signal('error');

}
