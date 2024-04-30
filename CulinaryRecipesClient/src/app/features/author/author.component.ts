import { Component, OnInit } from '@angular/core';
import { Subject, Subscription, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { RecipeHome } from '../../shared/models/recipeHome';
import { Ingredient } from '../../shared/models/ingredient';
import { RecipeParams } from '../../shared/models/recipeParams';
import { IngredientParams } from '../../shared/models/ingredientParams';
import { RecipesService } from '../../shared/services/recipes.service';
import { IngredientsService } from '../../shared/services/ingredients.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ListboxFilterEvent } from 'primeng/listbox';
import { SortEvent } from 'primeng/api';

@Component({
  selector: 'app-author',
  templateUrl: './author.component.html',
  styleUrl: './author.component.css'
})
export class AuthorComponent implements OnInit{
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

  authorName!: string;
  clickedRecipeId!: string;

  selectedSortOption: string = '_asc'; 

  constructor(
    private recipesService: RecipesService,
    private ingredientsService: IngredientsService,
    private router: Router,
    private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    let authorName = this.route.snapshot.paramMap.get('name');
    if (authorName) this.authorName = authorName;

    let clickedRecipeId = this.route.snapshot.paramMap.get('id');
    if (clickedRecipeId) this.clickedRecipeId = clickedRecipeId;

    this.getRecipesByAuthor();
    this.getIngredients();
    this.searchTerm.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) => this.ingredientsService.getIngredients({ name: term, pageNumber: 1, pageSize: 200 }))
    ).subscribe(ingredients => this.ingredients = ingredients.data);
  }

  getRecipesByAuthor() {
    this.recipeParams.sortOrder = this.selectedSortOption;
    if (this.authorName && this.clickedRecipeId) {
      this.recipesSubscription = this.recipesService.getRecipesByAuthor(this.authorName, this.clickedRecipeId, this.recipeParams).subscribe(
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
  }

  getRecipesByName() {
    this.recipesSubscription = this.recipesService.getRecipesByAuthorAndName(this.authorName, 
                this.clickedRecipeId, this.searchText, this.recipeParams).subscribe(
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
    let ingredients = this.selectedIngredients.map(i => i.name);
    let ingredientsString = ingredients.join(',');
    this.recipesSubscription = this.recipesService.getRecipesByAuthorAndIngredients(this.authorName, this.clickedRecipeId, ingredientsString, this.recipeParams).subscribe(
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
      this.getRecipesByAuthor();
    }
  }

  onRecipeClick(recipe: RecipeHome) {
    this.router.navigate(['/recipe', recipe.id]);
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
      this.getRecipesByAuthor();
  }
}

  clearFilters() {
    this.selectedIngredients = [];
    this.selectAll = false;
    this.getRecipesByAuthor();
  }

  filterRecipes() {
    this.getRecipesByIngredients();
  }

  ngOnDestroy(): void {
    this.recipesSubscription.unsubscribe();
    this.ingredientsSubscription.unsubscribe();
  }
}
