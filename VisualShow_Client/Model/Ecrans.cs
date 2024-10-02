using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualShow_Admin.Model
{
    public class Ecrans
    {
        public int idecran { get; set; }
        public string name { get; set; }
        public string lastUpdate { get; set; }
        public int IsOn { get; set; }
        public int salle_idsalle { get; set; }
    }
}
