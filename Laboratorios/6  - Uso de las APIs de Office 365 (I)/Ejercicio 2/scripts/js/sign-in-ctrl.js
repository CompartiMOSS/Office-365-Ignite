(function () {
    'use strict';

    angular.module('app365').controller('signInCtrl', ['$scope', '$location', 'app365api', signInCtrl]);

    function signInCtrl($scope,  $location , app365api) {

        $scope.signIn = function () {
            app365api.login(onlogin);
        };

        var onlogin = function (reason) {
            $location.path("/home");
        };
    }
})();