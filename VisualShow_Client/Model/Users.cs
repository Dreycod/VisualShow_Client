using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualShow_Admin.Model
{
    public class Users
    {
        public int iduser { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string date_creation { get; set; }
        public string mdp { get; set; }
    }
}
