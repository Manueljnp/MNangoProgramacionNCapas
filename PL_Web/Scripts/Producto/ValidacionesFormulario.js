$(document).ready(function () {
    let idSubcategoria = $('#ddlSubcategoria').val();

    if (idSubcategoria && idSubcategoria !== "" && idSubcategoria !== "0") {
        $(ddlSubcategoria).prop('disabled', false);
        $(ddlSubcategoria).data('idSubcategoriaActual', idSubcategoria);

        console.log("entró al if");
        console.log(idSubcategoria);
    }
});

function SubcategoriaGetByIdCategoria() {
    let ddlCategoria = $('#ddlCategoria');
    let ddlSubcategoria = $('#ddlSubcategoria');
    let idCategoria = ddlCategoria.val();

    if (!idCategoria || idCategoria === "") {
        //Si no hay categoría seleccionada
        ddlSubcategoria.prop("disabled", true); //Desactivar
        $('#tablaProductos').hide(); //ocultar tabla
        ddlSubcategoria.empty(); //Limpiar DDL
        ddlSubcategoria.append('<option value="">Selecciona una Subcategoria</option>');
        return; //Salir
    }

    // Si hay una categoría válida
    $.ajax({
        url: Rutas.GetByIdCategoria + "?idCategoria=" + idCategoria,
        type: "GET",
        datatype: "JSON",
        success: function (result) {
            console.log(result);

            ddlSubcategoria.empty();
            ddlSubcategoria.append('<option selected value="">Selecciona una Subcategoria</option>');

            if (result.Correct && result.Objects.length > 0) {
                $.each(result.Objects, function (i, valor) {
                    ddlSubcategoria.append(`<option value="${valor.IdSubcategoria}">${valor.Nombre}</option>`);
                });

                ddlSubcategoria.prop("disabled", false); //Habilitar
            } else {
                ddlSubcategoria.prop("disabled", true); //Deshabilitar
            }

            //CORRECCIÓN
            //Solo limpiar si NO tiene valor ya seleccionado (o si vamos a agregar)
            let idSubcategoriaActual = ddlSubcategoria.data('idSubcategoriaActual');
            if (idSubcategoriaActual && idSubcategoriaActual !== "0") {
                ddlSubcategoria.val(idSubcategoriaActual);
            }
            else {
                ddlSubcategoria.val(""); //Limpiar selección (caso agregar)
            }
        },
        error: function (xhr) {
            console.log(xhr);
            ddlSubcategoria.prop("disabled", true); // En caso de error
        }
    });
}

function validarExtension() {
    //Saber extensión - obtener extensión - dividir el nombre en 2 através de un punto (.)
    var input = $('#inptFileImagen')[0].files[0].name.split('.').pop().toLowerCase();
    console.log(input);

    //coparar con otras extensiones de imagen
    var extensionesPermitidas = ['png', 'jpg', 'jpeg', 'webp'];
    var banderaImg = false;

    for (var i = 0; i <= extensionesPermitidas.length; i++) {
        if (input == extensionesPermitidas[i]) {
            banderaImg = true;
        }
    }
    //Si el archivo cargado no es válido
    if (!banderaImg) {
        alert(`No seleccionaste una imagen, debe tener las extensiones: ${extensionesPermitidas}`);
        $('#inptFileImagen').val("");
    }
}

function visualizarImagen(input) {
    if (input.files) {
        var reader = new FileReader(); //FileReader lee cualquier tipo de archivo

        //evento onload => se utiliza para ejecutar una función cuando
        //una página web o un elemento específico ha terminado de cargarse

        reader.onload = function (elemento) {
            $('#imgProducto').attr('src', elemento.target.result);
        }
        reader.readAsDataURL(input.files[0]);

    }
}

