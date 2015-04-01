app.controller('userController', function ($scope, $http, $timeout) {
    $scope.m = {};

    $scope.initRoles = function () {
        if ($scope.m.RoleId <= 0) {
            $scope.m.role = $scope.lstRoles[0];
            $scope.m.RoleId = $scope.m.role.StKey;
        }
        else {
            $scope.m.role = undefined;
            for (var i = 0; i < $scope.lstRoles.length; i++) {
                var role = $scope.lstRoles[i];
                if (role.StKey === $scope.m.RoleId) {
                    $scope.m.role = role;
                    $scope.m.RoleId = $scope.m.role.StKey;
                    break;
                }
            }
            
            if ($scope.m.role === undefined) {
                $scope.m.role = $scope.lstRoles[0];
                $scope.m.roleId = $scope.m.role.StKey;
            }

        }

    };

});