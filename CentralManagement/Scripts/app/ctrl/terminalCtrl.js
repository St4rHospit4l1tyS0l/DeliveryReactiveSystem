app.controller('terminalController', function ($scope, $http) {
    $scope.sel = {};

    $scope.init = function () {
        $scope.sel.SelFranchise = $scope.lstFranchises[0];
    };  
    
    $scope.add = function (url) {
        var data = {
            InfoClientTerminalId: $scope.m.InfoClientTerminalId,
            Id: $scope.sel.Id,
            FranchiseId: $scope.sel.SelFranchise.StKey,
            Ip: $scope.sel.PosIpAddress
        };
        
        $http.post(url, data);
    };

});