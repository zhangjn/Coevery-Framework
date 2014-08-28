using System;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.Core.OptionSet.Models;
using Coevery.Core.OptionSet.Services;
using Coevery.Localization;
using Coevery.Mvc;
using Coevery.Settings;
using Coevery.UI.Navigation;

namespace Coevery.Core.OptionSet.Drivers {
    public class OptionItemPartDriver : ContentPartDriver<OptionItemPart> {
        private readonly IOptionSetService _optionSetService;
        private readonly ISiteService _siteService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContentManager _contentManager;

        public OptionItemPartDriver(
            IOptionSetService optionSetService,
            ISiteService siteService,
            IHttpContextAccessor httpContextAccessor,
            IContentManager contentManager) {
            _optionSetService = optionSetService;
            _siteService = siteService;
            _httpContextAccessor = httpContextAccessor;
            _contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix {
            get { return "OptionItem"; }
        }

        protected override DriverResult Display(OptionItemPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_OptionItemPart", () => {
                var pagerParameters = new PagerParameters();
                var httpContext = _httpContextAccessor.Current();
                if (httpContext != null) {
                    pagerParameters.Page = Convert.ToInt32(httpContext.Request.QueryString["page"]);
                }

                var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
                var optionSet = _optionSetService.GetOptionSet(part.OptionSetId);
                var totalItemCount = 100;

                // asign Taxonomy and Term to the content item shape (Content) in order to provide 
                // alternates when those content items are displayed when they are listed on a term
                //var termContentItems = _optionSetService.GetContentItems(part, pager.GetStartIndex(), pager.PageSize)
                //    .Select(c => _contentManager.BuildDisplay(c, "Summary").Taxonomy(optionSet).Term(part));

                var list = shapeHelper.List();

                //list.AddRange(termContentItems);

                var pagerShape = shapeHelper.Pager(pager)
                    .TotalItemCount(totalItemCount)
                    .Taxonomy(optionSet)
                    .Term(part);

                return shapeHelper.Parts_TermPart(ContentItems: list, Taxonomy: optionSet, Pager: pagerShape);
            });
        }

        protected override DriverResult Editor(OptionItemPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Taxonomies_Term_Fields",
                () => shapeHelper.EditorTemplate(TemplateName: "Parts/Taxonomies.Term.Fields", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(OptionItemPart termPart, IUpdateModel updater, string displayType, dynamic shapeHelper) {
            if (updater.TryUpdateModel(termPart, Prefix, null, null)) {
                var existing = _optionSetService.GetOptionItemByName(termPart.OptionSetId, termPart.Name);
                if (existing != null && existing.Record != termPart.Record && existing.OptionSetId == termPart.OptionSetId) {
                    updater.AddModelError("Name", T("The option item {0} already exists", termPart.Name));
                }
            }

            return Editor(termPart, displayType, shapeHelper);
        }

        protected override void Exporting(OptionItemPart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Selectable", part.Record.Selectable);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Weight", part.Record.Weight);

            var taxonomy = _contentManager.Get(part.Record.OptionSetId);
            var identity = _contentManager.GetItemMetadata(taxonomy).Identity.ToString();
            context.Element(part.PartDefinition.Name).SetAttributeValue("OptionSetId", identity);
        }

        protected override void Importing(OptionItemPart part, ImportContentContext context) {
            part.Record.Selectable = Boolean.Parse(context.Attribute(part.PartDefinition.Name, "Selectable"));
            part.Record.Weight = Int32.Parse(context.Attribute(part.PartDefinition.Name, "Weight"));

            var identity = context.Attribute(part.PartDefinition.Name, "OptionSetId");
            var contentItem = context.GetItemFromSession(identity);

            if (contentItem == null) {
                throw new CoeveryException(T("Unknown taxonomy: {0}", identity));
            }

            part.Record.OptionSetId = contentItem.Id;
        }
    }
}