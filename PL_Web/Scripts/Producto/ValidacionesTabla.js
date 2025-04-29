$(document).ready(function () {
    $('#tablaProductos').hide();
});

//Cargar Tabla (GetAll)
$('#ddlSubcategoria').on('change', function () {
    let idSubcategoria = $(this).val();

    if (idSubcategoria && idSubcategoria !== "") {
        console.log("Seleccionó una subcategoria " + idSubcategoria);
        $.ajax({
            url: Rutas.CargarTabla + "?idSubcategoria=" + idSubcategoria,
            type: 'GET',
            data: { idSubcategoria: idSubcategoria },
            success: function (result) {
                console.log(result);

                if (result.Productos && result.Productos.length > 0) {
                    let tabla = $('#tablaProductos tbody');
                    tabla.empty(); //Limpiar tabla

                    $.each(result.Productos, function (i, producto) {

                        var imagen = producto.ImagenBase64
                            ? `<img src="data:image/png;base64,${producto.ImagenBase64}" alt="Imagen del usuario"
                                style="width: 50px; height: 50px;">`
                            : "No hay imagen";

                        tabla.append(`
                            <tr>
                                <td>
                                    <a class="btn btn-warning" onclick="GetById(${producto.IdProducto})">
                                        <i class="bi bi-pencil-square"></i>
                                    </a>
                                </td>
                                <td>${imagen}</td>
                                <td>${producto.Nombre}</td>
                                <td>${producto.Descripcion}</td>
                                <td>${producto.Precio}</td>
                                <td>
                                    <a class="btn btn-danger">
                                        <i class="bi bi-trash"></i>
                                    </a>
                                </td>
                            </tr>
                        `);
                    });
                    $('#tablaProductos').show(); //Mostrar la tabla
                }
                else {
                    $('#tablaProductos').hide(); //ocultar la tabla si no hay productos
                }
            },
            error: function (xhr) {
                console.log(xhr);
                $('#tablaProductos').hide(); //ocultar tabla si ocurre error
            }
        });
    }
    else {
        console.log("No seleccionó nada");
        $('#tablaProductos').hide(); //ocultar tabla si se cambia de selección
    }
});

//DDL Subcategoria
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
                $('#tablaProductos').hide(); //ocultar tabla
            } else {
                ddlSubcategoria.prop("disabled", true); //Deshabilitar
                $('#tablaProductos').hide(); //ocultar tabla
            }

            ddlSubcategoria.val(""); // Limpiar selección
        },
        error: function (xhr) {
            console.log(xhr);
            ddlSubcategoria.prop("disabled", true); // En caso de error
            $('#tablaProductos').hide(); //ocultar tabla
        }
    });
}

function GetById(id) {
    $.ajax({
        url: Rutas.ProductoGetById,
        type: 'GET',
        data: { IdProducto: id },
        datatype: 'JSON',
        success: function (result) {
            if (result.Correct) {

                //cargar Categorias:
                DDLRol(() => {
                    $('#ddlCategoria').val(producto.Subcategoria.Categoria.IdCategoria);
                });

                //Asignar valores a los campos del formulario
                $('#inptNombre').val(producto.Nombre);

                //OpenModal();
            }
        }
    });
}

function DDLRol() {
    $.ajax({
        url: Rutas.pathCategorias, // Ruta del controlador
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data && data.length > 0) {
                let ddl = $('#ddlCategoria');
                ddl.empty(); // Limpiar opciones anteriores
                ddl.append($('<option>', {
                    value: '',
                    text: 'Selecciona un Rol'
                }));

                // Llenar opciones con los datos obtenidos
                $.each(data, function (index, categoria) {
                    ddl.append($('<option>', {
                        value: producto.IdCategoria,
                        text: producto.Nombre
                    }));
                });
            } else {
                console.error('No se encontraron roles');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los roles:', error);
        }
    });
}