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

//Validaciones EXPRESION REGULAR (REGEX)
function SoloLetras(event) { //Variable entrada de tipo string | which => Codigo (numero) de cada caracter
    var entrada = String.fromCharCode(event.which) //fromcharcode => Convertir el código which a Letra (solo válida el último caracter)
    var inputField = event.target; //target => Imprime todo lo relacionado al INPUT (type, class, id...)
    var ErrorMessage = inputField.parentNode.querySelector('.error') //ParentNode=> todos los relacionados a el (spam en este caso) | queryselector =>
    if ((/[a-zA-Z]/.test(entrada))) {
        console.log("Es una letra")
        inputField.style.borderColor = 'green';
        ErrorMessage.textContent = ''; //Cuando se vuelva a escribir letra borrar el mensaje de error
    }
    else {
        console.log("No es letra")
        event.preventDefault() //Evitará que muestre la tecla (en este caso los numeros) (no ejecuta el evento)
        //entramos al style del input y modificamos el border color para que se muestre en ROJO
        inputField.style.borderColor = 'red';
        //Cuando no ponga letra, entrar a ErrorMessage y que ponga un mensaje
        ErrorMessage.textContent = 'Solo se aceptan letras';
    }
}

function validarNumerosCaracter(event) {
    //fromCharCode solo obtiene la tecla especifica presionada,
    var entrada = String.fromCharCode(event.which); //entonces no evaluará toda la cadena (no sabrá si son 10 numeros)
    var inputField = event.target;
    var expresionRegular = /^\d$/;
    var ErrorMessage = inputField.parentNode.querySelector('.error');
    if (!expresionRegular.test(entrada)) {
        event.preventDefault();
        inputField.borderColor = 'red';
        ErrorMessage.textContent = 'Solo se permiten Numeros';
    }
    else {
        inputField.style.borderColor = 'green';
        ErrorMessage.textContent = '';

    }
}

function validarNumerosCadena(event) {
    var inputField = event.target;
    var entrada = inputField.value; //La entrada captura todo el texto del input
    var ErrorMessage = inputField.parentNode.querySelector('.error');

    if (entrada.length > 10) {

        inputField.value = entrada.slice(0, 10); //borrar el último caracter ingresado si se pasa de 10
        //entrada.preventDefault(); //NO FUNCIONA para cadea (aparentemente)
        inputField.borderColor = 'red';
        ErrorMessage.textContent = 'Solo 10 digitos';
    }
    else {
        inputField.borderColor = 'green';
        //ErrorMessage.textContent = ''; //Este borra el errormessage de validarCaracter=>'Solo se permiten Numeros'
    }
}

function validarCorreo(event) {
    var inputField = event.target;
    var entrada = inputField.value;
    var errorMessage = inputField.parentNode.querySelector(".error");

    var regex = /^[a-zA-Z0-9._%+-]+@@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (regex.test(entrada)) {
        //Todo bien
        errorMessage.textContent = 'Correo correcto'
        inputField.style.borderColor = 'green';
    }
    else {
        //Todo Mal
        errorMessage.textContent = 'Correo inválido';
        inputField.style.borderColor = 'red';
    }
}

function validarCURP(event) {
    var inputField = event.target;
    var entrada = inputField.value;
    var errorMessage = inputField.parentNode.querySelector(".error");
    var regex = /^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0\d|1[0-2])(?:[0-2]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$/;

    if (regex.test(entrada)) {
        inputField.style.borderColor = 'green';
        errorMessage.textContent = '';
    }
    else {
        event.preventDefault();
        inputField.style.borderColor = 'red';
        errorMessage.textContent = 'CURP errónea';
    }

}

function BorrarMensaje(event) {
    var inputField = event.target;
    var errorMessage = inputField.parentNode.querySelector('.error');

    //Validar si el input quedó vacío
    if (inputField.value.length === 0) { //si la longitud del valor del input es === 0 (está vacío)
        errorMessage.textContent = 'No puede ir vacío';
        inputField.style.borderColor = 'red'
    }
    else {
        errorMessage.textContent = '';
        inputField.style.borderColor = 'green';
    }
}

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

//Validar VACIOS
function validarVacio(event) {
    console.log("holi");
}

//this=> etiqueta
//event=> evento

//Validar FORM (Solo NOMBRE, Terminar)
$('Form').on('submit', function (event) {
    var userValor = document.getElementById("inptUserName")
    var userField = event.target;
    var errorMessage = userField.parentNode.querySelector(".error");
    if (userValor = '') {
        userField.style.borderColor = 'red';
    }
    if ($('#idNombre').val() == '') {

        alert("Completa los campos");
        event.preventDefault();
    }
})