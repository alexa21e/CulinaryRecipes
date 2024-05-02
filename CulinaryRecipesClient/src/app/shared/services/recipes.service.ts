import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RecipeParams } from "../models/recipeParams";
import { Pagination } from "../models/pagination";
import { RecipeHome } from "../models/recipeHome";
import { RecipeDetails } from "../models/recipeDetails";
import { SimilarRecipe } from "../models/similarRecipe";
import { RecipeStats } from "../models/recipeStats";

@Injectable({
    providedIn: 'root'
})

export class RecipesService {
    baseUrl = 'https://localhost:5001/api/Recipes'
    
    constructor(private http: HttpClient) {
    }

    getRecipes(recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        return this.http.get<Pagination<RecipeHome[]>>(this.baseUrl, {params});
    }

    getRecipesByName(name: string, recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('name', name);
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        return this.http.get<Pagination<RecipeHome[]>>(`${this.baseUrl}/search/name`, {params});
    }

    getRecipesByIngredients(ingredients: string[], recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        const formattedIngredients = ingredients.join(',');
        params = params.append('selectedIngredients', formattedIngredients);
        return this.http.get<Pagination<RecipeHome[]>>(this.baseUrl + '/search/ingredients' , {params});
    }

    getRecipesByAuthor(authorName: string, clickedRecipeId: string, recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        return this.http.get<Pagination<RecipeHome[]>>(`${this.baseUrl}/author/${authorName}/clickedRecipe/${clickedRecipeId}`, {params});
    }

    getRecipesByAuthorAndName(authorName: string, clickedRecipeId: string, recipeName: string, recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('recipeName', recipeName);
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        return this.http.get<Pagination<RecipeHome[]>>(`${this.baseUrl}/author/${authorName}/clickedRecipe/${clickedRecipeId}/search/name`, {params});
    }

    getRecipesByAuthorAndIngredients(authorName: string, clickedRecipeId: string, ingredients: string, recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        params = params.append('selectedIngredients', ingredients);
        return this.http.get<Pagination<RecipeHome[]>>(`${this.baseUrl}/author/${authorName}/clickedRecipe/${clickedRecipeId}/search/ingredients`, {params});
    }

    getMostComplexRecipes(recipesNumber: number){
        let params = new HttpParams();
        params = params.append('recipesNumber', recipesNumber);
        return this.http.get<RecipeStats[]>(this.baseUrl + '/mostComplex', {params});
    }

    getRecipe(id: string){
        return this.http.get<RecipeDetails>(this.baseUrl + '/' + id);
    }

    getFiveMostSimilarRecipes(id: string){
        return this.http.get<SimilarRecipe[]>(this.baseUrl + '/' + id + '/similar');
    }
}