import { Recipe } from "./recipe";

export interface RecipeDetails{
    recipe: Recipe;
    ingredients: string[];
    collections: string[];
    keywords: string[];
    dietTypes: string[];
}