using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            Console.Write("Enter the ingredient you would like to search for: ");
            string ingredientToSearch = Console.ReadLine();
            string url = $"https://api.edamam.com/search?q={ingredientToSearch}&app_id={_apiId}&app_key={_apiKey}";
            WebClient client = new WebClient();
            var contents = client.DownloadString(url);
            var recipes = contents.Split(new string[] { "Recipe" }, StringSplitOptions.None);
            foreach (var recipe in recipes)
            {
                var recipeTitle = recipe.Split(new string[] { "Label" }, StringSplitOptions.None);
                foreach (var recipeName in recipeTitle)
                {
                    Console.WriteLine(recipeName);
                    Console.WriteLine("--------------------------");
                }
            }
        }

        private void ViewFavoriteRecipes()
        {

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
