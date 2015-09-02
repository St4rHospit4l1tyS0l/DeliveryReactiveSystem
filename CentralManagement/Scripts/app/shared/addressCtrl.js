window.angJsDependencies.push('ui.bootstrap');

app.controller('addressController', function ($scope, focusFact) {

    $scope.m = {};

    $scope.zipCodes = function(val, url) {
        debugger;
        return $http.get(url, {
            params: { code: val }
        }).then(function (response) {
            try {
                debugger;
                return response.data.map(function (item) {
                    item.ViewName = item.ZipCode + " (" + item.Location + ")";
                    return item;
                });
            } catch (e) {
                return [];
            }
        });
    };

    $scope.onSelect = function(idElement) {
        $scope.m.ZipCode = $scope.zipcode.ZipCode;
        $scope.m.State = $scope.zipcode.State;
        $scope.m.Municipality = $scope.zipcode.Municipality;
        $scope.m.Location = $scope.zipcode.Location;
        focusFact(idElement);
    };

    $scope.$on('onClearData', function () {
        $scope.m = {};
        $scope.zipcode = undefined;
    });

});