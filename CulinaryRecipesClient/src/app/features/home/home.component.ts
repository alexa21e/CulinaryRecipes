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
import { SortEvent } from 'primeng/api';

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

  selectedSortOption: string = '_asc'; 

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
      switchMap((term: string) => this.ingredientsService.getIngredients({ name: term, pageNumber: 1, pageSize: 200 }))
    ).subscribe(ingredients => this.ingredients = ingredients.data);
  }

  getRecipes() {
    this.recipeParams.sortOrder = this.selectedSortOption;
    this.recipesSubscription = this.recipesService.getRecipes(this.recipeParams).subscribe(
      {
        next: (response) => {
          this.recipes = response.data;
          this.recipeParams.pageNumber = response.pageNumber;
          this.recipeParams.pageSize = response.pageSize;
          this.recipeParams.sortOrder = response.sortOrder;
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

  getRecipesByIngredients() {
    const ingredients = this.selectedIngredients.map(i => i.name);
    this.recipesSubscription = this.recipesService.getRecipesByIngredients(ingredients, this.recipeParams).subscribe(
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
          this.ingredients = response.data;
          this.ingredientsParams.pageNumber = response.pageNumber;
          this.ingredientsParams.pageSize = response.pageSize;
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

  onAuthorClick(event: Event, authorName: string, clickedRecipeId: string) {
    event.stopPropagation();
    this.router.navigate(['/author', authorName, 'id', clickedRecipeId] );
  }

  onIngredientsChange(event: any) {
    const { originalEvent, value } = event
    if (value) this.selectAll = value.length === this.ingredients.length;
  }

  onIngredientsSearch($event: ListboxFilterEvent) {
    if ($event.originalEvent.target) {
      this.searchTerm.next(($event.originalEvent.target as HTMLInputElement).value);
    }
  }

  onSort(event: SortEvent) {
    console.log(event);
    if (event) {
      const sortField = event.field; 
      const sortOrder = event.order === 1 ? 'asc' : 'desc';
      const formattedSortOrder = `${sortField}_${sortOrder}`;
      this.selectedSortOption = formattedSortOrder;
      this.getRecipes();
    }
  }

  clearFilters() {
    this.selectedIngredients = [];
    this.selectAll = false;
    this.getRecipes();
  }

  filterRecipes() {
    this.getRecipesByIngredients();
  }

  ngOnDestroy(): void {
    this.recipesSubscription.unsubscribe();
    this.ingredientsSubscription.unsubscribe();
  }
}
