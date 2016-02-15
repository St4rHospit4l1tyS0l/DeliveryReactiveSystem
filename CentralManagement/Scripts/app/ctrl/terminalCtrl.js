app.controller('terminalController', function ($scope) {
    $scope.m = {};

    $scope.init = function () {
        $scope.m.selFranchise = $scope.lstFranchises[0];
        console.log("ss", $scope.m.selFranchise);
    };

});