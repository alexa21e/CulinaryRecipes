import { Component, OnInit } from '@angular/core';
import { RecipesService } from '../../shared/services/recipes.service';
import { RecipeParams } from '../../shared/models/recipeParams';
import { RecipeHome } from '../../shared/models/recipeHome';
import { Router } from '@angular/router';
import { Subject, Subscription, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { IngredientsService } from '../../shared/services/ingredients.service';
import { Ingredient } from '../../shared/models/ingredient';
import { IngredientParams } from '../../shared/models/ingredientParams';
import { ListboxFilterEvent } from 'primeng/listbox';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  private recipesSubscription: Subscription = new Subscription();
  private ingredientsSubscription: Subscription = new Subscription();

  private searchTerm = new Subject<string>();
  searchText: string = '';

  recipes: RecipeHome[] = [];
  ingredients: Ingredient[] = [];

  recipeParams = new RecipeParams();
  ingredientsParams = new IngredientParams();

  recipesCount: number = 0;
  rowsPerPageOptions: number[] = [20];

  selectedIngredients!: any[];
  selectAll: any;

  constructor(
    private recipesService: RecipesService,
    private ingredientsService: IngredientsService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.getRecipes();
    this.getIngredients();
    this.searchTerm.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) => this.ingredientsService.getIngredients({ name: term, pageNumber: 1, pageSize: 500 }))
    ).subscribe(ingredients => this.ingredients = ingredients.data);
  }

  getRecipes() {
    this.recipesSubscription = this.recipesService.getRecipes(this.recipeParams).subscribe(
      {
        next: (response) => {
          this.recipes = response.data;
          this.recipeParams.pageNumber = response.pageNumber;
          this.recipeParams.pageSize = response.pageSize;
          this.recipesCount = response.count;
        },
        error: error => console.log(error)
      }
    );
  }

  getRecipesByName() {
    this.recipesSubscription = this.recipesService.getRecipesByName(this.searchText, this.recipeParams).subscribe(
      {
        next: (response) => {
          this.recipes = response.data;
          this.recipeParams.pageNumber = response.pageNumber;
          this.recipeParams.pageSize = response.pageSize;
          this.recipesCount = response.count;
        },
        error: error => console.log(error)
      }
    );
  }

  getIngredients() {
    this.ingredientsSubscription = this.ingredientsService.getIngredients(this.ingredientsParams).subscribe(
      {
        next: (response) => {
          console.log(response);
          this.ingredients = response.data;
          this.ingredientsParams.pageNumber = response.pageNumber;
          this.ingredientsParams.pageSize = response.pageSize;
          console.log(this.ingredients);
        },
        error: error => console.log(error)
      }
    );
  }

  onPageChange(event: any) {
    const newPageNumber = event.first / event.rows + 1;
    if (this.recipeParams.pageNumber !== newPageNumber) {
      this.recipeParams.pageNumber = newPageNumber;
      this.getRecipes();
    }
  }

  onRecipeClick(recipe: RecipeHome) {
    this.router.navigate(['/recipe', recipe.id]);
  }

  onIngredientsChange(event: any) {
    const { originalEvent, value } = event
    if (value) this.selectAll = value.length === this.ingredients.length;
  }

  onFilter($event: ListboxFilterEvent) {
    console.log("works");
    if ($event.originalEvent.target) {
      this.searchTerm.next(($event.originalEvent.target as HTMLInputElement).value);
    }
  }

  ngOnDestroy(): void {
    this.recipesSubscription.unsubscribe();
    this.ingredientsSubscription.unsubscribe();
  }
}
