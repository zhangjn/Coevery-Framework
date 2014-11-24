using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.FileSystems.AppData;

namespace Coevery.PropertyManagement.Services
{
    public interface IVoucherNumberService : IDependency
    {
        void ResetNumber();
        string GenerateNumber();
        string DispayGenerateNumber();
    }

    public class VoucherNumberService : IVoucherNumberService
    {
        private readonly IAppDataFolder _appDataFolder;
        private const string VoucherNoFileName = "VoucherNo.txt";

        public VoucherNumberService(IAppDataFolder appDataFolder)
        {
            _appDataFolder = appDataFolder;
        }

        public void ResetNumber()
        {
            _appDataFolder.CreateFile(VoucherNoFileName, "0");
        }
        public string DispayGenerateNumber()
        {
            int count = 0;
            if (_appDataFolder.FileExists(VoucherNoFileName))
            {
                int.TryParse(_appDataFolder.ReadFile(VoucherNoFileName), out count);
            }

            count++;
           return string.Format("{0}{1:D4}", DateTime.Today.ToString("yyyyMMdd"), count);
        }
        public string GenerateNumber()
        {
            int count = 0;
            if (_appDataFolder.FileExists(VoucherNoFileName))
            {
                int.TryParse(_appDataFolder.ReadFile(VoucherNoFileName), out count);
            }

            count++;
            _appDataFolder.CreateFile(VoucherNoFileName, count.ToString());
            return string.Format("{0}{1:D4}", DateTime.Today.ToString("yyyyMMdd"), count);
        }
    }
}