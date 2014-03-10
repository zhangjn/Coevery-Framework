require.config({
    paths: {
        core: 'Core/Common/Scripts',
        app: 'Core/Common/Scripts/app/' + appPrefix() + 'app'
    }
});


require(['app', 'core/app/logger'], function (app) {
    'use strict';
    
    var config = requirejs.s.contexts._.config;
    
    angular.element(document).ready(function() {
        angular.bootstrap(document, ['coevery', function($locationProvider) {
            //$locationProvider.html5Mode(true).hashPrefix('!');
        }]);
    });
});
