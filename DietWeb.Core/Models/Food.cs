using System;
using System.Collections.Generic;
using System.Linq;


namespace DietWeb.Core.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Calories { get; set; }
        public string Category { get; set; } = "";
        public string ServingSize { get; set; } = "";

    }

}
