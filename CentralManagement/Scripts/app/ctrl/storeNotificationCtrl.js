window.angJsDependencies.push('ui.bootstrap');

app.controller('storeNotificationController', function ($scope, $http) {
    $scope.m = {};
    $scope.categories = [];

    $scope.init = function () {
        try {
            for (var i = 0; i < $scope.m.CatMessages.length; i++) {
                var category = $scope.m.CatMessages[i];
                if (i === 0)
                    $scope.m.catSelected = category;
                category.items = $scope.m.Items.filter(function (item) {
                    if (item.CategoryMessageId === category.Id)
                        return true;
                    return false;
                });
                $scope.categories.push(category);
            }
        } catch (e) {
            console.log(e);
        }
    };

    $scope.$on('fileUploadSuccess', function (event, data) {
        $scope.m.resourceName = data.ResourceName;
    });

    $scope.deleteNotification = function (url, item) {
        $scope.working = true;
        $scope.MsgError = "";

        $http.post(url, item).then(function (res) {
            onSuccess(res.data, item, false);
        }, onError);
    };

    $scope.addNotification = function (url) {
        $scope.working = true;
        $scope.MsgError = "";
        
        var item = {
            CategoryMessageId: $scope.m.catSelected.Id,
            FranchiseStoreId: $scope.m.FranchiseStoreId,
            IsIndefinite: $scope.m.isIndefinite,
            Message: $scope.m.message,
            Resource: $scope.m.resourceName
        };

        $http.post(url, item).then(function (res) {
            onSuccess(res.data, item, true);
        }, onError);
    };

    function onSuccess(data, item, bHasToInsert) {
        $scope.working = false;
        if (data.HasError) {
            $scope.MsgError = data.Message;
            return;
        }

        if (bHasToInsert)
            insertDataOnCategory(data.Data, item);
        else
            deleteDataOnCategory(data.Data, item.CategoryMessageId);
    }

    function insertDataOnCategory(message, item) {
        for (var i = 0; i < $scope.categories.length; i++) {
            var category = $scope.categories[i];

            if (category.Id === item.CategoryMessageId) {
                category.items.push({
                    Message: message,
                    CategoryMessageId: item.CategoryMessageId,
                    FranchiseStoreId: item.FranchiseStoreId,
                    IsIndefinite: item.IsIndefinite,
                    Resource: item.Resource
                });
                break;
            }
        }
    }


    function deleteDataOnCategory(message, categoryMessageId) {
        for (var i = 0; i < $scope.categories.length; i++) {
            var category = $scope.categories[i];

            if (category.Id === categoryMessageId) {
                for (var j = 0; j < category.items.length; j++) {
                    if (category.items[j].Message !== message)
                        continue;
                    category.items.splice(j, 1);
                    break;
                }
                break;
            }
        }
    }

    function onError(res) {
        $scope.working = false;
        $scope.MsgError = res;
    }

    $scope.notifications = function (val, url) {
        return $http.post(url, {
            notification: val,
            limit: 20
        }).then(function (response) {
            try {
                if (response.data.HasError)
                    return [];
                return response.data.Data;
            } catch (e) {
                return [];
            }
        });
    };

});