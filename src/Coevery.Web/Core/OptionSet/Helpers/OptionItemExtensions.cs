using Coevery.Core.OptionSet.Models;
using Coevery.Core.OptionSet.ViewModels;

namespace Coevery.Core.OptionSet.Helpers {
    public static class OptionItemExtensions {
        public static OptionItemEntry CreateTermEntry(this OptionItemPart term) {
            return new OptionItemEntry {
                Id = term.Id,
                Name = term.Name,
                Selectable = term.Selectable,
                Weight= term.Weight,
                IsChecked = false,
                ContentItem = term.ContentItem,
                OptionSetId = term.OptionSetId
            };
        }
    }
}