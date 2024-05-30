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
            ArgumentNullException.ThrowIfNull(file, nameof(file));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var model = new ImportAndValidateModel<T>();
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                memoryStream.Position = 0;

                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets[0];
                    int rows = worksheet.Dimension.Rows;
                    bool hasErrors = false;
                    var properties = typeof(T).GetProperties()
                                              .Where(p => p.GetCustomAttribute<EpplusTableColumnAttribute>() != null)
                                              .ToList();
                    var columnIndices = properties.ToDictionary(p => p.Name, p => GetColumnIndexFromAttribute(p, worksheet));

                    for (int row = 2; row <= rows; row++)
                    {
                        var data = Activator.CreateInstance<T>();
                        var errors = new List<ValidateExcelModel>();
                        foreach (var property in properties)
                        {
                            int? columnIndex = columnIndices[property.Name];
                            if (columnIndex.HasValue)
                            {
                                var cellValue = worksheet.Cells[row, columnIndex.Value].Value;
                                object value;

                                if (property.PropertyType == typeof(TimeSpan) && cellValue != null)
                                {
                                    if (TimeSpan.TryParse(cellValue.ToString(), out TimeSpan timeValue))
                                    {
                                        value = timeValue;
                                    }
                                    else
                                    {
                                        value = TimeSpan.Zero;
                                        errors.Add(new ValidateExcelModel { RowIndex = row, ColumnName = property.Name, Message = "Invalid time format" });
                                    }
                                }
                                else
                                {
                                    value = Convert.ChangeType(cellValue, property.PropertyType);
                                }

                                property.SetValue(data, value);
                            }
                        }

                        model.Datas.Add(data);
                        if (validateDataFunc != null && !Task.Run(() => validateDataFunc(data, model.Datas, row, errors)).Result)
                        {
                            hasErrors = true;
                            foreach (var error in errors)
                            {
                                if (columnIndices.TryGetValue(error.ColumnName ?? string.Empty, out int? colIdx) && colIdx.HasValue)
                                {
                                    worksheet.Cells[error.RowIndex, colIdx.Value].AddComment(error.Message, "Validation");
                                    worksheet.Cells[error.RowIndex, colIdx.Value].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[error.RowIndex, colIdx.Value].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                }
                            }
                        }
                    }

                    if (hasErrors)
                    {
                        using (var errorStream = new MemoryStream(excelPackage.GetAsByteArray()))
                        {
                            model.Stream = new MemoryStream(errorStream.ToArray());
                        }
                    }
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
