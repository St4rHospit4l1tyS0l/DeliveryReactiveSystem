app.controller("reportController", function ($scope) {

    $scope.checkErr = function (startDate, endDate) {
        $scope.errorMsg = 'test';
        if (new Date(startDate) > new Date(endDate)) {
            $scope.errorMsg = 'La fecha final debe ser mayor a la fecha inicial';
            return false;
        }
    };


});