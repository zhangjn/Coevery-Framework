'use strict';

define(['core/app/detourService'], function(detour) {
    detour.registerFactory([
        'entityDataService',
        ['$rootScope', '$resource', function($rootScope, $resource) {
            return $resource(applicationBaseUrl + 'api/EntityManagement/entity/:Name',
                { Name: '@Name' },
                {
                    update: { method: 'PUT' },
                    get: { method: 'GET' }
                });
        }]
    ]);
});