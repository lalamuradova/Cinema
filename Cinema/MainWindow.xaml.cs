using Cinema.Model;
using Cinema.Pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Cinema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string Genre { get; set; }
        public string Time { get; set; }
        public string Rating { get; set; }
        public dynamic Data { get; set; }
        public dynamic SingleData { get; set; }
        public double Price { set; get; } = 5.50;
        public double TotalPrice { get; set; } = 0;
       public List<Seat> seatspdf { get; set; } = new List<Seat>();

        HttpClient httpclient = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        public int Index { get; set; } = 0;
        public string Genre2 { get; set; }
        public string Time2 { get; set; }
        public int Minute { get; set; }
        public int Hour { get; set; }
        public void GetMovie()
        {
            var name = SearchTxtBox.Text;
            HttpResponseMessage response = new HttpResponseMessage();
            response = httpclient.GetAsync($@"http://www.omdbapi.com/?apikey=4e6e4efc&s={name}&plot=full").Result;
            var str = response.Content.ReadAsStringAsync().Result;
            Data = JsonConvert.DeserializeObject(str);
            response = httpclient.GetAsync($@"http://www.omdbapi.com/?apikey=4e6e4efc&t={Data.Search[Index].Title}&plot=full").Result;

            str = response.Content.ReadAsStringAsync().Result;
            SingleData = JsonConvert.DeserializeObject(str);





            if (Index == 0)
            {
                image2.Source = SingleData.Poster;
                Genre = SingleData.Genre;
                Genre2 = " ";
                foreach (var item in Genre)
                {
                    if (item == ',')
                    {
                        Genre2 += " |";
                    }
                    else
                    {
                        Genre2 += item;
                    }
                }

                GenreTxtBlock.Text = Genre2;
                YearTxtBlock.Text = SingleData.Released;
                Time = SingleData.Runtime;
                Time2 = "";
                foreach (var item in Time)
                {
                    if (item != ' ' && item != 'm' && item != 'i' && item != 'n')
                    {
                        Time2 += item;
                    }

                }
                Minute = int.Parse(Time2);
                Hour = Minute / 60;
                Minute -= Hour * 60;

                timeTxtBlock.Text = Hour.ToString() + "h " + Minute.ToString() + "min";
                RatingTxtBlock.Text = SingleData.imdbRating + "/10";
                TitleTextBlock.Text = SingleData.Title;
                DTxtBlock.Text = FilmTypeCombobox.Text;

                DirectorTxtBlock.Text = SingleData.Director;
                WriterTxtBlock.Text = SingleData.Writer;
            }
            else
            {
                BackroundImage.Source = SingleData.Poster;
                --Index;
            }


        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            var border = sender as Border;

            if ((border.Background as SolidColorBrush) == Brushes.Red)
            {
                MessageBox.Show("The seat is full");            

            }
            else
            {
                TotalPrice += Price;
                TotalTxtBlock.Text = TotalPrice.ToString();
                border.Background = Brushes.Red;

                var textBlock = border.Child as TextBlock;
                Seat seat = new Seat(border.Background.ToString(), textBlock.Text);
                seatspdf.Add(seat);
            }



        }

        public void ReadJson()
        {
            SearchTxtBox.Text = SearchTxtBox.Text.ToLower();
            if (File.Exists(SearchTxtBox.Text + ".json"))
            {
                var movie = FileHelper.FileHelper.JsonDeserializeMovie(SearchTxtBox.Text);
                List<Seat> seats = new List<Seat>();
                int index = 0;
                foreach (var item in panelA.Children)
                {
                    var border = item as Border;

                    border.Background = ChangeColor(movie.Seats[index].Color);

                    index++;
                }
                foreach (var item in panelB.Children)
                {
                    var border = item as Border;

                    border.Background = ChangeColor(movie.Seats[index].Color);


                    index++;
                }
                foreach (var item in panelC.Children)
                {
                    var border = item as Border;

                    border.Background = ChangeColor(movie.Seats[index].Color);


                    index++;
                }
                TimeCombobox.Text = movie.Time;
                DateCombobox.Text = movie.Date;
                MallCombobox.Text = movie.Location;
            }
            else
            {
                foreach (var item in panelA.Children)
                {
                    var border = item as Border;

                    border.Background = Brushes.LightGray;
                }
                foreach (var item in panelB.Children)
                {
                    var border = item as Border;

                    border.Background = Brushes.LightGray;
                }
                foreach (var item in panelC.Children)
                {
                    var border = item as Border;

                    border.Background = Brushes.LightGray;
                }

            }
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GetMovie();
            ++Index;
            GetMovie();
            ReadJson();
        }


        public SolidColorBrush ChangeColor(string color)
        {

            // in WPF you can use a BrushConverter
            SolidColorBrush redBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
            return redBrush;
        }

        

        public void WriteJson()
        {
            List<Seat> seats = new List<Seat>();

            foreach (var item in panelA.Children)
            {
                var border = item as Border;
                var textBlock = border.Child as TextBlock;
                Seat seat = new Seat(border.Background.ToString(), textBlock.Text);
                seats.Add(seat);
            }
            foreach (var item in panelB.Children)
            {
                var border = item as Border;
                var textBlock = border.Child as TextBlock;
                Seat seat = new Seat(border.Background.ToString(), textBlock.Text);
                seats.Add(seat);
            }
            foreach (var item in panelC.Children)
            {
                var border = item as Border;
                var textBlock = border.Child as TextBlock;
                Seat seat = new Seat(border.Background.ToString(), textBlock.Text);
                seats.Add(seat);
            }

            Movie movie = new Movie
            {
                MovieName = SearchTxtBox.Text,
                Time = TimeCombobox.Text,
                Date= DateCombobox.Text,
                Location= MallCombobox.Text,
                Seats = seats
            };
            FileHelper.FileHelper.JsonSerializationMovie(movie);

            TotalTxtBlock.Text = TotalPrice.ToString() + " $";

            string filename = SearchTxtBox.Text + Index.ToString()+".pdf";
            PDF.CreatePDF(movie, filename, TotalPrice.ToString(),seatspdf);
            seatspdf = null;
        }
        private void CheckHoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Enjoy watching");            
            WriteJson();
            TotalPrice = 0;
        }
    }

}