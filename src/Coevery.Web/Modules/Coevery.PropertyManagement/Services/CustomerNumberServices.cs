using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Coevery.FileSystems.AppData;

namespace Coevery.PropertyManagement.Services
{
    public interface ICustomerNumberService : IDependency
    {
        string GenerateNumber();
        void SaveNumber();
    }

    [Serializable]
    internal class CustomerNumberEntity
    {
        public int Number { get; set; }
    }

    [Serializable]
    public class CustomerNumberServices : ICustomerNumberService
    {
        private const string CustomerNoNoFileName = "CustomerNo.bin";
        private readonly IAppDataFolder _appDataFolder;

        public CustomerNumberServices(IAppDataFolder appDataFolder)
        {
            _appDataFolder = appDataFolder;
        }

        public string GenerateNumber()
        {
            var entity = new CustomerNumberEntity
            {
                Number = 0
            };
            if (_appDataFolder.FileExists(CustomerNoNoFileName))
            {
                var formatter = new BinaryFormatter();
                using (Stream stream = _appDataFolder.OpenFile(CustomerNoNoFileName))
                {
                    entity = (CustomerNumberEntity) formatter.Deserialize(stream);
                }
            }
            entity.Number++;
            return string.Format("{0:D4}", entity.Number);
        }

        public void SaveNumber()
        {
            var entity = new CustomerNumberEntity
            {
                Number = 0
            };
            if (_appDataFolder.FileExists(CustomerNoNoFileName))
            {
                var formatter = new BinaryFormatter();
                using (Stream stream = _appDataFolder.OpenFile(CustomerNoNoFileName))
                {
                    entity = (CustomerNumberEntity)formatter.Deserialize(stream);
                }
            }
            entity.Number++;
            using (Stream stream = _appDataFolder.CreateFile(CustomerNoNoFileName))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, entity);
            }
        }
    }
}