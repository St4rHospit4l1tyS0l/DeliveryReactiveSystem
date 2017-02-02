app.controller('reportClientOrderController', function ($scope, $http, $timeout) {
    $scope.m = {};

    $scope.init = function () {
        $scope.m.FranchiseId = -1;
        $scope.m.franchise = window.initCatalog($scope.lstFranchises, $scope.m.FranchiseId);
        $scope.onFranchiseChange();
    };

    $scope.onFranchiseChange = function () {

        var franchiseId = $scope.m.FranchiseId;
        var lstFranchiseStores = [];

        if (franchiseId == -1) {
            lstFranchiseStores = $scope.lstFranchiseStoresFull;
        } else {
            lstFranchiseStores.push($scope.lstFranchiseStoresFull[0]);
            for (var i = 1, len = $scope.lstFranchiseStoresFull.length; i < len; i++) {
                var item = $scope.lstFranchiseStoresFull[i];
                if (item.IdKey == franchiseId)
                    lstFranchiseStores.push(item);
            }
        }

        $scope.lstFranchiseStores = lstFranchiseStores;
        $scope.m.franchiseStore = $scope.lstFranchiseStores[0];
        $scope.m.FranchiseStoreId = $scope.m.franchiseStore.Key;
    };

    $scope.search = function (url) {
        var m = $scope.m;
        $scope.working = true;

        if (!m.endDate)
            m.endDate = m.startDate;

        if (m.startDate > m.endDate)
            m.endDate = m.startDate;

        $http({
            method: 'POST',
            url: url,
            data: { Id: $scope.m.FranchiseId, SecondId: $scope.m.FranchiseStoreId, StartRequestDate: m.startDate, EndRequestDate: m.endDate }
        }).success($scope.handleSuccess)
            .error($scope.handleError);
    };
    
    $scope.handleSuccess = function (data) {
        $scope.working = false;

        if (data === undefined || data === null) {
            $scope.handleError();
        }
        else if (data.HasError === true) {
            $scope.msgErr = data.Message;
            $scope.hideMsgErr();
        }
        else if (data.HasError === false) {
            $scope.lstResult = data.Data;
        }
    };

    $scope.hideMsgErr = function () {
        $timeout(function () {
            $scope.msgErr = "";
        }, 10000);
    };

    $scope.handleError = function () {
        $scope.working = false;
        $scope.msgErr = "Ocurrió un error de red. Por favor intente más tarde";
        $scope.hideMsgErr();
    };
});