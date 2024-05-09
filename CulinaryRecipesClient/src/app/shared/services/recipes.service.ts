import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RecipeParams } from "../models/recipeParams";
import { Pagination } from "../models/pagination";
import { HomeRecipe } from "../models/homeRecipe";
import { DetailedRecipe } from "../models/detailedRecipe";
import { SimilarRecipe } from "../models/similarRecipe";
import { RecipeStats } from "../models/recipeStats";
import { environment } from "../../../environments/environment";
import { AuthorRecipeParams } from "../models/authorRecipeParams";
import { RecipeName } from "../models/recipeName";

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
        return this.http.get<Pagination<HomeRecipe[]>>(this.baseUrl + '/Recipes', {params});
    }

    getRecipesByAuthor(recipeParams: AuthorRecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        params = params.append('authorName', recipeParams.authorName);
        if(recipeParams.clickedRecipe && recipeParams.clickedRecipeId){
            params = params.append('clickedRecipe', recipeParams.clickedRecipe);
            params = params.append('clickedRecipeId', recipeParams.clickedRecipeId);
        }
        if(recipeParams.recipeName){
            params = params.append('recipeName', recipeParams.recipeName);
        }
        if(recipeParams.selectedIngredients){
            params = params.append('selectedIngredients', recipeParams.selectedIngredients);
        }
        return this.http.get<Pagination<HomeRecipe[]>>(this.baseUrl + '/Recipes/author', {params});
    }

    getMostComplexRecipes(recipesNumber: number){
        let params = new HttpParams();
        params = params.append('recipesNumber', recipesNumber);
        return this.http.get<RecipeStats[]>(this.baseUrl + '/Recipes/mostComplex', {params});
    }

    getRecipe(id: string){
        return this.http.get<DetailedRecipe>(this.baseUrl + '/Recipes/' + id);
    }

    getFiveMostSimilarRecipes(id: string){
        return this.http.get<SimilarRecipe[]>(this.baseUrl + '/Recipes/' + id + '/similar');
    }

    getRecipeNameById(id: string){
        return this.http.get<RecipeName>(this.baseUrl + '/Recipes/' + id + '/name');
    }
}