app.controller('loginController', function ($scope, $http, $timeout) {
    $scope.m = {};
    $scope.isLogin = true;

    $scope.login = function (formId, urlToGo) {
        var data = $(formId).serialize();
        $scope.m.password = "";
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

    $scope.doForget = function (formId, urlToGo) {
        var data = $(formId).serialize();
        $scope.m.emailForgot = "";
        $scope.working = true;
        $http({
            method: 'POST',
            url: urlToGo,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            data: data
        }).success($scope.handleSuccessForgot)
            .error($scope.handleErrorForgot);
    };


    $scope.handleSuccessForgot = function (data) {

        if (data === undefined || data === null) {
            $scope.handleErrorForgot();
        }
        else if (data.IsSuccess === false) {
            $scope.msgErrForgot = data.Message;
            $scope.hideMsgForgot();
        }
        else if (data.IsSuccess === true) {
            $scope.msgSucForgot = data.Message;
            $scope.hideMsgForgot();
        }

    };

    $scope.handleErrorForgot = function () {
        $scope.working = false;
        $scope.msgErrForgot = "Ocurrió un error de red. Por favor intente más tarde";
        $scope.hideMsgForgot();
    };
    
    $scope.hideMsgForgot = function () {
        $timeout(function () {
            $scope.msgErrForgot = "";
            $scope.msgSucForgot = "";
        }, 10000);
    };

    //$scope.forgotView = function() {
    //    $scope.isLogin = false;
    //};
});