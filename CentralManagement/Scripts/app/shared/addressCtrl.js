window.angJsDependencies.push('ui.bootstrap');

app.controller('addressController', function ($scope, focusFact, $http) {

    $scope.m = {};

    $scope.init = function() {
        if (!($scope.ini))
            return;

        var ini = $scope.ini;
        $scope.m.ZipCode = { IdKey: ini.ZipCodeId, Value: ini.ZipCode };
        $scope.m.Country = { IdKey: ini.CountryId, Value: ini.Country };
        $scope.m.RegionA = { IdKey: ini.RegionArId, Value: ini.RegionA };
        $scope.m.RegionB = { IdKey: ini.RegionBrId, Value: ini.RegionB };
        $scope.m.RegionC = { IdKey: ini.RegionCrId, Value: ini.RegionC };
        $scope.m.RegionD = { IdKey: ini.RegionDrId, Value: ini.RegionD };
        $scope.m.MainAddress = ini.MainAddress;
        $scope.m.NumExt = ini.NumExt;
        $scope.m.Reference = ini.Reference;
    };

    $scope.zipCodes = function(val, url) {
        return $http.post(url, {
            code: val
        }).then(function (response) {
            try {
                return response.data.Data.map(function (item) {
                    item.ViewName = item["ZipCode"].Value + " (" + item[$scope.region].Value + ")";
                    return item;
                });
            } catch (e) {
                return [];
            }
        });
    };

    $scope.onSelect = function (idElement) {
        try {
            var m = $scope.m;
            m.Country = $scope.zipcodeSel.Country ? $scope.zipcodeSel.Country : {};
            m.RegionA = $scope.zipcodeSel.RegionA ? $scope.zipcodeSel.RegionA : {};
            m.RegionB = $scope.zipcodeSel.RegionB ? $scope.zipcodeSel.RegionB : {};
            m.RegionC = $scope.zipcodeSel.RegionC ? $scope.zipcodeSel.RegionC : {};
            m.RegionD = $scope.zipcodeSel.RegionD ? $scope.zipcodeSel.RegionD : {};
            m.ZipCode = $scope.zipcodeSel.ZipCode ? $scope.zipcodeSel.ZipCode : {};
        } catch(e) {
            console.error(e);
        } 
        focusFact(idElement);
    };

    $scope.$on('onClearData', function () {
        $scope.m = {};
        $scope.zipcode = undefined;
    });

});