import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RecipeParams } from "../models/recipeParams";
import { Pagination } from "../models/pagination";
import { RecipeHome } from "../models/recipeHome";
import { RecipeDetails } from "../models/recipeDetails";

@Injectable({
    providedIn: 'root'
})

export class RecipesService {
    baseUrl = 'https://localhost:5001/Recipes'
    
    constructor(private http: HttpClient) {
    }

    getRecipes(recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        return this.http.get<Pagination<RecipeHome[]>>(this.baseUrl, {params});
    }

    getRecipesByName(name: string, recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        return this.http.get<Pagination<RecipeHome[]>>(`${this.baseUrl}/search/${name}`, {params});
    }

    getRecipesByIngredients(ingredients: string[], recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        const formattedIngredients = ingredients.join(',');
        params = params.append('selectedIngredients', formattedIngredients);
        return this.http.get<Pagination<RecipeHome[]>>(this.baseUrl + '/search/ingredients' , {params});
    }

    getRecipe(id: string){
        return this.http.get<RecipeDetails>(this.baseUrl + '/' + id);
    }
}