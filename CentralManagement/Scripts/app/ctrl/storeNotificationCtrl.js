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
                category.items = $scope.m.Items.filter(function(item) {
                    if (item.CategoryMessageId === category.Id)
                        return true;
                    return false;
                });
                $scope.categories.push(category);
            }
        } catch(e) {
            console.log(e);
        } 
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
        }).then(function(res) {
            onSuccess(res.data, categoryMessageId, franchiseStoreId);
        }, onError);
    };

    function onSuccess(data, categoryMessageId, franchiseStoreId) {
        $scope.working = false;
        if (data.HasError) {
            $scope.MsgError = data.Message;
            return;
        }
        
        insertDataOnCategory(data.Data, categoryMessageId, franchiseStoreId);
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

    function onError(res){
        $scope.working = false;
        $scope.MsgError = res;
    }
});