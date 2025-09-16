using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietWeb.Core.Models
{
    public class GenerateRecipeRequest
    {

        public string Query { get; set; }

        public bool IsVegetarian { get; set; }

        public bool IsVegan { get; set; }

        public bool IsGlutenFree { get; set; }

        public bool IsDairyFree { get; set; }

    }
}
