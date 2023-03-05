using AutoPart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public List<AutoItem> AutoItems { get; set; }

        public MainTabbedPage(List<AutoItem> autoItems, int fileCount)
        {
            InitializeComponent();
            AutoItems = autoItems;
            listview.ItemsSource = AutoItems;
            myContentPage.Title = $"Files({fileCount})";
            //var listview = new ListView
            //{
            //    HasUnevenRows = true,
            //    ItemsSource = AutoItems,

            //    ItemTemplate = new DataTemplate(() =>
            //    {
            //        Label Name = new Label();
            //        Name.SetBinding(Label.TextProperty, "Name");
            //        Label priceLabel = new Label() { TextColor = Color.Teal };
            //        priceLabel.SetBinding(Label.TextProperty, "Price");

            //        return new ViewCell
            //        {
            //            View = new StackLayout
            //            {
            //                Padding = new Thickness(10, 0, 10, 0),
            //                Orientation = StackOrientation.Vertical,
            //                Children = { Name, priceLabel }
            //            }
            //        };
            //    })
            //};
            //stack.Children.Add(listview);
        }
        void GetResultSearch(object sender, TextChangedEventArgs e)
        {
            var list = AutoItems.Where(x => x.Name.ToLower().Contains(entrySearch.Text.ToLower())).ToList();
            
            listview.ItemsSource = list;
        }
    }
}