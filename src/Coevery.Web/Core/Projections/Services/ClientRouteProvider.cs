﻿using Coevery.Mvc.ClientRoute;

namespace Coevery.Core.Projections.Services {
    public class ClientRouteProvider : ClientRouteProviderBase {
        public override void Discover(ClientRouteTableBuilder builder) {
            builder.Describe("EntityDetail.Views")
                .Configure(descriptor => { descriptor.Url = "/Views"; })
                .View(view => {
                    view.TemplateUrl = "'" + ModuleBasePath + @"List'";
                    view.Controller = "ProjectionListCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/listcontroller"});
                });

            builder.Describe("ProjectionCreate")
                .Configure(descriptor => { descriptor.Url = "/Projections/{EntityName:[0-9a-zA-Z]+}/Create/{Category:[0-9a-zA-Z]+}/{Type:[0-9a-zA-Z]+}"; })
                .View(view => {
                    view.TemplateUrl = "function(params) { return '" + ModuleBasePath + @"Create/' + params.EntityName + '?category='+ params.Category +'&type=' + params.Type;}";
                    view.Controller = "ProjectionDetailCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/detailcontroller"});
                });

            builder.Describe("ProjectionEdit")
                .Configure(descriptor => { descriptor.Url = "/Projections/{EntityName:[0-9a-zA-Z]+}/{Id:[0-9a-zA-Z]+}"; })
                .View(view => {
                    view.TemplateProvider = @"['$http', '$stateParams', function ($http, $stateParams) {
                                                var url = '" + ModuleBasePath + @"Edit/' + $stateParams.Id; 
                                                return $http.get(url).then(function(response) { return response.data; });
                                          }]";
                    view.Controller = "ProjectionDetailCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/detailcontroller"});
                });
        }
    }
}