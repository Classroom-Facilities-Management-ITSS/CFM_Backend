using ClassroomManagerAPI.Models.Excels;
using OfficeOpenXml;
using OfficeOpenXml.Attributes;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Reflection;

namespace ClassroomManagerAPI.Helpers
{
    public static class ExcelHelper
    {
        private static int? GetColumnIndexFromAttribute(PropertyInfo property, ExcelWorksheet worksheet)
        {
            EpplusTableColumnAttribute customAttribute = property.GetCustomAttribute<EpplusTableColumnAttribute>();
            if (customAttribute == null)
            {
                return null;
            }

            string header = customAttribute.Header;
            return worksheet.Cells.FirstOrDefault((ExcelRangeBase cell) => cell.Text == header)?.Start.Column;
        }
        public static ImportAndValidateModel<T> ImportAndValidateExcel<T>(this IFormFile file, Func<T, IList<T>, int, IList<ValidateExcelModel>, Task<bool>>? validateDataFunc)
        {
            Func<T, IList<T>, int, IList<ValidateExcelModel>, Task<bool>> validateDataFunc2 = validateDataFunc;
            ArgumentNullException.ThrowIfNull(file, "file");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ImportAndValidateModel<T> model = new ImportAndValidateModel<T>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                int rows = worksheet.Dimension.Rows;
                bool flag = false;
                List<PropertyInfo> list = (from x in typeof(T).GetProperties()
                                           where x.GetCustomAttribute<EpplusTableColumnAttribute>() != null
                                           select x).ToList();
                Dictionary<string, int?> dictionary = list.ToDictionary((PropertyInfo x) => x.Name, (PropertyInfo x) => GetColumnIndexFromAttribute(x, worksheet));
                for (int row = 2; row <= rows; row++)
                {
                    T data = Activator.CreateInstance<T>();
                    List<ValidateExcelModel> errors = new List<ValidateExcelModel>();
                    foreach (PropertyInfo item in list)
                    {
                        int? num = dictionary[item.Name];
                        if (num.HasValue)
                        {
                            dynamic value = worksheet.Cells[row, num.Value].Value;
                            dynamic val = Convert.ChangeType(value, item.PropertyType);
                            item.SetValue(data, val);
                        }
                    }

                    model.Datas.Add(data);
                    if (validateDataFunc2 == null || Task.Run(() => validateDataFunc2(data, model.Datas, row, errors)).Result)
                    {
                        continue;
                    }

                    flag = true;
                    foreach (ValidateExcelModel item2 in errors)
                    {
                        string columnName = item2.ColumnName;
                        int rowIndex = item2.RowIndex;
                        string message = item2.Message;
                        int? num2 = dictionary[columnName ?? string.Empty];
                        if (num2.HasValue)
                        {
                            worksheet.Cells[rowIndex, num2.Value].AddComment(message);
                            _ = worksheet.Dimension.Columns;
                            worksheet.Cells[rowIndex, num2.Value].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowIndex, num2.Value].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        }
                    }
                }

                if (flag)
                {
                    MemoryStream stream = new MemoryStream(excelPackage.GetAsByteArray());
                    model.Stream = stream;
                }
            }

            return model;
        }

        private static void GenerateExcelStyle(this ExcelWorksheet workSheet, int columnNumber)
        {
            ExcelRange excelRange = workSheet.Cells[1, 1, 1, columnNumber];
            excelRange.Style.Font.Bold = true;
            excelRange.Style.Font.Size = 11f;
            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelRange.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            excelRange.Style.WrapText = true;
            excelRange.Style.Font.Color.SetColor(Color.White);
            excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        public static Stream ExportExcel<T>(this IList<T>? datas)
        {
            MemoryStream memoryStream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                if (datas != null)
                {
                    excelWorksheet.Cells.LoadFromCollection(datas, PrintHeaders: true);
                }

                excelWorksheet.GenerateExcelStyle(excelWorksheet.Dimension.End.Column);
                excelPackage.Save();
            }

            memoryStream.Position = 0L;
            return memoryStream;
        }
    }
}
