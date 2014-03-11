﻿using Coevery.Mvc.ClientRoute;

namespace Coevery.DeveloperTools.Services {
    public class ClientRouteProvider : ClientRouteProviderBase {
        public override void Discover(ClientRouteTableBuilder builder) {
            builder.Describe("EntityList")
                .Configure(descriptor => { descriptor.Url = "/Entities"; })
                .View(view => {
                    view.TemplateUrl = "'DeveloperTools/EntityAdmin/List'";
                    view.Controller = "EntityListCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, "controllers/listcontroller");
                });

            builder.Describe("EntityCreate")
                .Configure(descriptor => { descriptor.Url = "/Entities/Create"; })
                .View(view => {
                    view.TemplateUrl = "'DeveloperTools/EntityAdmin/Create'";
                    view.Controller = "EntityEditCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, "controllers/editcontroller");
                });

            builder.Describe("EntityEdit")
                .Configure(descriptor => { descriptor.Url = "/Entities/{Id:[0-9a-zA-Z]+}/Edit"; })
                .View(view => {
                    view.TemplateProvider = @"['$http', '$stateParams', function ($http, $stateParams) {
                                                var url = 'DeveloperTools/EntityAdmin/Edit/' + $stateParams.Id; 
                                                return $http.get(url).then(function(response) { return response.data; });
                                          }]";
                    view.Controller = "EntityEditCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/editcontroller"});
                });

            builder.Describe("EntityDetail")
                .Configure(descriptor => {
                    descriptor.Abstract = true;
                    descriptor.Url = "/Entities/{Id:[0-9a-zA-Z]+}";
                })
                .View(view => {
                    view.TemplateProvider = @"['$http', '$stateParams', function ($http, $stateParams) {
                                                var url = 'DeveloperTools/EntityAdmin/Detail/' + $stateParams.Id; 
                                                return $http.get(url).then(function(response) { return response.data; });
                                          }]";
                    view.Controller = "EntityDetailCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/detailcontroller"});
                });

            builder.Describe("EntityDetail.Fields")
                .View(view => {
                    view.TemplateUrl = "'DeveloperTools/EntityAdmin/Fields/'";
                    view.Controller = "FieldsCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/fieldscontroller"});
                });

            #region Operate fields

            builder.Describe("EntityDetail.Fields.Create")
                .Configure(descriptor => { descriptor.Url = "/Create"; })
                .View(view => {
                    view.TemplateUrl = "function(params) { return 'DeveloperTools/FieldAdmin/ChooseFieldType/' + params.Id; }";
                    view.Controller = "ChooseFieldTypeCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/choosefieldtypecontroller"});
                });

            builder.Describe("EntityDetail.Fields.CreateFillInfo")
                .Configure(descriptor => { descriptor.Url = "/Create/{FieldTypeName:[0-9a-zA-Z]+}"; })
                .View(view => {
                    view.TemplateUrl = "function(params) { return 'DeveloperTools/FieldAdmin/FillFieldInfo/' + params.Id + '?FieldTypeName=' + params.FieldTypeName; }";
                    view.Controller = "FillFieldInfoCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/fillfieldinfocontroller"});
                });

            builder.Describe("EntityDetail.Fields.CreateConfirmInfo")
                .Configure(descriptor => { descriptor.Url = "/Create/{FieldTypeName:[0-9a-zA-Z]+}/Confirm"; })
                .View(view => {
                    view.TemplateUrl = "function(params) { return 'DeveloperTools/FieldAdmin/ConfirmFieldInfo/' + params.Id; }";
                    view.Controller = "ConfirmFieldInfoCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/confirmfieldinfocontroller"});
                });

            builder.Describe("FieldEdit")
                .Configure(descriptor => { descriptor.Url = "/Fields/{EntityName:[0-9a-zA-Z]+}/Edit/{FieldName:[0-9a-zA-Z]+}"; })
                .View(view => {
                    view.TemplateProvider = @"['$http', '$stateParams', function ($http, $stateParams) {
                                                var url = 'DeveloperTools/FieldAdmin/EditFields/' + $stateParams.EntityName + '?FieldName=' + $stateParams.FieldName;
                                                return $http.get(url).then(function(response) { return response.data; });
                                          }]";
                    view.Controller = "FieldEditCtrl";
                    view.AddDependencies(ToAbsoluteScriptUrl, new[] {"controllers/editfieldscontroller"});
                });

            #endregion
        }
    }
}