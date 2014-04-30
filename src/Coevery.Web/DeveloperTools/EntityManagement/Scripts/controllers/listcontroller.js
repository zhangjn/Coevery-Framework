'use strict';

define(['core/app/detourService', 'DeveloperTools/EntityManagement/Scripts/services/entitydataservice'], function (detour) {
    detour.registerController([
        'EntityListCtrl',
        ['$rootScope', '$scope', 'logger', '$state', 'entityDataService', '$http', '$i18next',
            function($rootScope, $scope, logger, $state, entityDataService, $http, $i18next) {

                var metadataColumnDefs = [
                    {
                        name: 'Name',
                        label: 'Name',
                        key: true,
                        formatter: $rootScope.cellLinkTemplate,
                        formatoptions: { hasView: true }
                    },
                    //{ name: 'Id', label: $i18next('Id'), hidden: true, sorttype: 'int' },
                    { name: 'DisplayName', label: $i18next('Display Name') },
                    { name: 'Modified', label: $i18next('Has Unpublished Modification') },
                    { name: 'HasPublished', label: $i18next('Has Ever Published') }
                ];

                $scope.gridOptions = {
                    url: "api/EntityManagement/entity",
                    colModel: metadataColumnDefs,
                    rowIdName: "Name"
                };

                angular.extend($scope.gridOptions, $rootScope.defaultGridOptions);

                $scope['delete'] = function() {
                    var deleteEntity = !!$scope.selectedRow ? $scope.selectedRow.Name : null;
                    if (!deleteEntity) return;
                    entityDataService['delete']({ name: deleteEntity }, function() {
                        if ($scope.selectedItems.length !== 0) {
                            $scope.selectedItems = [];
                        }
                        $scope.getAllMetadata();
                        logger.success("Delete the entity successful.");
                        //window.location.reload();
                    }, function(reason) {
                        logger.error("Failed to delete the entity:" + reason);
                    });
                };
                $scope.add = function() {
                    $state.transitionTo('EntityCreate', { Module: 'Entities' });
                };

                $scope.view = function(entityName) {
                    $state.transitionTo('EntityDetail.Fields', { Id: entityName });
                };

                $scope.edit = function(entityName) {
                    $state.transitionTo('EntityEdit', { Id: entityName });
                };

                $scope.getAllMetadata = function() {
                    $("#gridList").jqGrid('setGridParam', {
                        datatype: "json"
                    }).trigger('reloadGrid');
                };

                $scope.publish = function () {
                    var magicToken = $("input[name=__RequestVerificationToken]").first();
                    if (magicToken.length == 0) {
                        return;
                    } // no sense in continuing if form POSTS will fail
                    $http({
                        url: 'EntityManagement/Admin/Publish',
                        method: 'POST',
                        data: 'id=' + $scope.selectedItems[0]  + '&__RequestVerificationToken=' + magicToken.val(),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        tracker: 'publishEntity'
                    }).then(function (response) {
                        logger.success('Publish succeeded.');
                        $scope.getAllMetadata();
                        $scope.selectedItems = [];
                        return response;
                    }, function (reason) {
                        logger.error('Publish failed： ' + reason.data);
                    });
                };
            }]
    ]);
});