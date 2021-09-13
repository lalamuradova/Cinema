using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Model
{
    public class Seat
    {
        public Seat(string color, string numbers, double price)
        {
            Color = color;
            Numbers = numbers;           
        }

        public string Color { get; set; }
        public string Numbers { get; set; }       
    }
}