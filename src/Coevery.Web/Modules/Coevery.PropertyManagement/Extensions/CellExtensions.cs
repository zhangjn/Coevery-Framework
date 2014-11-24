using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Coevery.PropertyManagement.Extensions
{
    public static class CellExtensions
    {
        public static void SetCellValue(this ICell cell, object value)
        {
            if (value == null) return;
            if (value is bool)
            {
                cell.SetCellValue((bool)value);
            }
            else if (value is double)
            {
                cell.SetCellValue((double)value);
                cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            }
            else if (value is decimal)
            {
                cell.SetCellValue(Convert.ToDouble(value));
                cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            }
            else if (value is DateTime) {
                cell.SetCellValue(((DateTime) value).ToString("yyyy-MM-dd"));
            }
            else
            {
                var str = value as string;
                if (str != null)
                {
                    cell.SetCellValue(str);
                }
                else
                {
                    cell.SetCellValue(value.ToString());
                }
            }
        }

        public static T GetCellValue<T>(this IRow row, int cellNum) {
            var cell = row.GetCell(cellNum, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            object cellValue = null;
            switch (cell.CellType) {
                case CellType.String:
                    cellValue = cell.StringCellValue;
                    break;
                case CellType.Boolean:
                    cellValue = cell.BooleanCellValue;
                    break;
                case CellType.Formula:
                    cellValue = cell.CellFormula;
                    break;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell)) {
                        cellValue = cell.DateCellValue;
                    }
                    else {
                        cellValue = cell.NumericCellValue;
                    }

                    break;
            }
            if (cellValue is T) {
                return (T) cellValue;
            }
            try {
                return (T) Convert.ChangeType(cellValue, typeof (T));
            }
            catch (InvalidCastException) {
                return default(T);
            }
        }
    }
}