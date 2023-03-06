using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Syncfusion.XlsIO;
using Xamarin.Essentials;
using System.IO;
using AutoPart.Model;
using System.Linq;
using Xamarin.Forms.PlatformConfiguration;

namespace AutoPart
{
    public partial class MainPage : ContentPage
    {
        public List<AutoItem> AutoItems { get; set; }
        public MainPage()
        {
            InitializeComponent();
            var FilePaths = new List<string>();
            FilePaths.Add("/sdcard/download/Files/5.xlsx");
            //string path1 = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);

            AutoItems = new List<AutoItem>();

            var nameCol = new List<int>();
            var priceCol = new List<int>();

            for (int k = 0; k < FilePaths.Count; k++)
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                FileStream fileStream = new FileStream(FilePaths[k], FileMode.Open);
                IWorkbook workbook = application.Workbooks.Open(fileStream);
                for (int s = 0; s < workbook.Worksheets.Count; s++)
                {
                    IWorksheet worksheet = workbook.Worksheets[s];
                    int rowCount = worksheet.Rows.Length;
                    int colCount = worksheet.Columns.Length;
                    for (int i = 0; i < rowCount; i++)
                    {
                        for (int j = 0; j < colCount; j++)
                        {
                            if (worksheet.GetValueRowCol(i, j).ToString() == "#")
                            {
                                nameCol.Add(j);
                            }
                            if (worksheet.GetValueRowCol(i, j).ToString() == "$")
                            {
                                priceCol.Add(j);
                            }
                        }
                    }

                    for (int i = 1; i <= rowCount; i++)
                    {
                        if (!String.IsNullOrEmpty(worksheet.GetValueRowCol(i, nameCol[k])?.ToString()) && !String.IsNullOrEmpty(worksheet.GetValueRowCol(i, priceCol[k])?.ToString()))
                        {
                            var autoItem = new AutoItem() { Id = i, Name = worksheet.GetValueRowCol(i, nameCol[k]).ToString(), Price = worksheet.GetValueRowCol(i, priceCol[k]).ToString() };
                            AutoItems.Add(autoItem);
                        }
                    }
                }
                fileStream.Close();
                workbook.Close();
                excelEngine.Dispose();
            }
            listview.ItemsSource = AutoItems;
            //_ = Navigation.PushAsync(new MainTabbedPage(AutoItems, FilePaths.Count));
        }
        void GetResultSearch(object sender, TextChangedEventArgs e)
        {
            var list = AutoItems.Where(x => x.Name.ToLower().Contains(entrySearch.Text.ToLower())).ToList();

            listview.ItemsSource = list;
        }
        //async void PickExcelFilesClicked(object sender, EventArgs e)
        //{
            //var allowedUTIs = new string[] {
            //    "org.openxmlformats.wordprocessingml.document",
            //    "*/*",
            //};
            //var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            //{
            //    { DevicePlatform.Android, allowedUTIs },
            //    { DevicePlatform.iOS, allowedUTIs },
            //});

            //var Files = await FilePicker.PickMultipleAsync(new PickOptions
            //{
            //    FileTypes = customFileType,
            //    PickerTitle = "Pick an excel"
            //});
            //var FilePaths = new List<string>();
            //FilePaths.Add("/sdcard/download/Files/5.xlsx");
            
            //var FileNames = new List<string>();
            //if (Files != null)
            //{
            //    foreach (var file in Files)
            //    {
            //        FilePaths.Add(file.FullPath);
            //        //FileNames.Add(file.FileName);
            //    }
            //}
            //var AutoPartList = new List<AutoItem>();

            //var RowsCountList = new List<int>();
            //var nameCol = new List<int>();
            //var priceCol = new List<int>();

            //for (int k = 0; k < FilePaths.Count; k++)
            //{
            //    ExcelEngine excelEngine = new ExcelEngine();
            //    IApplication application = excelEngine.Excel;
            //    application.DefaultVersion = ExcelVersion.Xlsx;
            //    FileStream fileStream = new FileStream(FilePaths[k], FileMode.Open);
            //    IWorkbook workbook = application.Workbooks.Open(fileStream);
            //    for(int s = 0; s < workbook.Worksheets.Count; s++)
            //    {
            //        IWorksheet worksheet = workbook.Worksheets[s];
            //        int rowCount = worksheet.Rows.Length;
            //        int colCount = worksheet.Columns.Length;
            //        FileNames.Add(workbook.Worksheets[s].Name);
            //        for (int i = 0; i < rowCount; i++)
            //        {
            //            for (int j = 0; j < colCount; j++)
            //            {
            //                if (worksheet.GetValueRowCol(i, j).ToString() == "#")
            //                {
            //                    nameCol.Add(j);
            //                }
            //                if (worksheet.GetValueRowCol(i, j).ToString() == "$")
            //                {
            //                    priceCol.Add(j);
            //                }
            //            }
            //        }

            //        RowsCountList.Add(rowCount);
            //        for (int i = 1; i <= rowCount; i++)
            //        {
            //            if (!String.IsNullOrEmpty(worksheet.GetValueRowCol(i, nameCol[k])?.ToString()) && !String.IsNullOrEmpty(worksheet.GetValueRowCol(i, priceCol[k])?.ToString()))
            //            {
            //                var autoItem = new AutoItem() { Id = i, Name = worksheet.GetValueRowCol(i, nameCol[k]).ToString(), Price = worksheet.GetValueRowCol(i, priceCol[k]).ToString() };
            //                AutoPartList.Add(autoItem);
            //            }
            //        }
            //        //LabelsList.Add(labels);
            //    }
            //    fileStream.Close();
            //    workbook.Close();
            //    excelEngine.Dispose();
            //}

            //_ = Navigation.PushAsync(new MainTabbedPage(AutoPartList, FilePaths.Count));
        //}
    }
}
