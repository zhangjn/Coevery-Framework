using Coevery.DeveloperTools.OptionSet.Models;
using Coevery.DeveloperTools.OptionSet.ViewModels;

namespace Coevery.DeveloperTools.OptionSet.Helpers {
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