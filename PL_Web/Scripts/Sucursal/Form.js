let map;
let marker;

function initMap() {
    const defaultPosition = { lat: 19.432608, lng: -99.133209 }; // CDMX por defecto
    let initialPosition = defaultPosition;

    const latInput = document.getElementById("inptLatitud");
    const lngInput = document.getElementById("inptLongitud");

    const latValue = parseFloat(latInput.value);
    const lngValue = parseFloat(lngInput.value);

    // Si hay valores de lat/lng (es un editar), actualiza la posición inicial
    if (!isNaN(latValue) && !isNaN(lngValue)) {
        initialPosition = { lat: latValue, lng: lngValue };
    }

    // Inicializa el mapa
    map = new google.maps.Map(document.getElementById("map"), {
        center: initialPosition,
        zoom: 14,
    });

    // Si hay coordenadas precargadas, coloca el marcador
    if (!isNaN(latValue) && !isNaN(lngValue)) {
        marker = new google.maps.Marker({
            position: initialPosition,
            map: map,
        });
    }

    // Evento: clic en el mapa
    map.addListener("click", function (event) {
        const lat = event.latLng.lat();
        const lng = event.latLng.lng();

        // Coloca marcador nuevo o mueve el existente
        if (marker) {
            marker.setPosition(event.latLng);
        } else {
            marker = new google.maps.Marker({
                position: event.latLng,
                map: map,
            });
        }

        // Actualiza los campos del formulario
        latInput.value = lat.toFixed(6);
        lngInput.value = lng.toFixed(6);
    });
}

initMap();