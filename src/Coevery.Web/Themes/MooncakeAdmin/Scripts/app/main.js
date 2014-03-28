require.config({
    paths: {
        core: 'Core/Common/Scripts',
        app: 'Themes/MooncakeAdmin/Scripts/app/' + appPrefix() + 'app'
    }
});


require(['app', 'core/app/logger'], function () {
    'use strict';
    
    angular.element(document).ready(function() {
        angular.bootstrap(document, ['coevery', function($locationProvider) {
            //$locationProvider.html5Mode(true).hashPrefix('!');
        }]);
    });
});
