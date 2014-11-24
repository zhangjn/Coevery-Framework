using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.Records;
using Coevery.Data;

namespace Coevery.PropertyManagement.Handlers
{
    public static class DeleteRecordFilter
    {
        public static DeleteRecordFilter<TRecord> For<TRecord>(IRepository<TRecord> repository) where TRecord : ContentPartRecord, new()
        {
            if (typeof(TRecord).IsSubclassOf(typeof(ContentPartVersionRecord)))
            {
                var filterType = typeof(StorageVersionFilter<>).MakeGenericType(typeof(TRecord));
                return (DeleteRecordFilter<TRecord>)Activator.CreateInstance(filterType, repository);
            }
            return new DeleteRecordFilter<TRecord>(repository);
        }
    }

    public class DeleteRecordFilter<TRecord> : StorageFilterBase<ContentPart<TRecord>> where TRecord : ContentPartRecord, new()
    {
        protected readonly IRepository<TRecord> _repository;

        public DeleteRecordFilter(IRepository<TRecord> repository)
        {
            if (this.GetType() == typeof(StorageFilter<TRecord>) && typeof(TRecord).IsSubclassOf(typeof(ContentPartVersionRecord)))
            {
                throw new ArgumentException(
                    string.Format("Use {0} (or {1}.For<TRecord>()) for versionable record types", typeof(StorageVersionFilter<>).Name, typeof(StorageFilter).Name),
                    "repository");
            }

            _repository = repository;
        }

        protected override void Removed(RemoveContentContext context, ContentPart<TRecord> instance) {
            _repository.Delete(instance.Record);
        }
    }
}