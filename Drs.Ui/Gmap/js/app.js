window.angJsDependencies = [];
window.constMainApp = 'gMap';
var app = angular.module(window.constMainApp, window.angJsDependencies);

app.controller('gMapController', function ($scope, $timeout) {

    var marker, autocomplete, infoWindow;
    $scope.addresses = [];
    $scope.m = {};
    $scope.c = {};
    $scope.isReady = false;

    var init = function () {

        try {
            $scope.loadingMap = false;
            var infoMv = window.external.GetInfoAddress();
            var infoM = JSON.parse(infoMv);
            var address = infoM.AddressMapInfo;
            var lastConfig = JSON.parse(infoM.LastConfig);
            $scope.storesCoverage = JSON.parse(infoM.StoresCoverage);
            
            if (address) {
                lastConfig.lat = address.Position.Lat;
                lastConfig.lng = address.Position.Lng;
                lastConfig.zoom = 17;
                $scope.m.PlaceId = address.PlaceId;
                $scope.m.Address = address.Address;
            }

            window.geocoder = new google.maps.Geocoder();
            var mapOptions = {
                zoom: lastConfig.zoom,
                center: new google.maps.LatLng(lastConfig.lat, lastConfig.lng),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            window.gDrsMap = new google.maps.Map(window.document.getElementById('map'), mapOptions);

            var input = window.document.getElementById('pac-input');
            autocomplete = new google.maps.places.Autocomplete(input, {
                componentRestrictions: { country: 'MX' }
            });
            autocomplete.bindTo('bounds', window.gDrsMap);
            window.gDrsMap.controls[google.maps.ControlPosition.TOP_CENTER].push(input);

            window.gDrsMap.addListener('click', setMarkerInPosition);
            autocomplete.addListener('place_changed', placeChanged);

            infoWindow = new google.maps.InfoWindow;
            
            marker = new google.maps.Marker({
                map: window.gDrsMap,
                draggable: true,
                animation: google.maps.Animation.DROP,
                position: new google.maps.LatLng(lastConfig.lat, lastConfig.lng),
                visible: false
            });
            
            
            marker.addListener('click', getAddresses);
            marker.addListener('dragend', getAddresses);

            drawStoresCoverage();

            if (address) {
                marker.setVisible(true);
                calculateCoverage(marker.getPosition());
            }

            $scope.$apply(function () {
                $scope.isReady = true;
                $scope.c = infoM.Controls;
            });

        } catch (e) {
            $scope.$apply(function () {
                showError("La franquicia no está configurada. Se sucitó el siguiente error: " + e);
            });
        }
    };

    function drawStoresCoverage() {
        var iStores = $scope.storesCoverage;
        var position, stores = [];
        var i, j, k;

        for (i = iStores.length - 1; i > -1; i--) {
            var iStore = iStores[i];
            var polygons = [];
            for (j = iStore.paths.length - 1; j > -1; j--) {
                var path = iStore.paths[j].path;
                var coordinates = [];
                for (k = path.length - 1; k > -1; k--) {
                    var vertex = path[k];
                    position = new google.maps.LatLng(vertex.lat, vertex.lng);
                    coordinates.push(position);
                }
                var polygon = createPolygon(coordinates, iStore.color, iStore.name);
                polygons.push(polygon);
            }

            var store = {
                id: iStore.id,
                color: iStore.color,
                name: iStore.name,
                polygons: polygons
            };

            stores.push(store);
        }

        $scope.utilStores = stores;
    }

    function pinSymbol(color) {
        return {
            path: 'M 0,0 C -2,-20 -10,-22 -10,-30 A 10,10 0 1,1 10,-30 C 10,-22 2,-20 0,0 z M -2,-30 a 2,2 0 1,1 4,0 2,2 0 1,1 -4,0',
            fillColor: color,
            fillOpacity: 1,
            strokeColor: '#000',
            strokeWeight: 2,
            scale: 1,
        };
    }
    
    function createMarker(position, title, color, store) {
        store.marker = new google.maps.Marker({
            map: window.gDrsMap,
            draggable: true,
            animation: google.maps.Animation.DROP,
            position: position,
            visible: true,
            icon: pinSymbol(color),
            title: title
        });
    }

    function calculateCoverage(pos) {
        var stores = $scope.utilStores, i, j;
        var coverageStore = [];

        for (i = 0; i < stores.length; i++) {
            var store = stores[i];
            var polygons = store.polygons;
            for (j = 0; j < polygons.length; j++) {
                var polygon = polygons[j];
                if (google.maps.geometry.poly.containsLocation(pos, polygon)) {
                    store.stColor = { 'background-color': store.color };
                    coverageStore.push(store);
                    break;
                }
            }
        }
        $scope.coverageStore = coverageStore;
    }

    function setMarkerInPosition(e) {
        var location = e.latLng;
        marker.setPosition(new google.maps.LatLng(location.lat(), location.lng()));
        marker.setVisible(true);
        getAddresses();
    }

    function placeChanged() {
        try {
            var place = autocomplete.getPlace();
            if (!place.geometry) {
                return;
            }

            window.gDrsMap.setCenter(place.geometry.location);
            window.gDrsMap.setZoom(17);

            var location = window.gDrsMap.getCenter();
            marker.setPosition(new google.maps.LatLng(location.lat(), location.lng()));
            marker.setVisible(true);

            var currComp = place.address_components;

            if (currComp[0].types[0] === 'street_number' || currComp[0].types[0] === 'route') {
                $scope.$apply(function() {
                    $scope.allResults = [];
                    $scope.addresses = [];
                    $scope.onSelectAddress(place);
                    $scope.m.PlaceId = place.place_id;
                });
            } else {
                getAddresses();
            }

        } catch(e) {
            $scope.$apply(function () {
                showError("La franquicia no está configurada. Se sucitó el siguiente error: " + e);
            });
        } 
    }

    function showError(e) {
        $scope.error = e;
        $timeout(function () {
            $scope.error = "";
        }, 20000);
    }

    function showInfo(m) {
        $scope.msgInfo = m;
        $timeout(function () {
            $scope.error = "";
        }, 20000);
    }
    

    function getAddresses() {
        window.geocoder.geocode({ location: marker.getPosition() }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                $scope.$apply(function () {
                    $scope.allResults = results;
                    $scope.addressSelected = undefined;
                    $scope.addresses = [];
                    var tInfo = {};
                    for (var i = 0; i < results.length; i++) {
                        var row = results[i];
                        if (i === 0) {
                            $scope.onSelectAddress(row);
                            $scope.m.PlaceId = row.place_id;
                        }
                        if (tInfo[row.geometry.location_type] > 1)
                            continue;
                        tInfo[row.geometry.location_type] = (tInfo[row.geometry.location_type] === undefined ? 1 : tInfo[row.geometry.location_type] + 1);
                        $scope.addresses.push(row);
                    }
                });
            } else {
                $scope.$apply(function () {
                    showError("Fallo en la geolocalización debido a: " + e);
                });
            }
        });
    }

    $scope.iconByLocationType = function (type) {
        switch (type) {
            case "ROOFTOP":
                return 'ok';
            case "RANGE_INTERPOLATED":
                return 'road';
            case "GEOMETRIC_CENTER":
                return 'screenshot';
            case "APPROXIMATE":
                return 'fullscreen';
            default:
                return 'remove-sign';
        }
    };

    $scope.bgColorByLocationType = function (type) {
        switch (type) {
            case "ROOFTOP":
                return 'bk-color-green';
            case "RANGE_INTERPOLATED":
                return 'bk-color-blue';
            case "GEOMETRIC_CENTER":
                return 'bk-color-orange';
            case "APPROXIMATE":
                return 'bk-color-brown';
            default:
                return 'bk-color-black';
        }
    };

    function fillAddressField(address, comp) {
        switch (comp.types[0]) {
            case 'street_number':
                if (!address.NumExt) address.NumExt = comp.long_name;
                break;
            case 'route':
                if (!address.MainAddress) address.MainAddress = comp.long_name;
                break;
            case 'sublocality_level_1':
            case 'sublocality':
            case 'neighborhood':
                if (!address.RegionC) address.RegionC = comp.long_name;
                break;
            case 'administrative_area_level_3':
            case 'administrative_area_level_2':
                if (!address.RegionB) address.RegionB = comp.long_name;
                break;
            case 'administrative_area_level_1':
                if (!address.RegionA) address.RegionA = comp.long_name;
                break;
            case 'country':
                if (!address.Country) address.Country = comp.long_name;
                break;
            case 'postal_code':
                if (!address.ZipCode) address.ZipCode = comp.long_name;
                break;
            default:
                break;
        }
    }


    $scope.onSelectAddress = function (address) {
        $scope.addressSelected = address;
        $scope.m.Address = {};

        var i, j, currComp = address.address_components, comp;

        for (j = 0; j < currComp.length; j++) {
            comp = currComp[j];
            if(!comp.types || comp.types.length === 0)
                continue;
            fillAddressField($scope.m.Address, comp);
        }

        var bIsLess = true;
        for (i = 0; i < $scope.allResults.length; i++) {
            var currAdd = $scope.allResults[i];

            if (bIsLess) {
                if (currAdd === address)
                    bIsLess = false;
                continue;
            }

            currComp = currAdd.address_components;
            for (j = 0; j < currComp.length; j++) {
                comp = currComp[j];
                if (!comp.types || comp.types.length === 0)
                    continue;
                fillAddressField($scope.m.Address, comp);
            }
        }
        
        calculateCoverage(marker.getPosition());

    };
    
    function createPolygon(coordinates, color, title) {
        var polygon = new google.maps.Polygon({
            map: window.gDrsMap,
            paths: coordinates,
            strokeColor: color,
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: color,
            fillOpacity: 0.1,
            zIndex: -1,
            draggable: false,
            editable: false
        });

        polygon.addListener('click', setMarkerInPosition);
        polygon.addListener('rightclick', function(event) {
            var content = '<div id="content"><div id="siteNotice"></div><h5 id="firstHeading" class="firstHeading">' + title + '</h5></div></div>';
            infoWindow.setContent(content);
            infoWindow.setPosition(event.latLng);
            
            infoWindow.open(window.gDrsMap);
        });
        

        return polygon;
    };

    $scope.save = function (isValid) {
        try {
            $scope.working = true;
            $scope.validate = true;
            if (isValid == false)
                return;

            var pos = marker.getPosition();

            $scope.m.Position = { Lat: pos.lat(), Lng: pos.lng() };

            $scope.m.CoverageStoreIds = [];
            for (var i = 0; i < $scope.coverageStore.length; i++) {
                $scope.m.CoverageStoreIds.push($scope.coverageStore[i].id);
            }

            var resp = JSON.parse(window.external.SaveAddress(JSON.stringify($scope.m)));
            if (resp.HasError === true) {
                showError(resp.Message);
            } else {
                showInfo(resp.Message);
            }

        } catch(e) {
            showError("Se sucitó el siguiente error al intentar guardar: " + e);
        } finally {
            $scope.working = false;
        }
    };

    $scope.cancel = function() {
        window.external.Cancel();
    };
    
    window.initCb = init;
    
    window.setInternet = function () {
        $scope.$apply(function() {
            $scope.loadingMap = false;
            $scope.hasInternet = false;
        });
    };
});