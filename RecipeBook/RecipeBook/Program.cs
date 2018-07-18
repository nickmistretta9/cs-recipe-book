using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RecipeBookInfo recipeBook = new RecipeBookInfo();
                recipeBook.Run();
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
