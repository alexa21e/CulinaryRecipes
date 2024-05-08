import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RecipeParams } from "../models/recipeParams";
import { Pagination } from "../models/pagination";
import { RecipeHome } from "../models/recipeHome";
import { RecipeDetails } from "../models/recipeDetails";
import { SimilarRecipe } from "../models/similarRecipe";
import { RecipeStats } from "../models/recipeStats";
import { environment } from "../../../environments/environment";

@Injectable({
    providedIn: 'root'
})

export class RecipesService {
    baseUrl = environment.baseUrl + '/api';
    
    constructor(private http: HttpClient) {
    }

    getRecipes(recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        if(recipeParams.recipeName){
            params = params.append('recipeName', recipeParams.recipeName);
        }
        if(recipeParams.selectedIngredients){
            params = params.append('selectedIngredients', recipeParams.selectedIngredients);
        }
        return this.http.get<Pagination<RecipeHome[]>>(this.baseUrl + '/Recipes', {params});
    }

    getRecipesByAuthor(authorName: string, clickedRecipeId: string, recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        return this.http.get<Pagination<RecipeHome[]>>(`${this.baseUrl}/Recipes/author/${authorName}/clickedRecipe/${clickedRecipeId}`, {params});
    }

    getRecipesByAuthorAndName(authorName: string, clickedRecipeId: string, recipeName: string, recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('recipeName', recipeName);
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        return this.http.get<Pagination<RecipeHome[]>>(`${this.baseUrl}/Recipes/author/${authorName}/clickedRecipe/${clickedRecipeId}/search/name`, {params});
    }

    getRecipesByAuthorAndIngredients(authorName: string, clickedRecipeId: string, ingredients: string, recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        params = params.append('selectedIngredients', ingredients);
        return this.http.get<Pagination<RecipeHome[]>>(`${this.baseUrl}/Recipes/author/${authorName}/clickedRecipe/${clickedRecipeId}/search/ingredients`, {params});
    }

    getMostComplexRecipes(recipesNumber: number){
        let params = new HttpParams();
        params = params.append('recipesNumber', recipesNumber);
        return this.http.get<RecipeStats[]>(this.baseUrl + '/Recipes/mostComplex', {params});
    }

    getRecipe(id: string){
        return this.http.get<RecipeDetails>(this.baseUrl + '/Recipes/' + id);
    }

    getFiveMostSimilarRecipes(id: string){
        return this.http.get<SimilarRecipe[]>(this.baseUrl + '/Recipes/' + id + '/similar');
    }
}