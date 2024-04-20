import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'primeng/api';

import { HomeComponent } from './home/home.component';
import { RecipeComponent } from './recipe/recipe.component';

import { DataViewModule } from 'primeng/dataview';
import { TableModule } from 'primeng/table';

@NgModule({
  declarations: [
    HomeComponent,
    RecipeComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    DataViewModule,
    TableModule
  ],
  exports: [
    HomeComponent
  ]
})
export class FeaturesModule { }
