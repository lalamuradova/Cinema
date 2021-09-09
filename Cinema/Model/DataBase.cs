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
            Price = price;
        }

        public string Color { get; set; }
        public string Numbers { get; set; }
        public double Price { get; set; }
    }
    public class Movie
    {
        public Movie(string movieName, string time, string date, string location, List<Seat> seats)
        {
            MovieName = movieName;
            Time = time;
            Date = date;
            Location = location;
            Seats = seats;
        }

        public string MovieName { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public List<Seat> Seats { get; set; } = new List<Seat>();
        public override string ToString()
        {
            return $"{MovieName} {Date} {Time}  {Location}";
        }

    }
    public class DataBase
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();

    }
}
