angular.module('jmmApp').factory('fakeSvc', ['$q', function ($q) {
    var service = {};

    service.getYears = function () {
        var deferred = $q.defer();
        deferred.resolve([1999, 2001, 2002, 2003, 2004, 2005]);
        return deferred.promise;
    }

    service.getMakes = function (year) {
        var deferred = $q.defer();
        switch (year) {
            case 1999:
            case 2001:
            case 2002:
            case 2003:
            case 2004:
            case 2005:
                deferred.resolve(['Toyota', 'Ford', 'Chevvy', 'Abarth'])
                break;
        }
        return deferred.promise;
    }
        
    service.getModels = function (make) {
        var deferred = $q.defer();
        switch (make) {
            case 'Toyota':
                deferred.resolve(['Camry', 'Cressida'])
                break;
            case 'Ford':
                deferred.resolve(['Sable', 'F-150'])
                break;
            case 'Chevvy':
                deferred.resolve(['Malibu', 'Nova'])
                break;
            case 'Abarth':
                deferred.resolve(['Fiesta', 'Bomb'])
                break;
        }
        return deferred.promise;
    }

    service.getTrims = function (model) {
        var deferred = $q.defer();
        switch (model) {
            case 'Camry':
                deferred.resolve(['AM Radio', '4 Tires'])
                break;
            case 'F-150':
                deferred.resolve(['Sweet Blonde', 'Gun Rack'])
                break;
            case 'Malibu':
                deferred.resolve(['Surboard rack', 'Free Insurance'])
                break;
            case 'Fiesta':
                deferred.resolve(['Italian Mob', 'Bomb'])
                break;
        }
        return deferred.promise;
    }

    return service;
}]);