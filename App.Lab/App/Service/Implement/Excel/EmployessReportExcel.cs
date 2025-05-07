﻿using App.Lab.Common.Helper;
using App.Lab.Model;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Reflection;
using static App.Lab.Common.Helper.ExcelHelp;


namespace App.Lab.App.Service.Implement
{
    /// <summary> fill file excel  </summary>
    /// Author: thuanbv
    /// Created: 29/04/2025
    /// Modified: date - user - description
    public static class EmployessReportExcel
    {
        /// <summary> Tạo danh sách tiêu đề cho báo cáo Excel. </summary>
        /// Author: thuanbv
        /// Created: 29/04/2025
        /// Modified: date - user - description
        public static List<string> Title()
        {
            var lstTitle = new List<string>();

            lstTitle.Add("STT");
            lstTitle.Add("Họ và tên");
            lstTitle.Add("Số điện thoại");
            lstTitle.Add("Số giấy phép lái xe");
            lstTitle.Add("Ngày cấp");
            lstTitle.Add("Ngày hết hạn");
            lstTitle.Add("Nơi cấp");
            lstTitle.Add("Loại bằng");
            lstTitle.Add("Ngày cập nhật");

            return lstTitle;
        }
        /// <summary> Cấu hình các hàng tiêu đề cho file Excel </summary>
        /// Author: thuanbv
        /// Created: 29/04/2025
        /// Modified: date - user - description
        public static List<ExportExcelConfigRow> HeaderRows()
        {
            return new List<ExportExcelConfigRow>()
            {
                new ExportExcelConfigRow()
                {
                    ColumnFormat=ExportExcelConfigFormatType.Text,
                    PropertyName="DisplayName"
                },

                new ExportExcelConfigRow()
                {
                    ColumnFormat=ExportExcelConfigFormatType.Text,
                    PropertyName="Mobile"
                },
                new ExportExcelConfigRow()
                {
                    ColumnFormat=ExportExcelConfigFormatType.Text,
                    PropertyName="DriverLicense"
                },

                new ExportExcelConfigRow()
                {
                    ColumnFormat=ExportExcelConfigFormatType.Date,
                    PropertyName="IssueLicenseDate"
                },
                new ExportExcelConfigRow()
                {
                    ColumnFormat=ExportExcelConfigFormatType.Date,
                    PropertyName="ExpireLicenseDate"
                },
                new ExportExcelConfigRow()
                {
                    ColumnFormat=ExportExcelConfigFormatType.Text,
                    PropertyName="IssueLicensePlace"
                },
                new ExportExcelConfigRow()
                {
                    ColumnFormat=ExportExcelConfigFormatType.Text,
                    PropertyName="LicenseTypeName"
                },
                new ExportExcelConfigRow()
                {
                    ColumnFormat=ExportExcelConfigFormatType.DateTime,
                    PropertyName="UpdatedDate",
                    WrapText =true
                },
            };
        }
        /// <summary>  Điền dữ liệu vào file Excel </summary>
        /// <param name="ws">Worksheet Excel.</param>
        /// <param name="title">Tiêu đề báo cáo.</param>
        /// <param name="listFilter">Danh sách bộ lọc.</param>
        /// <param name="startRow">Dòng bắt đầu.</param>
        /// <param name="result">Danh sách dữ liệu.</param>
        /// <param name="rows">Cấu hình các cột.</param>
        /// Author: thuanbv
        /// Created: 29/04/2025
        /// Modified: date - user - description
        public static void FillExcell(ExcelWorksheet ws, string title, List<Lab.Model.SearchOption> listFilter, int startRow, List<HrmEmployees> result = null, List<ExportExcelConfigRow> rows = null)
        {
            int currRowIdx = startRow;
            int currColIdx = 1;
            int currStartData;
            var listTitel = Title();
            var totalCols = listTitel.Count;
            int heightRow = 36;
            try
            {

                //title
                ws.Cells[currRowIdx, currColIdx].Value = title;
                ws.Cells[currRowIdx, 1, 2, totalCols].Merge = true;
                ws.Cells[currRowIdx, 1].Style.Font.Bold = true;
                ws.Cells[currRowIdx, 1].Style.Font.Size = 20;
                ws.Cells[currRowIdx, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                currRowIdx = currRowIdx + 2;
                foreach (var item in listFilter)
                {
                    ws.Cells[currRowIdx, 1].Value = string.Format("{0}: {1}", item.Key, item.Value);
                    ws.Cells[currRowIdx, 1].Style.WrapText = true;
                    ws.Cells[currRowIdx, 1, currRowIdx, totalCols].Merge = true;
                    ws.Cells[currRowIdx, 1].Style.Font.Size = 12;
                    ws.Cells[currRowIdx, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    currRowIdx++;
                }


                //  Cách 1 dòng trống 
                ws.Cells[currRowIdx, 1, currRowIdx, totalCols].Merge = true;
                currRowIdx++;
                foreach (var headerRow in listTitel)
                {
                    ws.Cells[currRowIdx, currColIdx].Value = headerRow;
                    ws.Cells[currRowIdx, currColIdx].Style.Font.Bold = true;
                    ws.Cells[currRowIdx, currColIdx].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[currRowIdx, currColIdx].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    currColIdx++;
                }
                ExcelHelp.ConvertBorderExcel(ws, currRowIdx, 1, currRowIdx, currColIdx - 1);
                ws.Cells[currRowIdx, 1, currRowIdx, totalCols].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[currRowIdx, 1, currRowIdx, totalCols].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#84AEE0"));
                ws.Cells[currRowIdx, 1, currRowIdx, totalCols].Style.Font.Size = 12;
                Type type = typeof(HrmEmployees);
                currRowIdx++;
                currStartData = currRowIdx;
                int index = 0;
                foreach (var item in result)
                {
                    // giá trị stt
                    index++;
                    ws.Cells[currRowIdx, 1].Value = index;
                    ws.Cells[currRowIdx, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[currRowIdx, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Row(currRowIdx).Height = heightRow;
                    currColIdx = 2;
                    foreach (var column in rows)
                    {
                        PropertyInfo info = type.GetProperty(column.PropertyName) as PropertyInfo;
                        var value = info.GetValue(item);

                        // Kiểm tra nếu cột là kiểu DateTime 
                        if (column.ColumnFormat == ExportExcelConfigFormatType.DateTime && value is DateTime dateTimeValue)
                        {
                            // Kiểm tra nếu cột là kiểu DateTime

                            ws.Cells[currRowIdx, currColIdx].Value = $"{dateTimeValue:HH:mm}\n{dateTimeValue:dd/MM/yyyy}";
                            // Bật WrapText để xuống dòng
                            ws.Cells[currRowIdx, currColIdx].Style.WrapText = true;
                            ExcelHelp.SetFormat(ws.Cells[currRowIdx, currColIdx], column.ColumnFormat);
                        }
                        else
                        {
                            // Các cột khác giữ nguyên
                            ws.Cells[currRowIdx, currColIdx].Value = value;
                            ws.Cells[currRowIdx, currColIdx].Style.WrapText = column.WrapText.HasValue ? column.WrapText.Value : false;
                            ExcelHelp.SetFormat(ws.Cells[currRowIdx, currColIdx], column.ColumnFormat);
                        }

                        currColIdx++;
                    }
                    currRowIdx++;
                }
                currRowIdx++;

                //borders 
                ConvertBorderExcel(ws, currRowIdx - result.Count - 1, 1, currRowIdx - 2, totalCols);

                ws.Cells[currStartData, 3, currRowIdx, totalCols].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[currStartData, 1, currRowIdx, totalCols].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //autofit columns 
                ws.Cells[1, 1, currRowIdx, totalCols].AutoFitColumns();
                ws.Cells[1, 1, currRowIdx - 1, totalCols].Style.WrapText = true;

                //font
                ws.Cells[1, 1, currRowIdx - 1, totalCols].Style.Font.Name = "Times New Roman";
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi trong quá trình tạo file!");
            }


        }
    }
}
