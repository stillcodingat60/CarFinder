angular.module('jmmApp').factory('carSvc', ['$http', '$q', function ($http, $q) {

    var service = {};

    service.getYears = function () {
        return $http.post("/api/car/GetAllYears").then(function (response) {
            return response.data;
        });
    };

    service.getMakes = function (selected) {
        return $http.post("/api/car/GetMakesbyYear", selected).then(function (response) {
            return response.data;
        });
    };

    service.getModels = function (selected) {
        return $http.post("/api/car/GetModelsbyYearMake", selected).then(function (response) {
            return response.data;
        });
    };

    service.getTrims = function (selected) {
        return $http.post("/api/car/GetTrims", selected).then(function (response) {
            return response.data;
        });
    };

    service.getCars = function (selected) {
        return $http.post("/api/car/getCars", selected).then(function(response){
            return response.data;
        });
    };

    service.getRecallandPic = function (id) {                     //this function call the NHTSA API + Bing
        //console.log('in Service getRecallandPic', id);
        return $http.post("/api/car/getCar", {id:id}).then(function (response) {
            return response.data;
        });
    };

    return service;
}]);