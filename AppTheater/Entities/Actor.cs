using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Entities
{
    
    public class Actor : EntityBase 
    {
        
        public string? Name { get; set; }    
        //public decimal Payments { get; set; } 

        public override string ToString() => $"Numer Id: {Id}, Imię i Nazwisko: {Name}";


    }

    
}
