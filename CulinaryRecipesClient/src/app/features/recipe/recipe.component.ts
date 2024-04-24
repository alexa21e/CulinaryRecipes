import { Component, OnInit } from '@angular/core';
import { RecipeDetails } from '../../shared/models/recipeDetails';
import { RecipesService } from '../../shared/services/recipes.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrl: './recipe.component.css'
})
export class RecipeComponent implements OnInit {
  private recipeSubscription: Subscription = new Subscription();
  recipeDetails!: RecipeDetails;

  constructor(private recipesService: RecipesService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getRecipe();
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

  ngOnDestroy(): void {
    this.recipeSubscription.unsubscribe();
  }
}

