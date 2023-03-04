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

        public MainTabbedPage(List<Label[,]> labels, List<int> rowCounts, List<int> nameCol, List<int> priceCol, List<string> fileNames)
        {
            InitializeComponent();
            LabelList = labels;
            RowCounts = rowCounts;
            NameCol = nameCol;
            PriceCol = priceCol;
            FileNamesList = fileNames;
            Entries = new List<Entry>();
            CreateComponents();
        }
        void GetResultSearch(object sender, TextChangedEventArgs e)
        {
            //(Entry)sender
            var a = MyTabbedPage.CurrentPage;
            var c = new ContentPage();
            
        }
        void CreateComponents()
        {
            for (int i = 0; i < FileNamesList.Count; i++)
            {
                var contentPage = new ContentPage() { Title = FileNamesList[i] };
                var stackLayout = new StackLayout();
                var scrollView = new ScrollView() { HorizontalScrollBarVisibility = ScrollBarVisibility.Always, VerticalOptions = LayoutOptions.FillAndExpand };
                var entrySearch = new Entry() { Placeholder = "Search" };
                entrySearch.TextChanged += GetResultSearch;
                Grid GridContent = new Grid();
                //
                Entries.Add(entrySearch);
                GridContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                GridContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                GridContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Absolute) });
                GridContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Auto) });
                for (int k = 0; k < RowCounts[i]; k++)
                {
                    GridContent.Children.Add(new Label { Text = (k + 1).ToString() }, 0, k);
                    if (LabelList[i][k, NameCol[0]] != null && NameCol[0] != 0)
                    {
                        GridContent.Children.Add(LabelList[i][k, NameCol[0]], NameCol[0], k);
                    }
                    if (LabelList[i][k, PriceCol[0]] != null && PriceCol[0] != 0)
                    {
                        GridContent.Children.Add(LabelList[i][k, PriceCol[0]], PriceCol[0], k);
                    }
                }
                scrollView.Content = GridContent;
                stackLayout.Children.Add(scrollView);
                stackLayout.Children.Add(entrySearch);
                contentPage.Content = stackLayout;
                MyTabbedPage.Children.Add(contentPage);
            }
        }
    }
}