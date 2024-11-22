using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualShow_Admin.Model
{
    public class Offres
    {
        public int id { get; set; }
        public string type { get; set; } // This will store the raw string from the API
        public string title { get; set; }
        public string description { get; set; }
        public string date { get; set; }
    }
}
