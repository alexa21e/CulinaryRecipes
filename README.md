
# Culinary Recipes Web Application

Culinary Recipes is a .NET web app with Angular on the client side running on a Neo4J database. It enables its users to browse, search, filter and interact with recipes and their ingredients, authors, collections, diet types, and keywords.

You can find it running at https://alexa21e.github.io/CulinaryRecipes/, with its server side on https://culinary-recipes.azurewebsites.net/ 





## Features

- On the home page, all the recipes are initially sorted alphabetically inside a table with a pagination of 20, providing the name, author, number of ingredients and skill level of each recipe. The user can filter the recipes by their name, the ingredients they contain or sort both ascendingly and descendingly based on the name, number of ingredients or skill level. On the left part of the screen, the statistics of the website are displayed as follows: the five most common ingredients, the five most prolific authors and the five most complex recipes (calculated based on multiple factors such as their preparation time, their cooking time, their number of ingredients and their skill level)

- When clicking on or hovering over a recipe, the user will be redirected to the recipe's detail page where the description, the cooking time, the preparation time, the ingredients can be found. Moreover, for each recipe, there exist five most similar recipe and they are shown on this page together with their similarity procentage (calculated using [**Jaccard's**](https://medium.com/@mayurdhvajsinhjadeja/jaccard-similarity-34e2c15fb524)  similarity). At the bottom of the page, there are displayed the collections, keywords and diet types views. If the recipe is part of any, it will be showed down there.

- The user can access an author page by clicking on an author's name from the recipes table. There, a similar page to the homepage presents all the recipes written by the given author, with the same possibilities to filter and sort as presented on the homepage


