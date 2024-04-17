import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class BibliothecaService {
    baseUrl = 'https://localhost:5001/api/recipes/'
    
    constructor(private http: HttpClient) {
    }

    getRecipes(){
        return this.http.get(this.baseUrl);
    }
}