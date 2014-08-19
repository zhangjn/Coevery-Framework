using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.OptionSet.Models;
using Coevery.Core.OptionSet.ViewModels;
using Coevery.Data;
using Coevery.Localization;
using Coevery.Logging;
using Coevery.Security;
using Coevery.UI.Notify;
using Coevery.Utility.Extensions;

namespace Coevery.Core.OptionSet.Services {
    public class OptionSetService : IOptionSetService {
        private readonly IRepository<OptionItemContentItem> _optionItemContentItemRepository;
        private readonly IContentManager _contentManager;
        private readonly INotifier _notifier;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICoeveryServices _services;

        public OptionSetService(
            IRepository<OptionItemContentItem> optionItemContentItemRepository,
            IContentManager contentManager,
            INotifier notifier,
            IAuthorizationService authorizationService,
            ICoeveryServices services) {
            _optionItemContentItemRepository = optionItemContentItemRepository;
            _contentManager = contentManager;
            _notifier = notifier;
            _authorizationService = authorizationService;
            _services = services;

            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public IEnumerable<OptionSetPart> GetOptionSets() {
            return _contentManager.Query<OptionSetPart, OptionSetPartRecord>().List();
        }

        public OptionSetPart GetOptionSet(int id) {
            return _contentManager.Get(id, VersionOptions.Published, new QueryHints().ExpandParts<OptionSetPart>()).As<OptionSetPart>();
        }

        public OptionSetPart GetOptionSetByName(string name) {
            if (String.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException("name");
            }

            return _contentManager
                .Query<OptionSetPart>()
                .Where<OptionSetPartRecord>(r => r.Name == name)
                .List()
                .FirstOrDefault();
        }


        public void DeleteOptionSet(OptionSetPart taxonomy) {
            _contentManager.Remove(taxonomy.ContentItem);

            // Removing terms
            foreach (var term in GetOptionItems(taxonomy.Id)) {
                DeleteOptionItem(term);
            }
        }

        public OptionItemPart NewOptionItem(OptionSetPart optionSet) {
            var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
            optionItem.OptionSetId = optionSet.Id;

            return optionItem;
        }

        public IEnumerable<OptionItemPart> GetOptionItems(int optionSetId) {
            var result = _contentManager.Query<OptionItemPart, OptionItemPartRecord>()
                .Where(x => x.OptionSetId == optionSetId)
                .List();

            return OptionItemPart.Sort(result);
        }

        public OptionItemPart GetOptionItem(int id) {
            return _contentManager
                .Query<OptionItemPart, OptionItemPartRecord>()
                .Where(x => x.Id == id).List().FirstOrDefault();
        }

        public IEnumerable<OptionItemPart> GetOptionItemsForContentItem(int contentItemId, string field = null) {
            return String.IsNullOrEmpty(field)
                ? _optionItemContentItemRepository.Fetch(x => x.OptionItemContainerPartRecord.Id == contentItemId).Select(t => GetOptionItem(t.OptionItemRecord.Id))
                : _optionItemContentItemRepository.Fetch(x => x.OptionItemContainerPartRecord.Id == contentItemId && x.Field == field).Select(t => GetOptionItem(t.OptionItemRecord.Id));
        }

        public OptionItemPart GetOptionItemByName(int optionSetId, string name) {
            return _contentManager
                .Query<OptionItemPart, OptionItemPartRecord>()
                .Where(t => t.OptionSetId == optionSetId && t.Name == name)
                .List()
                .FirstOrDefault();
        }

        public bool CreateOptionItem(OptionItemPart termPart) {
            if (GetOptionItemByName(termPart.OptionSetId, termPart.Name) == null) {
                _authorizationService.CheckAccess(StandardPermissions.AccessAdminPanel, _services.WorkContext.CurrentUser, null);

                _contentManager.Create(termPart);
                return true;
            }
            _notifier.Warning(T("The option item {0} already exists in this ", termPart.Name));
            return false;
        }

        public bool EditOptionItem(OptionItemEntry newItem) {
            var oldItem = _contentManager.Get(newItem.Id).As<OptionItemPart>();
            if (oldItem == null) {
                return false;
            }
            oldItem.Name = newItem.Name;
            oldItem.Selectable = newItem.Selectable;
            oldItem.Weight = newItem.Weight;
            return true;
        }

        public void DeleteOptionItem(OptionItemPart optionItemPart) {
            _contentManager.Remove(optionItemPart.ContentItem);

            // delete termContentItems
            var optionItemContentItems = _optionItemContentItemRepository
                .Fetch(t => t.OptionItemRecord == optionItemPart.Record)
                .ToList();

            foreach (var item in optionItemContentItems) {
                _optionItemContentItemRepository.Delete(item);
            }
        }

        public void UpdateSelectedItems(ContentItem contentItem, IEnumerable<OptionItemPart> items, string field) {
            var containerPart = contentItem.As<OptionItemContainerPart>();

            // removing current terms for specific field
            var fieldIndexes = containerPart.OptionItems
                .Where(t => t.Field == field)
                .Select((t, i) => i)
                .OrderByDescending(i => i)
                .ToList();

            foreach (var x in fieldIndexes) {
                containerPart.OptionItems.RemoveAt(x);
            }

            // adding new terms list
            foreach (var item in items) {
                containerPart.OptionItems.Add(
                    new OptionItemContentItem {
                        OptionItemContainerPartRecord = containerPart.Record,
                        OptionItemRecord = item.Record,
                        Field = field
                    });
            }
        }

        public IContentQuery<OptionItemContainerPart, OptionItemContainerPartRecord> GetContentItemsQuery(OptionItemPart term, string fieldName = null) {
            var query = _contentManager
                .Query<OptionItemContainerPart, OptionItemContainerPartRecord>();

            if (String.IsNullOrWhiteSpace(fieldName)) {
                query = query.Where(
                    tpr => tpr.OptionItems.Any(tr =>
                        tr.OptionItemRecord.Id == term.Id));
            }
            else {
                query = query.Where(
                    tpr => tpr.OptionItems.Any(tr =>
                        tr.Field == fieldName
                        && (tr.OptionItemRecord.Id == term.Id)));
            }

            return query;
        }

        public IEnumerable<IContent> GetContentItems(OptionItemPart term, int skip = 0, int count = 0, string fieldName = null) {
            return GetContentItemsQuery(term, fieldName)
                //.Join<CommonPartRecord>()
                //.OrderByDescending(x => x.CreatedUtc)
                .Slice(skip, count);
        }
    }
}