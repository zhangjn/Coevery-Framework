using System.Linq;
using Coevery.ContentManagement;
using Coevery.Core.Fields.Providers;
using Coevery.Core.OptionSet.Fields;
using Coevery.Core.OptionSet.Services;

namespace Coevery.Core.OptionSet.Projections {

    public class OptionSetFieldValueProvider : ContentFieldValueProvider<OptionSetField> {
        private readonly IOptionSetService _optionSetService;

        public OptionSetFieldValueProvider(IOptionSetService optionSetService) {
            _optionSetService = optionSetService;
        }

        public override object GetValue(ContentItem contentItem, ContentField field) {
            var optionItems = _optionSetService.GetOptionItemsForContentItem(contentItem.VersionRecord.Id, field.Name).ToList();

            var value = string.Join(", ", optionItems.Select(t => t.Name).ToArray());
            return value;
        }
    }
}