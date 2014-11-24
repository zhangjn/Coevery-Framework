using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;
using Coevery.Roles.Models;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Models
{
    public sealed class ServicePart : ContentPart<ServicePartRecord>
    {

      /*  public int HouseId
        {
            get { return Record.HouseId; }
            set { Record.HouseId = value; }
        }

        public int OwnerId
        {
            get { return Record.OwnerId; }
            set { Record.OwnerId = value; }
        }
        */
        public HousePartRecord House
        {
            get { return Record.House; }
            set { Record.House = value; }
        }

        public CustomerPartRecord Owner
        {
            get { return Record.Owner; }
            set { Record.Owner = value; }
        }
        public string Mobile
        {
            get { return Record.Mobile; }
            set { Record.Mobile = value; }
        }

        public string FaultDescription
        {
            get { return Record.FaultDescription; }
            set { Record.FaultDescription = value; }
        }

        public DateTime ReceivedDate
        {
            get { return Record.ReceivedDate; }
            set { Record.ReceivedDate = value; }
        }

       /* public int ServicePersonId
        {
            get { return Record.ServicePersonId; }
            set { Record.ServicePersonId = value; }
        }*/
        public UserPartRecord ServicePerson
        {
            get { return Record.ServicePerson; }
            set { Record.ServicePerson = value; }
        }
        public decimal ServiceCharge
        {
            get { return Record.ServiceCharge; }
            set { Record.ServiceCharge = value; }
        }

        public string ServiceVoucherNo
        {
            get { return Record.ServiceVoucherNo; }
            set { Record.ServiceVoucherNo = value; }
        }

        public string StockOutVoucher
        {
            get { return Record.StockOutVoucher; }
            set { Record.StockOutVoucher = value; }
        }
        public string StockReturnVoucher
        {
            get { return Record.StockReturnVoucher; }
            set { Record.StockReturnVoucher = value; }
        }
    }
}