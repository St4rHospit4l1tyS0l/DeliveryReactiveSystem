app.controller('storeController', function ($scope) {
    $scope.m = {};

    $scope.init = function() {
        $scope.m.franchise = window.initCatalog($scope.lstFranchises, $scope.m.FranchiseId);
        $scope.m.FranchiseId = $scope.m.franchise.StKey;
    };

});