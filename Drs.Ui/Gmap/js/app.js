window.angJsDependencies = [];
window.constMainApp = 'gMap';
var app = angular.module(window.constMainApp, window.angJsDependencies);

app.controller('gMapController', function ($scope) {

    var marker, autocomplete, service;
    $scope.addresses = [];

    function setMarkerInPosition(e) {
        var location = e.latLng;
        marker.setPosition(new google.maps.LatLng(location.lat(), location.lng()));
        marker.setVisible(true);
    }

    function placeChanged() {
        var place = autocomplete.getPlace();
        if (!place.geometry) {
            return;
        }

        window.gDrsMap.setCenter(place.geometry.location);
        window.gDrsMap.setZoom(17);

        var location = window.gDrsMap.getCenter();
        marker.setPosition(new google.maps.LatLng(location.lat(), location.lng()));
        marker.setVisible(true);
    }


    $scope.init = function () {
        window.geocoder = new google.maps.Geocoder();
        var mapOptions = {
            zoom: 14,
            center: new google.maps.LatLng(0, 0),
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

        marker = new google.maps.Marker({
            map: window.gDrsMap,
            draggable: true,
            animation: google.maps.Animation.DROP,
            position: { lat: 0, lng: 0 },
            visible: false
        });
        marker.addListener('click', getAddresses);
        service = new google.maps.places.PlacesService(window.gDrsMap);

    };

    function getAddresses() {
        window.geocoder.geocode({ location: marker.getPosition() }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                $scope.$apply(function() {
                    $scope.addresses = results;
                });
                //if (results[1]) {
                //    console.log(results[1].place_id);
                //    console.log(results);
                //} else {
                //    window.alert('No results found');
                //}
            } else {
                //window.alert('Geocoder failed due to: ' + status);
            }
        });
    }

    window.initCb = $scope.init;
});