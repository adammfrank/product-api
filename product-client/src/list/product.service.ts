import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable, Subscription } from "rxjs";
import { Product } from "./product.model";

@Injectable({providedIn: 'root'})
export class ProductService {
    private http = inject(HttpClient)


    GetAll(): Observable<Product[]> {
        return this.http.get<Product[]>("http://localhost:5290/api/products");
    }

}