using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace RecipeBook
{
    public class RecipeBookInfo
    {
        private Person _person;
        private string _apiId = "8242e057";
        private string _apiKey = "52033be95890a90bdf1f0af120eed2fd";

        public void Run()
        {
            List<string> loginOptions = new List<string>
            {
                "1) View existing people",
                "2) Create new person"
            };
            Console.WriteLine("Welcome to the recipe book application. How would you like to sign in?");
            foreach (string loginOption in loginOptions)
                Console.WriteLine(loginOption);
            string input = Console.ReadLine();
            switch (input)
            {
                default:
                case "1":
                    _person = LookupUser();
                    break;
                case "2":
                    _person = CreateNewUser();
                    break;
            }

            List<string> menuOptions = new List<string>
            {
                "1) Search recipes by ingredient(s)",
                "2) Choose favorite recipe(s)",
                "3) See your favorite recipes",
                "4) Quit"
            };

            Console.WriteLine("Hi, {0}. What would you like to do?", _person.Name);
            bool done = false;
            while (!done)
            {
                foreach (string menuOption in menuOptions)
                    Console.WriteLine(menuOption);
                input = Console.ReadLine();
                switch (input)
                {
                    default:
                    case "1":
                        ViewAllRecipes("all");
                        break;
                    case "2":
                        ChooseFavoriteRecipes();
                        break;
                    case "3":
                        ViewFavoriteRecipes();
                        break;
                    case "4":
                        done = true;
                        break;
                }
            }
        }

        public void ViewAllRecipes(string viewType)
        {
            Console.Write("Enter the ingredient you would like to search for: ");
            string ingredientToSearch = Console.ReadLine();
            string url = $"https://api.edamam.com/search?q={ingredientToSearch}&app_id={_apiId}&app_key={_apiKey}";
            WebClient client = new WebClient();
            var contents = client.DownloadString(url);
            var recipes = contents.Split(new string[] { "\"recipe\"" }, StringSplitOptions.None);
            switch(viewType)
            {
                case "all":
                    ViewIndividualRecipe(recipes);
                    break;
                case "favorites":
                    ViewRecipesForFavorites(recipes);
                    break;
                default:
                    break;
            }
        }

        private void ChooseFavoriteRecipes()
        {
            bool doneAdding = false;
            while(!doneAdding)
            {
                Console.WriteLine("Search for recipes to add to your favorites.");
                ViewAllRecipes("favorites");
                Console.Write("Would you like to add another? (Y/N): ");
                var input = Console.ReadLine();
                if (input.ToUpper() == "N")
                    doneAdding = true;
            }
        }

        private void ViewRecipesForFavorites(string[] recipes)
        {
            List<Recipe> recipeTitles = ViewRecipeTitles(recipes);
            int count = 1;
            Console.WriteLine("Which recipe would you like to add to your favorites?");
            foreach(Recipe recipe in recipeTitles)
            {
                Console.WriteLine("{0}) {1}", count, recipe.Name);
                count++;
            }
            int input = int.Parse(Console.ReadLine());
            Recipe recipeToReturn = new Recipe
            {
                Name = recipeTitles[input - 1].Name,
                Ingredients = ViewRecipeIngredients(recipes, input),
                PersonId = _person.Id
            };
            _person.FavoriteRecipes.Add(recipeToReturn);
            using (var context = new RecipeContext())
            {
                context.Recipe.Add(recipeToReturn);
                context.SaveChanges();
            }
        }

        private void ViewIndividualRecipe(string[] recipes)
        {
            List<Recipe> recipeTitles = ViewRecipeTitles(recipes);
            int count = 1;
            Console.WriteLine("Which recipe would you like to view?");
            foreach(Recipe recipe in recipeTitles)
            {
                Console.WriteLine("{0}) {1}", count, recipe.Name);
                count++;
            }
            int input = int.Parse(Console.ReadLine());
            ViewRecipeIngredients(recipes, input);
        }

        private List<Ingredient> ViewRecipeIngredients(string[] recipes, int index)
        {
            List<Ingredient> ingredientList = new List<Ingredient>();
            var recipeIngredients = GetBetween(recipes[index], "\"ingredientLines\"", "\"ingredients\"").Trim(new Char[] { ' ', ':', '"', '[', ']' });
            var ingredients = recipeIngredients.Split(',');
            foreach (var ingredient in ingredients)
            {
                ingredientList.Add(new Ingredient { Name = ingredient.Trim(new Char[] { '"', ']', ' ' }) });
            }
            DrawIngredientsTable(ingredientList);
            return ingredientList;
        }

        private void ViewFavoriteRecipes()
        {
            int count = 1;
            foreach (var recipe in _person.FavoriteRecipes)
            {
                Console.WriteLine("{0}) {1}", count, recipe.Name);
                count++;
            }
        }

        private List<Recipe> ViewRecipeTitles(string[] recipes)
        {
            List<Recipe> recipesToShow = new List<Recipe>();
            foreach (var recipe in recipes)
            {
                var recipeTitle = GetBetween(recipe, "\"label\"", "\"image\"").Trim(new Char[] { ' ', ':', '"' });
                if (recipeTitle.Length > 0)
                {
                    recipeTitle = recipeTitle.Substring(0, recipeTitle.Length - 3);
                    recipesToShow.Add(new Recipe { Name = recipeTitle });
                }
            }
            return recipesToShow;
        }

        private void DrawIngredientsTable(List<Ingredient> ingredients)
        {
            int count = 1;
            foreach (var ingredient in ingredients)
            {
                if (!string.IsNullOrWhiteSpace(ingredient.Name))
                {
                    Console.WriteLine("Ingredient {0} | {1}", count, ingredient.Name);
                    count++;
                }
            }
        }

        private static string GetBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        private Person CreateNewUser()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            var person = new Person(name);
            using (var context = new RecipeContext())
            {
                context.Person.Add(person);
                context.SaveChanges();
            }
            return new Person(name);
        }

        private Person LookupUser()
        {
            Person personToReturn;
            using(var context = new RecipeContext())
            {
                var people = context.Person.SqlQuery("SELECT * FROM dbo.People").ToList();
                Console.WriteLine("Select your name from the list:");
                int count = 1;
                foreach (var person in people)
                {
                    Console.WriteLine("{0}) {1}", count, person.Name);
                    count++;
                }
                int input = int.Parse(Console.ReadLine()) - 1;
                personToReturn = new Person(people[input].Name)
                {
                    FavoriteRecipes = people[input].FavoriteRecipes,
                    Id = people[input].Id
                };
            }
            return personToReturn;
        }
    }
}
