﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coevery.ContentManagement;
using Coevery.Core.OptionSet.Helpers;
using Coevery.Core.OptionSet.Models;
using Coevery.Core.OptionSet.Services;
using Coevery.Core.OptionSet.ViewModels;
using Coevery.Localization;

namespace Coevery.Core.OptionSet.Controllers {
    public class OptionItemController : ApiController {
        private readonly IOptionSetService _optionSetService;
        public OptionItemController(
            IOptionSetService optionSetService,
            ICoeveryServices coeveryServices) {
            _optionSetService = optionSetService;
            Services = coeveryServices;
            T = NullLocalizer.Instance;
        }
        public Localizer T { get; set; }
        public ICoeveryServices Services { get; set; }

        // GET api/<controller>
        public object Get(int optionSetId, int page, int rows) {
            var result = _optionSetService.GetOptionItems(optionSetId);
            if (!result.Any()) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            var query = result.Select(part => part.CreateTermEntry());
            var totalRecords = query.Count();
            return new {
                total = Convert.ToInt32(Math.Ceiling((double)totalRecords / rows)),
                page = page,
                records = totalRecords,
                rows = query
            };
        }

        // POST api/<controller>
        public HttpResponseMessage Post(OptionItemEntry optionItem) {
            var itemPart = Services.ContentManager.New<OptionItemPart>("OptionItem");
            itemPart.OptionSetId = optionItem.OptionSetId;
            itemPart.Name = optionItem.Name;
            itemPart.Selectable = optionItem.Selectable;
            itemPart.Weight = optionItem.Weight;
            return _optionSetService.CreateTerm(itemPart)
                ? Request.CreateResponse(HttpStatusCode.OK)
               : Request.CreateResponse(HttpStatusCode.Conflict, T("The term {0} already exists in this optionset", itemPart.Name));
        }

        // PUT api/<controller>/...
        public HttpResponseMessage Put(OptionItemEntry optionItem) {
            return _optionSetService.EditOptionItem(optionItem) ?
                Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE api/<controller>/5
        public void Delete(int id) {
            var optionItem = _optionSetService.GetOptionItem(id);
            _optionSetService.DeleteOptionItem(optionItem);
        }
    }
}