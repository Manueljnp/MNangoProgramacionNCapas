// Inicializar y agregar el mapa
let map;

async function initMap() {
    // Importar la librería de Google Maps
    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");

    // Crear una nueva instancia del mapa centrado en la CDMX
    map = new Map(document.getElementById("map"), {
        zoom: 5, // Nivel de zoom inicial
        center: { lat: 19.432608, lng: -99.133209 }, // Centro inicial (CDMX)
    });

    // Crear límites geográficos para ajustar el mapa a todos los marcadores
    const bounds = new google.maps.LatLngBounds();

    // Recorrer todas las sucursales para crear sus marcadores
    sucursales.forEach(sucursal => {
        const position = {
            lat: parseFloat(sucursal.Latitud),
            lng: parseFloat(sucursal.Longitud)
        };

        // Crear un marcador para cada sucursal
        const marker = new google.maps.Marker({
            position: position,
            map: map,
            title: sucursal.Nombre
        });

        // Crear el contenido de la ventana de información (InfoWindow)
        const infoContent = `
            <div>
                <h3>${sucursal.Nombre}</h3>
                <p><strong>Latitud:</strong> ${sucursal.Latitud}</p>
                <p><strong>Longitud:</strong> ${sucursal.Longitud}</p>
            </div>
        `;

        // Crear una nueva instancia de InfoWindow
        const infowindow = new google.maps.InfoWindow({
            content: infoContent
        });

        // Mostrar la InfoWindow al hacer clic en el marcador
        marker.addListener("click", () => {
            infowindow.open({
                anchor: marker,
                map: map
            });
        });

        // Extender los límites del mapa para incluir este marcador
        bounds.extend(position);
    });

    // Ajustar el zoom dependiendo de la cantidad de sucursales
    if (sucursales.length === 1) {
        map.setCenter(bounds.getCenter());
        map.setZoom(15); // Zoom más cercano si hay solo una sucursal
    } else {
        map.fitBounds(bounds); // Ajustar vista para mostrar todas las sucursales
    }
}

// Llamar la función para iniciar el mapa
initMap();