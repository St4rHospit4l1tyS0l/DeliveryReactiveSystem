window.initParamsMaps = function() {
    var m = window.modelCl;
    var position = new google.maps.LatLng(m.Lat, m.Lng);
    var mapOptions1 = {
        zoom: 17,
        center: position
    };

    var mapElement1 = document.getElementById('map1');
    var mainMap = new google.maps.Map(mapElement1, mapOptions1);
    
    new google.maps.Marker({
        map: mainMap,
        draggable: false,
        position: position,
        visible: true
    });

    var panoramaOptions = {
        position: position,
        pov: {
            heading: 10,
            pitch: 10
        }
    };
    window.mapPanorama = new google.maps.StreetViewPanorama(document.getElementById('pano'), panoramaOptions);

};

