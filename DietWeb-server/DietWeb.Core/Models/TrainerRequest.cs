using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietWeb.Core.Models
{
    public class TrainerRequest
    {
        public string UserId { get; set; } = "";
        public string Personality { get; set; } = "";
        public string Input { get; set; } = "";
    }

}
