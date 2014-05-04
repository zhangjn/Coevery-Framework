'use strict';

define(['core/app/detourService', 'DeveloperTools/EntityManagement/Scripts/services/entitydataservice'], function (detour) {
    detour.registerController([
        'EntityDetailCtrl',
        ['$scope', 'logger', '$state', '$stateParams', 'entityDataService', '$http',
            function ($scope, logger, $state, $stateParams, entityDataService, $http) {
                $scope.refreshTab = function() {
                    $scope.showField = $state.includes('EntityDetail.Fields');
                    $scope.showRelation = $state.includes('EntityDetail.Relationships');
                    $scope.showView = $state.includes('EntityDetail.Views');
                };
                
                $scope.exit = function () {
                    $state.transitionTo('EntityList');
                };

                $scope.edit = function () {
                    $state.transitionTo('EntityEdit', { Id: $stateParams.Id });
                };

                $scope['delete'] = function () {
                    entityDataService['delete']({ name: $stateParams.Id }, function () {
                        $state.transitionTo('EntityList');
                        logger.success("Delete the entity successful.");
                    }, function (reason) {
                        logger.error("Failed to delete the entity:" + reason);
                    });
                };

                $scope.formDesigner = function () {
                    $state.transitionTo('FormDesigner', { EntityName: $stateParams.Id });
                };

                $scope.publish = function() {
                    var magicToken = $("input[name=__RequestVerificationToken]").first();
                    if (magicToken.length == 0) {
                        return;
                    } // no sense in continuing if form POSTS will fail
                    $http({
                        url: 'EntityManagement/Admin/Publish',
                        method: 'POST',
                        data: 'id=' + $stateParams.Id + '&__RequestVerificationToken=' + magicToken.val(),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        tracker: 'publishEntity'
                    }).then(function(response) {
                        logger.success('Publish succeeded.');
                        return response;
                    }, function(reason) {
                        logger.error('Publish failed： ' + reason.data);
                    });
                };
            }]
    ]);
});