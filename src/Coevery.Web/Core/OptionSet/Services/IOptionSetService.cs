using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.Core.OptionSet.Models;
using Coevery.Core.OptionSet.ViewModels;

namespace Coevery.Core.OptionSet.Services {
    public interface IOptionSetService : IDependency {
        IEnumerable<OptionSetPart> GetOptionSets();
        OptionSetPart GetOptionSet(int id);
        OptionSetPart GetOptionSetByName(string name);
        void DeleteOptionSet(OptionSetPart taxonomy);

        IEnumerable<OptionItemPart> GetOptionItems(int optionSetId);
        OptionItemPart GetOptionItem(int id);
        OptionItemPart GetOptionItemByName(int optionSetId, string name);
        void DeleteOptionItem(OptionItemPart optionItemPart);

        OptionItemPart NewOptionItem(OptionSetPart taxonomy);
        bool CreateOptionItem(OptionItemPart termPart);
        bool EditOptionItem(OptionItemEntry newItem);
        IEnumerable<OptionItemPart> GetOptionItemsForContentItem(int contentItemId, string field = null);
        void UpdateSelectedItems(ContentItem contentItem, IEnumerable<OptionItemPart> optionItems, string field);
        IEnumerable<IContent> GetContentItems(OptionItemPart term, int skip = 0, int count = 0, string fieldName = null);
    }
}