using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Entities
{
    public class Sufler : EntityBase
    {
        public string? Name { get; set; }
        public override string ToString() => base.ToString() + " (Sufler)";
    }
}
