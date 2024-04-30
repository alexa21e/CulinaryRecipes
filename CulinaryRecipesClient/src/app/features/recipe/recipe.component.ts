import { Component, OnInit } from '@angular/core';
import { RecipeDetails } from '../../shared/models/recipeDetails';
import { RecipesService } from '../../shared/services/recipes.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { SimilarRecipe } from '../../shared/models/similarRecipe';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrl: './recipe.component.css'
})
export class RecipeComponent implements OnInit {
  private recipeSubscription: Subscription = new Subscription();
  private similarRecipesSubscription: Subscription = new Subscription();
  recipeDetails!: RecipeDetails;
  similarRecipes: SimilarRecipe[] = [];

  constructor(private recipesService: RecipesService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getRecipe();
    this.getFiveMostSimilarRecipes();
  }

  getRecipe() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.recipeSubscription = this.recipesService.getRecipe(id).subscribe(
        {
          next: (response) => {
            this.recipeDetails = response;
          },
          error: error => console.log(error)
        }
      );
    }
  }

  getFiveMostSimilarRecipes() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.similarRecipesSubscription = this.recipesService.getFiveMostSimilarRecipes(id).subscribe(
        {
          next: (response) => {
            this.similarRecipes = response;
          },
          error: error => console.log(error)
        }
      );
    }
  }

  ngOnDestroy(): void {
    this.recipeSubscription.unsubscribe();
    this.similarRecipesSubscription.unsubscribe();
  }
}

