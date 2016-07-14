app.controller('reportTimeSaleController', function ($scope, $http, $timeout) {
    $scope.m = {};

    $scope.searchDaysByRange = function (url) {
        var m = $scope.m;
        $scope.working = true;

        if (!m.endDate)
            m.endDate = m.startDate;
        $http({
            method: 'POST',
            url: url,
            data: { StartRequestDate: m.startDate, EndRequestDate: m.endDate }
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

    $scope.setByStatus = function (status) {
        switch (status) {
            case 'None':
            case 'PreDelay':
            case 'InDelay':
                return "white";
            case 'KitchenDelay':
            case 'Cooking':
            case 'Prepared':
                return "warning";
            case 'InTransit':
            case 'Fulfilled':
                return "primary";
            case 'Canceled':
                return "danger";
            case 'Closed':
                return "success";
            default:
                return "inverse";
        }

    };

    $scope.isSameSale = function (i, records, field) {
        return (i === 0 || records[i].LastStatus !== records[i - 1].LastStatus || records[i][field] === records[i - 1][field]);
    };

    $scope.isUp = function (i, records, field) {
        return records[i][field] > records[i - 1][field];
    };

    $scope.colorBySale = function (i, records, field) {
        if ($scope.isSameSale(i, records, field))
            return "muted";

        return $scope.isUp(i, records, field) ? "navy" : "danger";
    };

    $scope.iconBySale = function (i, records, field) {
        if ($scope.isSameSale(i, records, field))
            return "caret-right";

        return $scope.isUp(i, records, field) ? "caret-up" : "caret-down";
    };

    $scope.handleError = function () {
        $scope.working = false;
        $scope.msgErr = "Ocurrió un error de red. Por favor intente más tarde";
        $scope.hideMsgErr();
    };


    $scope.hideMsgErr = function () {
        $timeout(function () {
            $scope.msgErr = "";
        }, 10000);
    };

    $scope.setRowBinaryClass = function (i, items, colName, colNew, evenLabel, oddLabel) {
        if (i === 0) {
            items[0][colNew] = {pos: i, val:0};
            result = evenLabel;
        } else {
            if (items[0][colNew].pos !== i) {
                items[0][colNew].pos = i;
                
                if (items[i - 1][colName] != items[i][colName]) {
                    items[0][colNew].val = (items[0][colNew].val + 1) % 2;
                }
            }
            var result = (items[0][colNew].val === 0 ? evenLabel : oddLabel);
        }
        return result;
    };
    
    var iPos;
    $scope.setPosition = function (i, items, colName) {
        if (i === 0) {
            iPos = 1;
            return iPos;
        }

        if (items[0].iVal != i) {
            items[0].iVal = i;
            iPos++;
        }
        
        if (items[i - 1][colName] != items[i][colName]) {
            iPos = 1;
        }
        return iPos;
    };
    
});