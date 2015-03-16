
var app365 = angular.module('app365', [
  'ngRoute']);






app365.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/', {
            templateUrl: 'partials/sign-in.html',
            controller: 'signInCtrl'
        }).
        when('/home', {
            templateUrl: 'partials/contact-list.html',
            controller: 'contactCtrl'
        })

  }
]);