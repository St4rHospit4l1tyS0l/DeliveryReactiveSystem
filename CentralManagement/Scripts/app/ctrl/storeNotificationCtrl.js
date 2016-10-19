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

    $scope.deleteNotification = function (url, item) {
        $scope.working = true;
        $scope.MsgError = "";

        var categoryMessageId = item.CategoryMessageId;
        var franchiseStoreId = item.FranchiseStoreId;
        $http.post(url, {
            franchiseStoreId: franchiseStoreId,
            categoryMessageId: categoryMessageId,
            notification: item.Message
        }).then(function (res) {
            onSuccess(res.data, categoryMessageId, franchiseStoreId, false);
        }, onError);
    };

    $scope.addNotification = function (url) {
        $scope.working = true;
        $scope.MsgError = "";

        var categoryMessageId = $scope.m.catSelected.Id;
        var franchiseStoreId = $scope.m.FranchiseStoreId;
        $http.post(url, {
            franchiseStoreId: franchiseStoreId,
            categoryMessageId: categoryMessageId,
            notification: $scope.m.notification
        }).then(function (res) {
            onSuccess(res.data, categoryMessageId, franchiseStoreId, true);
        }, onError);
    };

    function onSuccess(data, categoryMessageId, franchiseStoreId, bHasToInsert) {
        $scope.working = false;
        if (data.HasError) {
            $scope.MsgError = data.Message;
            return;
        }

        if (bHasToInsert)
            insertDataOnCategory(data.Data, categoryMessageId, franchiseStoreId);
        else
            deleteDataOnCategory(data.Data, categoryMessageId);
    }

    function insertDataOnCategory(notification, categoryMessageId, franchiseStoreId) {
        for (var i = 0; i < $scope.categories.length; i++) {
            var category = $scope.categories[i];

            if (category.Id === categoryMessageId) {
                category.items.push({
                    Message: notification,
                    CategoryMessageId: categoryMessageId,
                    FranchiseStoreId: franchiseStoreId
                });
                break;
            }
        }
    }


    function deleteDataOnCategory(notification, categoryMessageId) {
        for (var i = 0; i < $scope.categories.length; i++) {
            var category = $scope.categories[i];

            if (category.Id === categoryMessageId) {
                for (var j = 0; j < category.items.length; j++) {
                    if (category.items[j].Message !== notification)
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