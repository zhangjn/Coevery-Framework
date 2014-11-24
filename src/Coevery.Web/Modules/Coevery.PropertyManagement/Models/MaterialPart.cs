
using Coevery.ContentManagement;

namespace Coevery.PropertyManagement.Models
{
    public sealed class MaterialPart : ContentPart<MaterialPartRecord>
    {
        public string SerialNo
        {
            get { return Record.SerialNo; }
            set { Record.SerialNo = value; }
        }

        public string Name
        {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public string Brand
        {
            get { return Record.Brand; }
            set { Record.Brand = value; }
        }

        public string Model
        {
            get { return Record.Model; }
            set { Record.Model = value; }
        }

        public double? CostPrice
        {
            get { return Record.CostPrice; }
            set { Record.CostPrice = value; }
        }

    
        public string Remark
        {
            get { return Record.Remark; }
            set { Record.Remark = value; }
        }

        public string Unit
        {
            get { return Record.Unit; }
            set { Record.Unit = value; }
        }

    }
}