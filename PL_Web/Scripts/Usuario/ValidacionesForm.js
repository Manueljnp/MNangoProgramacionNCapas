function MunicipioGetByIdEstado() {
    let ddl = $('#ddlEstado').val();    //Guardar el Id del Estado

    $.ajax({
        url: Rutas.GetByIdEstado + "?idEstado=" + ddl,
        type: "GET",
        datatype: "JSON",
        //data => Es solo para MODELOS
        success: function (result) {    //result nos lo manda el controlador, es una propiedad de JsonResult
            console.log(result);    //VER QUE TRAE EL RESULT
            if (result.Correct) {
                //buscar el ddl donde pintaré los valores
                let ddlMunicipio = $('#ddlMunicipio');

                //LIMPIAR ANTES DE MOSTRAR NUEVAS OPCIONES
                ddlMunicipio.empty();
                ddlMunicipio.append('<option value="">Selecciona un Municipio</option>');

                $.each(result.Objects, function (i, valor) {    //i es la posición y valor un objeto
                    let option = "<option value=" + valor.IdMunicipio + ">" + valor.Nombre + "</option>";
                    ddlMunicipio.append(option)
                })
            }
        },
        error: function (xhr) { //xhr Manejo de errores de JavaScript
            console.log(xhr)
        }
    })
}

function ColoniaGetByIdMunicipio() {
    let ddlMunicipio = $('#ddlMunicipio').val();
    console.log("ID del Municipio seleccionado:", ddlMunicipio);

    if (!ddlMunicipio) {
        console.log("No se ha seleccionado un municipio.");
        return;
    }

    $.ajax({
        url: Rutas.GetByIdMunicipio,
        type: "GET",
        datatype: "JSON",
        data: { idMunicipio: ddlMunicipio }, // Enviar como data
        success: function (result) {
            console.log("Respuesta del servidor:", result);

            if (result.Correct) {
                let ddlColonia = $('#ddlColonia');

                //Limpiar DDL y mostrar nuevo mensaje
                ddlColonia.empty();
                ddlColonia.append('<option value="">Selecciona una Colonia</option>');

                $.each(result.Objects, function (i, valor) {
                    let option = "<option value='" + valor.IdColonia + "'>" + valor.Nombre + "</option>";
                    ddlColonia.append(option);
                    console.log("Opción agregada:", option);
                });
            } else {
                console.log("No se encontraron colonias para el municipio seleccionado.");
            }
        },
        error: function (xhr) {
            console.log("Error en la petición AJAX:", xhr);
        }
    });
}

function validarExtension() {
    //Saber extensión - obtener extensión - dividir el nombre en 2 através de un punto (.)
    //buscamos el id del input (id="inptFileImagen").posicion0 es un input (sino objeto).split=saltarse el (.).pop=quitar la primera parte del array
    var input = $('#inptFileImagen')[0].files[0].name.split('.').pop().toLowerCase()    //toLowerCase=Convertir en minusculas
    console.log(input)

    //Comprobarla con extensiones de imagen (png, jpg, jpeg, webp)
    var extensionesValidas = ['png', 'jpg', 'jpeg', 'webp'] //como el input ya se convirtió en minusculas no es necesario comparar con MAY.
    var banderaImg = false;     //colocamos una bandera que cambiará a true cuando el archivo tenga la extensión correcta

    for (var i = 0; i <= extensionesValidas.length; i++) {
        if (input == extensionesValidas[i]) {
            banderaImg = true;
        }
    }
    //No me da una imagen
    if (!banderaImg) {
        alert(`No seleccionaste una imagen, debe tener las extensiones: ${extensionesValidas}`)
        $('#inptFileImagen').val("");
    }
}

function visualizarImagen(input) {
    if (input.files) {
        var reader = new FileReader();  //FileReader => Leer cualquier tipo de Archivo

        //evento onload => se utiliza para ejecutar una función cuando una página web o un elemento específico ha terminado de cargarse.
        reader.onload = function (elemento) {
            $('#imgUsuario').attr('src', elemento.target.result)
        }
        reader.readAsDataURL(input.files[0])
    }
}

//Para el DatePicker
$("#datepicker").datepicker({
    dateFormat: "dd/mm/yy", //Formato en que se verá en el TextBox
    showAnim: "drop"
});

//Para PASSWORD
function validarPassword(event) {

    var password = document.getElementById("inptPassword").value; //Obtener el valor del password
    var confirmarPassword = document.getElementById("inptConfirmarPassword").value;   //Obtener el valor de ConfirmarPassword
    var inputField = event.target;  //Campo de onfirmar contraseña NO gurdamos su valor aquí, solo el input
    var errorMessage = inputField.parentNode.querySelector(".error");

    if (password !== confirmarPassword) {
        inputField.style.borderColor = 'red'; // Borde rojo si no coinciden
        errorMessage.textContent = 'Las contraseñas no coinciden'; // Mensaje de error
    } else {
        inputField.style.borderColor = 'green'; // Borde verde si coinciden
        errorMessage.textContent = ''; // Limpiar mensaje de error
    }

}