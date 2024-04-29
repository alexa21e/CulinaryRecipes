import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { RecipeComponent } from './features/recipe/recipe.component';
import { AuthorComponent } from './features/author/author.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'author/:name/id/:id', component: AuthorComponent },
  { path: 'recipe/:id', component: RecipeComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
