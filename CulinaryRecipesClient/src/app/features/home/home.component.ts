import { Component, OnInit } from '@angular/core';
import { RecipesService } from '../../shared/services/recipes.service';
import { RecipeParams } from '../../shared/models/recipeParams';
import { RecipeHome } from '../../shared/models/recipeHome';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  recipes: RecipeHome[] = [];
  params = new RecipeParams();
  count: number = 0;
  rowsPerPageOptions: number[] = [20];

  constructor(
    private recipesService: RecipesService,
    private router: Router) 
  { }

    ngOnInit(): void {
      this.getRecipes();
    }

  getRecipes(){
    this.recipesService.getRecipes(this.params).subscribe(
      {next: (response) => {
        this.recipes = response.data;
        console.log(this.recipes);
        this.params.pageNumber = response.pageNumber;
        this.params.pageSize = response.pageSize;
        this.count = response.count;
        console.log(this.count);
      },
      error: error => console.log(error)}
    );
  }

  onPageChange(event: any) {
    const newPageNumber = event.first / event.rows + 1;
    if(this.params.pageNumber !== newPageNumber){
      this.params.pageNumber = newPageNumber;
      this.getRecipes();
    }
  }

  onRecipeClick(recipe: RecipeHome) {
    this.router.navigate(['/recipe', recipe.name]);
  }
}
