'use strict';

define(['core/app/detourService', 'DeveloperTools/EntityManagement/Scripts/services/entitydataservice'], function (detour) {
    detour.registerController([
        'EntityPublishConfirmCtrl',
        ['$scope', 'logger', '$state', '$stateParams', 'entityDataService', '$http',
            function ($scope, logger, $state, $stateParams, entityDataService, $http) {
                $scope.exit = function () {
                    $state.transitionTo('EntityEdit', { Id: $stateParams.Id });
                };

                $scope.publish = function () {
                    //$http.get('EntityManagement/Admin/Publish/' + $stateParams.Id).then(function () {
                    //    logger.success("Publish the entity successful.");
                    //});
                    var form = $("#myForm");
                    var promise = $http({
                        url: form.attr('action'),
                        method: "POST",
                        data: form.serialize(),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        tracker: 'saveRole'
                    }).then(function (response) {
                        logger.success('Publish succeeded');
                        return response;
                    }, function (reason) {
                        logger.error('Publish Failed： ' + reason.data);
                    });
                };
            }]
    ]);
});