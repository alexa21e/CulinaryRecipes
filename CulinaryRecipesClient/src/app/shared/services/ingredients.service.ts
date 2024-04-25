import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Pagination } from "../models/pagination";
import { IngredientParams } from "../models/ingredientParams";
import { Ingredient } from "../models/ingredient";

@Injectable({
    providedIn: 'root'
})

export class IngredientsService {
    baseUrl = 'https://localhost:5001/api/Ingredients'
    
    constructor(private http: HttpClient) {
    }

    getIngredients(ingredientParams: IngredientParams){
        let params = new HttpParams();
        if(ingredientParams.name){
            params = params.append('name', ingredientParams.name);
        }
        params = params.append('pageNumber', ingredientParams.pageNumber);
        params = params.append('pageSize', ingredientParams.pageSize);
        return this.http.get<Pagination<Ingredient[]>>(this.baseUrl, {params});
    }
}