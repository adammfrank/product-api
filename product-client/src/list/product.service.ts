import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable, Subscription } from "rxjs";

@Injectable({providedIn: 'root'})
export class ProductService {
    private http = inject(HttpClient)


    GetAll(): Observable<any> {
        return this.http.get("http://localhost:5290/api/products");
    }

}