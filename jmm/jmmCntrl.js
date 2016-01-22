(function () {                   //wrap the entire app in an anonymous function - IIFE
    //var app = angular.module('jmmApp');

    app.controller('jmmCntrl', ['$interval', function ($interval) {    //$interval is an ng service

        var scope = this;  //this references this code block
        //console.log('Hi');
        //scope.name = 'jamie';
        //scope.time = new Date();
        //$interval(function () {
        //    scope.time = new Date();
        //}, 1000);

        //scope.color = '';
        scope.turn = 'X';
       
        scope.arr = ['', '', '', '', '', '', '', '', ''];
       
        scope.move = (function (a) {
            console.log('in move');
            
            scope.arr[a] = scope.turn;
            scope.turn = scope.turn === 'X' ? 'O' : 'X';
        });

    }]);
})();