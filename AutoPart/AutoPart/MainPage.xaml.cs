using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Syncfusion.XlsIO;
using Xamarin.Essentials;
using System.IO;

namespace AutoPart
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async void PickExcelFilesClicked(object sender, EventArgs e)
        {
            var allowedUTIs = new string[] {
                "com.microsoft.excel.xls",
                "com.microsoft.excel.xlsx",
                "org.openxmlformats.wordprocessingml.document",
                "*/*",
                "*",
                ".*",
                ".xlsx",
                "*.*"
            };
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, allowedUTIs },
                { DevicePlatform.iOS, allowedUTIs },
            });

            var Files = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = customFileType,
                PickerTitle = "Pick an excel"
            });
            var FilePaths = new List<string>();
            var FileNames = new List<string>();
            if (Files != null)
            {
                foreach (var file in Files)
                {
                    FilePaths.Add(file.FullPath);
                    FileNames.Add(file.FileName);
                }
            }
            
            var LabelsList = new List<Label[,]>();
            var RowsCountList = new List<int>();
            var nameCol = new List<int>();
            var priceCol = new List<int>();
            for (int k = 0; k < FilePaths.Count; k++)
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                FileStream fileStream = new FileStream(FilePaths[k], FileMode.Open);
                IWorkbook workbook = application.Workbooks.Open(fileStream);
                IWorksheet worksheet = workbook.Worksheets["Лист1"];
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

                RowsCountList.Add(rowCount);
                var labels = new Label[rowCount, colCount];
                for (int i = 1; i <= rowCount; i++)
                {
                    if (!String.IsNullOrEmpty(worksheet.GetValueRowCol(i, nameCol[k])?.ToString()) && !String.IsNullOrEmpty(worksheet.GetValueRowCol(i, priceCol[k])?.ToString()))
                    {
                        labels[i - 1, nameCol[k]] = new Label { Text = worksheet.GetValueRowCol(i, nameCol[k]).ToString(), FontFamily = "Arial", };
                        labels[i - 1, priceCol[k]] = new Label { Text = worksheet.GetValueRowCol(i, priceCol[k]).ToString(), FontFamily = "Arial", };
                    }
                }
                LabelsList.Add(labels);
                fileStream.Close();
                workbook.Close();
                excelEngine.Dispose();
            }
            
            _ = Navigation.PushAsync(new MainTabbedPage(LabelsList, RowsCountList, nameCol, priceCol, FileNames));
        }
    }
}


//int nameCol = 0;
//int priceCol = 0;
//int rowCount = 0;
//int colCount = 0;
//foreach (var file in FilePaths)
//{
//    fileStream = new FileStream(file, FileMode.Open);
//    IWorkbook workbook = application.Workbooks.Open(fileStream);
//    IWorksheet worksheet = workbook.Worksheets["Лист1"];
//    rowCount = worksheet.Rows.Length;
//    colCount = worksheet.Columns.Length;

//    for (int i = 0; i < rowCount; i++)
//    {
//        for (int j = 0; j < colCount; j++)
//        {
//            if (worksheet.GetValueRowCol(i, j).ToString() == "#")
//            {
//                nameCol = j;
//            }
//            else if (worksheet.GetValueRowCol(i, j).ToString() == "$")
//            {
//                priceCol = j;
//                break;
//            }
//        }
//    }

//    for (int i = 1; i <= rowCount; i++)
//    {
//        for (int j = 1; j <= colCount; j++)
//        {

//            if (!String.IsNullOrEmpty(worksheet.GetValueRowCol(i, j).ToString()) && rowCount != 0 && nameCol != 0)
//            {
//                var label = new Label()
//                {
//                    Text = worksheet.GetValueRowCol(i, nameCol).ToString() + " - " + worksheet.GetValueRowCol(i, priceCol).ToString(),
//                };
//                Labels.Add(label);
//            }
//                //Names.Add(worksheet.GetValueRowCol(i, nameCol).ToString() + " - " + worksheet.GetValueRowCol(i, priceCol).ToString());
//        }
//    }
//}


//_ = Navigation.PushAsync(new MainTabbedPage(LabelsList, RowsCountList, ColumnCountList, FileNames));
