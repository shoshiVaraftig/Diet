using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietWeb.Core.Models
{
    public class WeightTracing
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float Weight { get; set; }
        public DateTime Date { get; set; }
    }

}
