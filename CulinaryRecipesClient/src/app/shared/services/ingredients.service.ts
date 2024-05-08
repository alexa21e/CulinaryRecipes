import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Pagination } from "../models/pagination";
import { IngredientParams } from "../models/ingredientParams";
import { Ingredient } from "../models/ingredient";
import { environment } from "../../../environments/environment";

@Injectable({
    providedIn: 'root'
})

export class IngredientsService {
    baseUrl = environment.baseUrl + '/api';
    
    constructor(private http: HttpClient) {
    }

    getIngredients(ingredientParams: IngredientParams){
        let params = new HttpParams();
        if(ingredientParams.name){
            params = params.append('name', ingredientParams.name);
        }
        params = params.append('pageNumber', ingredientParams.pageNumber);
        params = params.append('pageSize', ingredientParams.pageSize);
        return this.http.get<Pagination<Ingredient[]>>(this.baseUrl + '/Ingredients', {params});
    }

    getMostCommonIngredients(ingredientsNumber: number) {
        let params = new HttpParams();
        params = params.append('ingredientsNumber', ingredientsNumber);
        return this.http.get<Ingredient[]>(this.baseUrl + '/Ingredients/mostCommon', {params});
    }
}