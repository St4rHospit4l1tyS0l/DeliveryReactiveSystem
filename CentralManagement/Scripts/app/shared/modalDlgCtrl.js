app.controller('modalDlgController', function ($scope, $q, sharedSvc) {

    $scope.showInnScope = function(data, urlToGo, divToAppendId, dlgUpsertId) {
        return $scope.show(data, urlToGo, divToAppendId, dlgUpsertId, true);
    };

    $scope.show = function (data, urlToGo, divToAppendId, dlgUpsertId, innerScp) {
        if (innerScp === true) { $scope.working = true; } else { $scope.$apply(function () { $scope.working = true; }); }
        var def = $q.defer();
        divToAppendId = divToAppendId || "#dlgUpsert";
        dlgUpsertId = dlgUpsertId || "#dlgUpModalId";

        var settings = {
            dataType: "html",
            type: "POST",
            url: urlToGo,
            data: data,
            success: function (resp) {
                $(divToAppendId).empty().append(resp);
                var scope = angular.element($(dlgUpsertId)).scope();
                scope.Model.def = def;
                scope.$apply();
                $scope.$apply(function () { $scope.working = false; });
            },
            error: function() {
                sharedSvc.showMsg(
                    {
                        Title: "Error de red",
                        Message: "<strong>No fue posible conectarse al servidor</strong> <br/><br/>Por favor intente más tarde",
                        Type: "danger"
                    }).then(function () { def.reject({ isError: true }); });
                $scope.$apply(function () { $scope.working = false; });
            }
        };

        $.ajax(settings);
        return def.promise;
    };

    $scope.doConfirm = function (data, urlToGo, innerScp) {
        var def = $q.defer();
        sharedSvc.showConf({ Title: "Confirmación de Servicio", Message: "¿Está seguro que desea de confirmar el uso del servicio?", Type: "warning" }).
            then(function () {
                $scope.doPost(data, urlToGo, def, innerScp);
            }, def.reject);
        return def.promise;
    };
    
    $scope.doCancelDocument = function (data, urlToGo, folio) {
        var def = $q.defer();
        sharedSvc.showConf({ Title: "Confirmación de cancelación de documento", Message: "¿Está seguro que desea cancelar el documento con folio "+folio+"?", Type: "warning" }).
            then(function () {
                $scope.doPost(data, urlToGo, def);
            }, def.reject);
        return def.promise;
    };

    $scope.sendPostAction = function (data, urlToGo, innerScp, showSuccess) {
        var def = $q.defer();
        $scope.doPost(data, urlToGo, def, innerScp, showSuccess);
        return def.promise;
    };

    $scope.doObsolete = function(data, urlToGo) {
        var def = $q.defer();
        sharedSvc.showConf({ Title: "Eliminar registro", Message: "¿Está seguro de que desea eliminar el registro?", Type: "danger" }).
            then(function() {
                $scope.doPost(data, urlToGo, def);
            }, def.reject);
        return def.promise;
    };

    $scope.doPost = function (data, urlToGo, def, innerScp, showSuccess) {
        if (innerScp === true) { $scope.working = true; } else { $scope.$apply(function () { $scope.working = true; }); }
        var settings = {
            dataType: "json",
            type: "POST",
            url: urlToGo,
            data: data,
            success: function (resp) {
                $scope.$apply(function () { $scope.working = false; });
                if (resp.HasError === true) {
                    sharedSvc.showMsg(
                        {
                            Title: resp.Title,
                            Message: resp.Message,
                            Type: "danger"
                        }).then(function () { def.reject({ isError: true }); });
                }
                else if (showSuccess) {
                    sharedSvc.showMsg(
                    {
                        Title: resp.Title,
                        Message: resp.Message,
                        Type: "info"
                    }).then(function () { def.reject({ isError: true }); });
                    def.resolve();
                } else {
                    def.resolve();
                }
            },
            error: function () {
                $scope.$apply(function () { $scope.working = false; });
                sharedSvc.showMsg(
                    {
                        Title: "Error de red",
                        Message: "<strong>No fue posible conectarse al servidor</strong> <br/><br/>Por favor intente más tarde",
                        Type: "danger"
                    }).then(function () { def.reject({ isError: true }); });
            }
        };

        $.ajax(settings);
    };

});

