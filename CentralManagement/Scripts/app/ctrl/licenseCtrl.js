app.controller("licenseCtrl", function ($scope, $http, $timeout) {

    $scope.actCode = {};

    $scope.initDevices = function () {
        $scope.devicesSel = {
            lstServers: [], lstClients: [], isReady: function () {
                return $scope.devicesSel.lstServers.length > 0 && $scope.devicesSel.lstClients.length > 0 
                    && $scope.ActivationCodeOld !== undefined && $scope.ActivationCodeOld !== "";
            }
        };
        var device;
        for (var i = 0, len = $scope.lstClients.length; i < len; i++) {
            device = $scope.lstClients[i];
            device.IsServer = false;
            if (device.IsSelected) {
                $scope.devicesSel.lstClients.push(device.DeviceId);
            }
        }

        for (i = 0, len = $scope.lstServers.length; i < len; i++) {
            device = $scope.lstServers[i];
            device.IsServer = true;
            if (device.IsSelected) {
                $scope.devicesSel.lstServers.push(device.DeviceId);
            }
        }

        $scope.BtnReady = $scope.devicesSel.isReady();

    };

    $scope.getStatusClass = function (codeId) {
        switch (codeId) {
            case 2811:
                return 'orange-bg';
            case 1287:
                return 'yellowgreen-bg';
            case 8123:
                return 'yellowgreen-bg';
            case 2938:
                return 'red-bg';
            case 1028:
                return 'blue-light-bg';
            default:
                return 'red-bg';
        }
    };

    $scope.updateSelected = function (device, lstDevice) {
        if (device.IsSelected) {
            lstDevice.push(device.DeviceId);
        }
        else {
            for (var i = lstDevice.length - 1; i >= 0; i--) {
                var deviceId = lstDevice[i];
                if (deviceId === device.DeviceId)
                    lstDevice.splice(i, 1);
            }
        }

        $scope.BtnReady = $scope.devicesSel.isReady();
    };

    $scope.updateDevice = function (device) {
        if (device.IsServer) {
            $scope.updateSelected(device, $scope.devicesSel.lstServers);
        }
        else {
            $scope.updateSelected(device, $scope.devicesSel.lstClients);
        }
    };

    $scope.select = function (device, url, enable) {
        $scope.working = true;
        device.MsgError = "";
        $http.post(url, { id: device.DeviceId, enable: enable })
            .success(function (data) {
                try {
                    if (data.HasError === false) {
                        device.IsSelected = enable;
                        $scope.updateDevice(device);
                    } else {
                        device.MsgError = data.Message;
                    }
                } catch (e) {
                    $scope.showMsgError(device, "Error del sistema, reinicie e intente de nuevo");
                }
                $scope.working = false;
            })
            .error(function () {
                $scope.showMsgError(device, "Error de red");
                $scope.working = false;
            });
    };

    $scope.showMsgError = function (item, msg) {
        item.MsgError = msg;
        $timeout(function () {
            item.MsgError = "";
        }, 8000);
    };

    $scope.activateLicense = function(url) {
        $scope.working = true;
        $scope.MsgError = "";

        $http.post(url, {actCode: $scope.ActivationCode})
            .success(function (data) {
                try {
                    if (data.HasError === false) {
                        $scope.ActivationCodeOld = $scope.ActivationCode.substring(0, 10) + "***********************************";
                        $scope.ActivationCode = "";
                        $scope.BtnReady = $scope.devicesSel.isReady();
                    } else {
                        $scope.showMsgError($scope, data.Message);
                    }
                } catch (e) {
                    $scope.showMsgError($scope, "Error del sistema, reinicie e intente de nuevo");
                }
                $scope.working = false;
            })
            .error(function () {
                $scope.showMsgError($scope, "Error de red");
                $scope.working = false;
            });
    };

    $scope.askForLicense = function (url) {
        $scope.working = true;
        $scope.MsgError = "";

        $http.post(url)
            .success(function (data) {
                try {
                    if (data.HasError === false) {
                        window.goToUrlMvcUrl(data.Message);
                    } else {
                        $scope.showMsgError($scope, data.Message);
                    }
                } catch (e) {
                    $scope.showMsgError($scope, "Error del sistema, reinicie e intente de nuevo");
                }
                $scope.working = false;
            })
            .error(function () {
                $scope.showMsgError($scope, "Error de red");
                $scope.working = false;
            });
    };
});
