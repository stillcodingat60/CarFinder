(function () {
    var app = angular.module("jmmApp");

    app.controller('carFinderCntl', ['carSvc', '$uibModal', function (carSvc, $uibModal) {
        var scope = this;
        scope.years = [];
        scope.makes = [];
        scope.models = [];
        scope.trims = [];
        scope.selected = {
            year: '',
            make: '',
            model: '',
            trim: ''
        }
        scope.cars = []
        scope.id = '';

        scope.open = function (id) {
            //console.log("Id in open "+id)             
            var modalInstance = $uibModal.open({                
                animation: true,
                templateUrl: 'carModal.html',               //where all the markup for the modal goes
                controller: 'carModalCtrl as cm',           //modal controller
                size: 'lg',
                resolve: {                                 
                    car: function() {                       //function is run and the results stored in car:
                        return carSvc.getRecallandPic(id)
                    }
                 }
            })
        };

        scope.getYears = function () {             
            carSvc.getYears().then(function (data) {
                scope.years = data;
                scope.makes = [];
                scope.models = [];
                scope.trims = [];
                scope.selected.year = '';
                scope.selected.make = '';
                scope.selected.model = '';
                scope.selected.trim = '';
            });
        };
       
        scope.getMakes = function () {
            carSvc.getMakes(scope.selected).then(function (data) {
                scope.makes = data;
                scope.models = [];
                scope.trims = [];
                scope.selected.make = '';
                scope.selected.model = '';
                scope.selected.trim = '';
                scope.getCars();
            });
        };

        scope.getModels = function () {
            carSvc.getModels(scope.selected).then(function (data) {
                scope.models = data;
                scope.trims = [];
                scope.selected.model = '';
                scope.selected.trim = '';
                scope.getCars();
            });
        };

        scope.getTrims = function () {
            carSvc.getTrims(scope.selected).then(function (data) {
                scope.trims = data;
                scope.getCars();
            });
        };

        scope.getCars = function () {
            carSvc.getCars(scope.selected).then(function (data) {
                scope.cars = data;
            });
        };

        scope.getRecallandPic = function (id) {
            console.log('in getRecallandPic', id);
            carSvc.getRecallandPic(id).then(function (data) {
                scope.cars = data;
            });
        };

        scope.getYears();
    }])

    app.controller('carModalCtrl', function ($uibModalInstance, car) { // add car later to params

        var scope = this;
        scope.n = 0;
        scope.car = car;

        scope.ok = function () {
            $uibModalInstance.close();
        };

        scope.cancel = function () {
            $uibModalInstance.dismiss();
        };
    })
})();
