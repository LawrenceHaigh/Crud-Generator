using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class ColumnModel
    {
        public string? ColumnName { get; set; }
        public string? DataType { get; set; }
        public string? MaxLength { get; set; }
        public string? NumericPrecision { get; set; }
        public string? PercisionFloatPoint { get; set; }
        public string? Nullable { get; set; }
        public string? OrdinalPosition { get; set; }
        public string? Default { get; set; }
    }
}
