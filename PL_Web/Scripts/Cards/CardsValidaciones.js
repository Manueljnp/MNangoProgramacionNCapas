$(document).ready(function () {
    $('#ddlCategoria').trigger('change');
    console.log("Ready listo jsjjs");
});

function SubcategoriaGetByIdCategoria() {
    let ddlCategoria = $('#ddlCategoria');
    let ddlSubcategoria = $('#ddlSubcategoria');
    let idCategoria = ddlCategoria.val();

    if (!idCategoria || idCategoria === "") {
        //Si no hay categoría seleccionada
        ddlSubcategoria.prop("disabled", true); //Desactivar
        ddlSubcategoria.empty(); //Limpiar DDL
        ddlSubcategoria.append('<option value="">Selecciona una Subcategoria</option>');
        return; //Salir
    }

    // Si hay una categoría válida
    $.ajax({
        url: Rutas.pathGetByIdCategoria + "?idCategoria=" + idCategoria,
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
            ddlSubcategoria.val(""); // Limpiar selección
        },
        error: function (xhr) {
            console.log(xhr);
            ddlSubcategoria.prop("disabled", true); // En caso de error
        }
    });
}