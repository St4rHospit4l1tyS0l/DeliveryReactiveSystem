app.controller('orderViewController', function ($scope) {
    $scope.m = {};

    $scope.init = function () {

        if ($scope.m.IsMap) {
            window.modelCl = $scope.m;
            window.initParamsMaps();
        }
    };


    $scope.padding = function(spaces) {
        var str = '';
        for (var i = 0; i < spaces; i++) {
            str += "-";
        }
        return str;
    };
});