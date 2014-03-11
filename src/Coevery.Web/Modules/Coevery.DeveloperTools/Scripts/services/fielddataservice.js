﻿'use strict';

define(['core/app/detourService'], function(detour) {
    detour.registerFactory([
        'fieldDataService',
        ['$rootScope', '$resource', function($rootScope, $resource) {
            return $resource(applicationBaseUrl + 'api/developertools/field/:Name',
                { Name: '@Name' },
                { update: { method: 'PUT' } });
        }]
    ]);
});