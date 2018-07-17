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
                "2) See your favorite recipes",
                "3) Quit"
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
                        ViewAllRecipes();
                        break;
                    case "2":
                        ViewFavoriteRecipes();
                        break;
                    case "3":
                        done = true;
                        break;
                }
            }
        }

        public void ViewAllRecipes()
        {
            List<Recipe> recipesToShow = new List<Recipe>();
            Console.Write("Enter the ingredient you would like to search for: ");
            string ingredientToSearch = Console.ReadLine();
            string url = $"https://api.edamam.com/search?q={ingredientToSearch}&app_id={_apiId}&app_key={_apiKey}";
            WebClient client = new WebClient();
            var contents = client.DownloadString(url);
            var recipes = contents.Split(new string[] { "\"recipe\"" }, StringSplitOptions.None);
            ViewIndividualRecipe(recipes);
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

        private void ViewRecipeIngredients(string[] recipes, int index)
        {
            List<Ingredient> ingredientList = new List<Ingredient>();
            var recipeIngredients = GetBetween(recipes[index], "\"ingredientLines\"", "\"ingredients\"").Trim(new Char[] { ' ', ':', '"', '[', ']' });
            var ingredients = recipeIngredients.Split(',');
            foreach (var ingredient in ingredients)
            {
                ingredientList.Add(new Ingredient { Name = ingredient.Trim(new Char[] { '"', ']', ' ' }) });
            }
            DrawTopLine(ingredientList);
            foreach(var ingredient in ingredientList)
                Console.Write("{0} | ", ingredient.Name);
        }

        private void ViewFavoriteRecipes()
        {

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

        private void DrawTopLine(List<Ingredient> ingredients)
        {
            string drawLength = "";
            for(int i = 1; i <= ingredients.Count; i++)
            {
                drawLength += $"Ingredient {i} | ";
            }
            drawLength = drawLength.Trim();
            Console.WriteLine(drawLength);
            Console.WriteLine(new string('-', drawLength.Length));            
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
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            using(var context = new RecipeContext())
            {
                personToReturn = (Person) context.Person.Where(p => p.Name == name);
            }
            return personToReturn;
        }
    }
}
