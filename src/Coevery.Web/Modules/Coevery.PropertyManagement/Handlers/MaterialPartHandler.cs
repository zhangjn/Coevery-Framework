using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.Records;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Handlers
{
    public class MaterialPartHandler : ContentHandler
    {
        private readonly Lazy<ISessionLocator> _sessionLocator;
        private readonly ICoeveryServices _coeveryServices;

        public MaterialPartHandler(IRepository<MaterialPartRecord> repository, Lazy<ISessionLocator> sessionLocator, ICoeveryServices coeveryServices)
        {
            _sessionLocator = sessionLocator;
            _coeveryServices = coeveryServices;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));
            OnRemoving<MaterialPart>(Removing);
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private void Removing(RemoveContentContext context, MaterialPart part)
        {
            /*var session = _sessionLocator.Value.For(typeof(ContentPartRecord));

            var hql = string.Format("select count(Id) from {0} where MaterialId = {1}", typeof(InventoryRecord).FullName, part.Id);
            int count = Convert.ToInt32(session.CreateQuery(hql).SetCacheable(true).UniqueResult());
            if (count > 0)
            {
                _coeveryServices.Notifier.Error(T("该记录已经被使用，不能被删除！"));
                context.Cancel = true;
            }*/
        }
    }
}