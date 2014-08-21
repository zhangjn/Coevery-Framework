'use strict';
define(['core/app/detourService'], function (detour) {
    detour.registerController([
        'ProjectionDetailCtrl',
        ['$rootScope', '$scope', 'logger', '$state', '$stateParams', '$http',
            function ($rootScope, $scope, logger, $state, $stateParams, $http) {
                var isInit = true;
                $scope.fieldCoumns = [];
                $scope.SelectedColumns = [];

                var previewSection = $('#preview-section');
                $scope.preview = function () {
                    var pool = { list: [] };
                    $scope.$broadcast("getSelectedList", pool);

                    $scope.fieldCoumns = [];
                    $scope.myData = [];
                    var jsondata = {};
                    for (var i = 0; i < pool.list.length; i++) {
                        var fieldName = pool.list[i].Value;
                        $scope.fieldCoumns[i] = { name: fieldName, label: pool.list[i].Text };
                        jsondata[fieldName] = "data_" + fieldName;
                    }
                    if (i > 0) {
                        for (var j = 0; j < 5; j++) {
                            var newjson = {};
                            for (var filed in jsondata) {
                                newjson[filed] = jsondata[filed] + "_" + (j + 1);
                            }
                            $scope.myData.push(newjson);
                        }
                    }
                    
                    var sortby = $('#SortColumn').val();
                    var sortmode = $('#SortMode').val();
                    sortmode || (sortmode = 'asc');
                    $scope.gridOptions = {
                        colModel: $scope.fieldCoumns,
                        needReloading: !isInit,
                        sortname: sortby,
                        sortorder: sortmode
                    };
                    isInit = false;
                    angular.extend($scope.gridOptions, $rootScope.defaultGridOptions);
                    $scope.gridOptions.datatype = "local";
                    $scope.gridOptions.data = $scope.myData;

                    previewSection.show();
                    scroll(0, previewSection.offset().top);
                };

                var validator = $("form[name=myForm]").validate({
                    errorClass: "inputError"
                });

                $scope.save = function () {
                    var form = $("form[name=myForm]");
                    if (!validator.form()) {
                        return null;
                    }

                    var promise = $http({
                        url: form.attr('action'),
                        method: "POST",
                        data: form.serialize(),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        tracker: 'saveview'
                    }).then(function (response) {
                        logger.success('Save succeeded.');
                        return response;
                    }, function (reason) {
                        logger.error('Save Failed： ' + reason);
                    });
                    return promise;
                };

                $scope.saveAndView = function () {
                    var promise = $scope.save();
                    promise && promise.then(function (response) {
                        var id = response.data.id;
                        if (id)
                            $state.transitionTo('ProjectionEdit', { EntityName: $stateParams.EntityName, Id: id });
                    });
                };

                $scope.saveAndBack = function () {
                    var promise = $scope.save();
                    promise && promise.then(function () {
                        $scope.exit();
                    }, function () {
                    });
                };

                $scope.exit = function () {
                    $state.transitionTo('EntityDetail.Views', { Id: $stateParams.EntityName });
                };

            }]
    ]);
});