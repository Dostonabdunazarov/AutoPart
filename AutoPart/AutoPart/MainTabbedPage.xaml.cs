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
        public List<Label[,]> LabelList { get; set; }
        public List<int> RowCounts { get; set; }
        public List<string> FileNamesList { get; set; }
        public List<Entry> Entries { get; set; }
        public List<int> NameCol { get; set; }
        public List<int> PriceCol { get; set; }
        public List<List<AutoItem>> AutoItems { get; set; }
        private ListView ListView;

        public MainTabbedPage(List<List<AutoItem>> autoItems, List<int> rowCounts, List<int> nameCol, List<int> priceCol, List<string> fileNames)
        {
            InitializeComponent();
            //LabelList = labels;
            AutoItems = autoItems;
            RowCounts = rowCounts;
            NameCol = nameCol;
            PriceCol = priceCol;
            FileNamesList = fileNames;
            Entries = new List<Entry>();
            ListView = new ListView();
            CreateComponents();
        }
        void GetResultSearch(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            int currentItem = 0;
            for(int i =0; i < Entries.Count; i++)
            {
                if(e.NewTextValue == Entries[i].Text)
                {
                    entry = Entries[i];
                    currentItem = i;
                }
            }
            var list = AutoItems[currentItem].Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            ListView listView = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = list,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label Name = new Label();
                    Name.SetBinding(Label.TextProperty, "Name");
                    Label priceLabel = new Label() { TextColor = Color.Teal };
                    priceLabel.SetBinding(Label.TextProperty, "Price");
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { Name, priceLabel }
                        }
                    };
                })
            };
            
        }
        void CreateComponents()
        {
            for (int i = 0; i < FileNamesList.Count; i++)
            {
                /////
                var contentPage = new ContentPage() { Title = FileNamesList[i] };
                var stackLayout = new StackLayout();
                //var listView = new ListView();

                ListView = new ListView
                {
                    HasUnevenRows = true,
                    // Определяем источник данных
                    ItemsSource = AutoItems[i],

                    // Определяем формат отображения данных
                    ItemTemplate = new DataTemplate(() =>
                    {
                        // привязка к свойству Name
                        Label Name = new Label();
                        Name.SetBinding(Label.TextProperty, "Name");

                        // привязка к свойству Price
                        Label priceLabel = new Label() { TextColor = Color.Teal };
                        priceLabel.SetBinding(Label.TextProperty, "Price");

                        return new ViewCell
                        {
                            View = new StackLayout
                            {
                                Padding = new Thickness(10, 0, 10, 0),
                                Orientation = StackOrientation.Vertical,
                                Children = {Name, priceLabel }
                            }
                        };
                    })
                };

                var entrySearch = new Entry() { Placeholder = "Search" };
                entrySearch.TextChanged += GetResultSearch;
                Entries.Add(entrySearch);
                
                
                stackLayout.Children.Add(ListView);
                stackLayout.Children.Add(entrySearch);
                contentPage.Content = stackLayout;
                
                MyTabbedPage.Children.Add(contentPage);
            }
        }
    }
}