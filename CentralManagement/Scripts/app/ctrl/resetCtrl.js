app.controller('resetController', function ($scope, $http, $timeout) {
    $scope.m = {};

    $scope.recovery = function (formId, urlToGo) {
        var data = $(formId).serialize();
        $scope.m.password = "";
        $scope.m.confirm = "";
        $scope.working = true;
        $http({
            method: 'POST',
            url: urlToGo,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            data: data
        }).success($scope.handleSuccess)
            .error($scope.handleError);
    };

    $scope.handleSuccess = function (data) {
        $scope.working = false;

        if (data === undefined || data === null) {
            $scope.handleError();
        }
        else if (data.IsSuccess === false) {
            $scope.msgErr = data.Message;
            $scope.hideMsgErr();
        }
        else if (data.IsSuccess === true) {
            window.location.replace(data.UrlToGo);
        }

    };

    $scope.handleError = function () {
        $scope.working = false;
        $scope.msgErr = "Ocurrió un error de red. Por favor intente más tarde";
        $scope.hideMsgErr();
    };

    $scope.hideMsgErr = function() {
        $timeout(function() {
            $scope.msgErr = "";
        }, 10000);
    };

});