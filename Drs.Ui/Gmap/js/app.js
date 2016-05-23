window.angJsDependencies = [];
window.constMainApp = 'gMap';
var app = angular.module(window.constMainApp, window.angJsDependencies);

app.controller('gMapController', function ($scope, $timeout) {

    function hasValue(str) {
        if (str && str.trim().length > 0)
            return true;
        return false;
    }

    $scope.search = function () {
        var geocoder = window.geocoder;
        var gMap = window.gDrsMap;

        var restrictions = { country: 'MX' };

        if (hasValue($scope.zipCode)) {
            restrictions.postalCode = $scope.zipCode;
        }

        geocoder.geocode({
            address: $scope.zipCode,
            componentRestrictions: restrictions,
            region: 'MX'
        }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                gMap.setCenter(results[0].geometry.location);
                var marker = new google.maps.Marker({
                    map: gMap,
                    position: results[0].geometry.location
                });
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
            window.console.log(results);
        });
    };

    var marker, service;

    $scope.init = function() {
        window.geocoder = new google.maps.Geocoder();

        var mapOptions = {
            zoom: 14,
            center: new google.maps.LatLng(0, 0),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        window.gDrsMap = new google.maps.Map(window.document.getElementById('map'), mapOptions);

        var input = window.document.getElementById('pac-input');

        var autocomplete = new google.maps.places.Autocomplete(input);
        autocomplete.bindTo('bounds', window.gDrsMap);

        window.gDrsMap.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

        var infowindow = new google.maps.InfoWindow();
        //var marker = new google.maps.Marker({
        //    map: window.gDrsMap,
        //    draggable: true,
        //    animation: google.maps.Animation.DROP
        //});
        //marker.addListener('dblclick', function () {
        //    infowindow.open(window.gDrsMap, marker);
        //});

        autocomplete.addListener('place_changed', function() {
            infowindow.close();
            var place = autocomplete.getPlace();
            if (!place.geometry) {
                return;
            }
            

            if (place.geometry.viewport) {
                window.gDrsMap.fitBounds(place.geometry.viewport);
            } else {
                window.gDrsMap.setCenter(place.geometry.location);
                window.gDrsMap.setZoom(17);
            }

            // Set the position of the marker using the place ID and location.
            //var marker = new google.maps.Marker({
            //    map: window.gDrsMap,
            //    draggable: true,
            //    animation: google.maps.Animation.DROP
            //});

            //marker.setPlace({
            //    //placeId: place.place_id,
            //    location: place.geometry.location
            //});
            
            var location = window.gDrsMap.getCenter();

            marker.setPosition(new google.maps.LatLng(location.lat(), location.lng()));
            marker.setVisible(true);

            //marker.addListener('click', function() {
            //    if (marker.getAnimation() !== null) {
            //        marker.setAnimation(null);
            //    } else {
            //        marker.setAnimation(google.maps.Animation.BOUNCE);
            //    }
            //}
            //);


            //infowindow.setContent('<div><strong>' + place.name + '</strong><br>' +
            //    'Place ID: ' + place.place_id + '<br>' +
            //    place.formatted_address);
            //infowindow.open(window.gDrsMap, marker);


        });

        marker = new google.maps.Marker({
            map: window.gDrsMap,
            draggable: true,
            animation: google.maps.Animation.DROP,
            position: { lat: 0, lng: 0 },
            visible: false
        });
        marker.addListener('click', toggleBounce);
        
        

        service = new google.maps.places.PlacesService(window.gDrsMap);

    };
    
    function callback(results, status) {
        if (status === google.maps.places.PlacesServiceStatus.OK) {
            for (var i = 0; i < results.length; i++) {
                console.log(results[i]);
            }
        }
    }


    function toggleBounce() {
        //if (marker.getAnimation() !== null) {
        //    marker.setAnimation(null);
        //} else {
        //    marker.setAnimation(google.maps.Animation.BOUNCE);
        //}

        service.nearbySearch({
            location: marker.getPosition(),
            radius: 20,
            types: ['street_address', 'street_number', 'administrative_area_level_1'
            , 'administrative_area_level_2'
            , 'administrative_area_level_3'
            , 'administrative_area_level_4'
            , 'administrative_area_level_5'
            , 'sublocality'
            , 'sublocality_level_1'
            , 'sublocality_level_2'
            , 'sublocality_level_3'
            , 'sublocality_level_4'
            , 'sublocality_level_5'
            , 'geocode'
            , 'locality'
            ]
        }, callback);
    }



    window.initCb = $scope.init;

});