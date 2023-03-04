using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Syncfusion.XlsIO;
using Xamarin.Essentials;
using System.IO;
using AutoPart.Model;

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
                    //FileNames.Add(file.FileName);
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
                for(int s = 0; s < workbook.Worksheets.Count; s++)
                {
                    IWorksheet worksheet = workbook.Worksheets[s];
                    int rowCount = worksheet.Rows.Length;
                    int colCount = worksheet.Columns.Length;
                    FileNames.Add(workbook.Worksheets[s].Name);
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
                }
                fileStream.Close();
                workbook.Close();
                excelEngine.Dispose();
            }

            _ = Navigation.PushAsync(new MainTabbedPage(LabelsList, RowsCountList, nameCol, priceCol, FileNames));
        }
    }
}
