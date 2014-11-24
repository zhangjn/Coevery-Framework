using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;

namespace Coevery.PropertyManagement.Models
{
    public sealed class SupplierPart : ContentPart<SupplierPartRecord>
    {

        public string Name
        {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public string Address
        {
            get { return Record.Address; }
            set { Record.Address = value; }
        }

        public string Contactor
        {
            get { return Record.Contactor; }
            set { Record.Contactor = value; }
        }

        public string Tel
        {
            get { return Record.Tel; }
            set { Record.Tel = value; }
        }

        public string MobilePhone
        {
            get { return Record.MobilePhone; }
            set { Record.MobilePhone = value; }
        }

        public string Email
        {
            get { return Record.Email; }
            set { Record.Email = value; }
        }

        public string QQ
        {
            get { return Record.QQ; }
            set { Record.QQ = value; }
        }

        public string Fax
        {
            get { return Record.Fax; }
            set { Record.Fax = value; }
        }

        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }
    }
}