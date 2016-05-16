window.angJsDependencies.push("colorpicker.module");

app.controller('franchiseAssignController', function ($scope) {
    $scope.m = {};
    $scope.vm = { stores: [] };

    $scope.init = function () {
        try {
            if (!$scope.m || !$scope.m.LstStoresByFranchise || $scope.m.LstStoresByFranchise.length === 0) {
                $scope.bStore = false;
                return;
            }
        } catch (e) {
            $scope.bStore = false;
            return;
        }

        $scope.bStore = true;
    };

    $scope.createCoordinates = function () {
        var bound = window.appGlobalMap.getBounds();
        var sizLat = Math.abs(bound.H.H - bound.H.j) / 8;
        var sizLng = Math.abs(bound.j.H - bound.j.j) / 8;
        var lat = window.appGlobalMap.center.lat();
        var lng = window.appGlobalMap.center.lng();

        var corners = [];
        corners.push([lat - sizLat, lng - sizLng]);
        corners.push([lat + sizLat, lng - sizLng]);
        corners.push([lat + sizLat, lng + sizLng]);
        corners.push([lat - sizLat, lng + sizLng]);

        var coordinates = [];

        for (var i = 0; i < corners.length; i++) {
            var position = new google.maps.LatLng(corners[i][0], corners[i][1]);
            coordinates.push(position);
        }

        return coordinates;
    };

    $scope.clearSelected = function () {
        for (var i = 0; i < $scope.vm.stores.length; i++) {
            var store = $scope.vm.stores[i];
            for (var j = 0; j < store.polygons.length; j++) {
                var polygon = store.polygons[j];
                polygon.setOptions({
                    fillOpacity: 0.1
                });
            }
        }
    };

    function setStoreByPoly(currPoly) {
        for (var i = 0; i < $scope.vm.stores.length; i++) {
            var store = $scope.vm.stores[i];
            for (var j = 0; j < store.polygons.length; j++) {
                if (currPoly === store.polygons[j]) {
                    $scope.$apply(function () {
                        $scope.selectStore(store.item.IdKey);
                    });
                    return;
                }
            }
        }
    };

    $scope.createPolygon = function (coordinates, color) {
        var polygon = new google.maps.Polygon({
            map: window.appGlobalMap,
            paths: coordinates,
            strokeColor: color,
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: color,
            fillOpacity: 0.1,
            zIndex: -1,
            draggable: true,
            editable: true
        });

        polygon.addListener('click', function () {
            $scope.clearSelected();
            polygon.setOptions({
                fillOpacity: 0.5
            });
            $scope.currPolygonSel = polygon;
            setStoreByPoly(polygon);
        });

        polygon.addListener('dblclick', function (e) {
            if (e.vertex > -1) {
                var path = polygon.getPath();
                path.removeAt(e.vertex);
                if (path.length === 2) {
                    $scope.$apply(function() {
                        $scope.currPolygonSel = polygon;
                        $scope.delCoverage();
                    });
                    
                }
            }
        });

        return polygon;
    };

    $scope.findItem = function (catalog, id) {
        for (var i = 0; i < catalog.length; i++) {
            var row = catalog[i];
            if (row.item.IdKey === id)
                return row;
        }
        return undefined;
    };

    $scope.addCoverage = function () {

        var store = $scope.findItem($scope.vm.stores, $scope.selStore.IdKey);
        var polygon = $scope.createPolygon($scope.createCoordinates(), (!store ? $scope.selColor : store.color));

        if (!store) {
            $scope.vm.stores.push({
                polygons: [polygon],
                color: $scope.selColor,
                item: $scope.selStore
            });
        } else {
            store.polygons.push(polygon);
        }

    };

    $scope.selectStore = function (id) {
        for (var i = 0; i < $scope.vm.stores.length; i++) {
            var store = $scope.vm.stores[i];
            if (store.item.IdKey === id) {
                $scope.selStore = store.item;
                $scope.selColor = store.color;
            }
        }
    };

    $scope.changeColor = function () {
        if (!$scope.selStore)
            return;

        for (var i = 0; i < $scope.vm.stores.length; i++) {
            var store = $scope.vm.stores[i];

            if ($scope.selStore.IdKey !== store.item.IdKey)
                continue;

            store.color = $scope.selColor;

            for (var j = 0; j < store.polygons.length; j++) {
                var polygon = store.polygons[j];
                polygon.setOptions({
                    fillColor: $scope.selColor,
                    strokeColor: $scope.selColor
                });
            }
        }
    };

    $scope.delCoverage = function () {
        if (!$scope.currPolygonSel) return;
        var currPoly = $scope.currPolygonSel;
        $scope.currPolygonSel = undefined;

        for (var i = 0; i < $scope.vm.stores.length; i++) {
            var store = $scope.vm.stores[i];
            for (var j = store.polygons.length - 1; j > -1; j--) {
                if (currPoly === store.polygons[j]) {
                    currPoly.setMap(null);
                    store.polygons.splice(j, 1);

                    if (store.polygons.length === 0)
                        $scope.vm.stores.splice(i, 1);

                    return;
                }
            }
        }
    };

    $scope.save = function() {

    };

});