$(document).ready(function () {
    GetAll();
});

//MODAL
function OpenModal() {
    $("#staticBackdrop").modal("show");
}
function ClearModal() {
    $('#staticBackdrop form')[0].reset(); // Limpia todo el formulario automáticamente
    $('#imgPreview').attr('src', ''); // Limpia la imagen
    $('#ddlMunicipio').prop('disabled', true);
    $('#ddlColonia').prop('disabled', true);
}
function FormularioNuevo() {
    ClearModal();
    DDLRol();

    EstadoGetAllSimple();

    OpenModal();
}


//Funciones CRUD
function Guardar() {
    if (IdUsuario == 0) {
        Add();
    } else {
        Update();
    }
}
function GetAll() {
    $.ajax({
        //url: '@@Url.Action("GetAllusuario", "Usuario")', RUTA RAZOR
        url: pathGetAll,
        type: 'GET',
        dataType: 'JSON',
        success: function (result) {
            if (result.Correct) {
                var usuarios = result.Objects;
                var tabla = document.getElementById("tbody");
                //var tabla = $('#tbody');
                //tabla.empty();
                var row = ``;

                $.each(usuarios, function (index, usuario) {
                    var imagen = usuario.ImagenBase64
                        ? `<img src="data:image/png;base64,${usuario.ImagenBase64}" alt="Imagen del usuario"
                                style="width: 50px; height: 50px;">`
                        : "No hay imagen";

                    row += `
                        <tr>
                            <td><a class="btn btn-warning" onclick="GetById(${usuario.IdUsuario})"> <i class="bi bi-pencil-square"></i></a></td>
                            <td>${usuario.UserName}</td>
                            <td>${usuario.Nombre}</td>
                            <td>${usuario.ApellidoPaterno}</td>
                            <td>${usuario.ApellidoMaterno}</td>
                            <td>${usuario.Email}</td>
                            <td>${usuario.Password}</td>
                            <td>${usuario.FechaNacimiento}</td>
                            <td>${usuario.Sexo}</td>
                            <td>${usuario.Telefono}</td>
                            <td>${usuario.Celular}</td>
                            <td>${usuario.CURP}</td>
                            <td>${usuario.Rol.Nombre}</td>
                            <td> ${usuario.Direccion.Calle}, ${usuario.Direccion.NumeroInterior}, ${usuario.Direccion.NumeroExterior},
                            ${usuario.Direccion.Colonia.Nombre}, ${usuario.Direccion.Colonia.CodigoPostal}, ${usuario.Direccion.Colonia.Municipio.Nombre},
                            ${usuario.Direccion.Colonia.Municipio.Estado.Nombre} </td>
                            <td>${imagen}</td>

                            <td><a href="" class="btn btn-danger" onclick="Delete(${usuario.IdUsuario})">
                            <i class="bi bi-trash"></i></a></td>
                        </tr>
                        `;
                });

                $('#tbody').empty().append(row);
            } else {
                alert('Error');
            }
        }
    })
}

function GetById(id) {
    $.ajax({
        url: pathGetById,
        type: 'GET',
        data: { IdUsuario: id },
        dataType: 'JSON',
        success: function (result) {
            if (result.Correct) {
                var usuario = result.Object;

                // Cargar los estados
                EstadoGetAll(() => {
                    $('#ddlEstado').val(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);

                    // Cargar los municipios dependientes del estado seleccionado
                    MunicipioGetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado, () => {
                        $('#ddlMunicipio').val(usuario.Direccion.Colonia.Municipio.IdMunicipio);

                        // Cargar las colonias dependientes del municipio seleccionado
                        ColoniaGetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio, () => {
                            $('#ddlColonia').val(usuario.Direccion.Colonia.IdColonia);
                        });
                    });
                });

                //Asignar valores a los campos del formulario
                $('#inptUserName').val(usuario.UserName);
                $('#inptNombre').val(usuario.Nombre);
                $('#inptApellidoPaterno').val(usuario.ApellidoPaterno);
                $('#inptApellidoMaterno').val(usuario.ApellidoMaterno);
                $('#inptEmail').val(usuario.Email);
                $('#inptPassword').val(usuario.Password);

                //$('#datepicker').val(usuario.FechaNacimiento);
                //$('#datepicker').val(usuario.FechaNacimiento).format('DD-MM-YYYY');

                $('#inptTelefono').val(usuario.Telefono);
                $('#inptCelular').val(usuario.Celular);
                $('#inptCURP').val(usuario.CURP);

                //Cargar los roles antes
                DDLRol(); // Esto llena el dropdown con opciones
                setTimeout(() => {
                    $('#ddlRol').val(usuario.Rol.IdRol); // Selecciona el valor correspondiente
                }, 500); // Retrasa la selección para asegurarte de que las opciones ya están cargadas

                $('#inptCalle').val(usuario.Direccion.Calle);
                $('#inptNumeroInterior').val(usuario.Direccion.NumeroInterior);
                $('#inptNumeroExterior').val(usuario.Direccion.NumeroExterior);
                
                OpenModal();
            }
        }
    });
}
function Delete(id) {
    
    if (confirm('¿Estás seguro de eliminar?')) {
        $.ajax({
            url: pathDelete,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ IdUsuario: id }),
            success: function (result) {
                if (result.Correct) {
                    alert('El usuario fue eliminado correctamente');
                    GetAll();
                } else {
                    alert('Error al eliminar');
                }
            }
        });
    }
}
function Add() {
}

$('#staticBackdrop').submit(function (e) {
    e.preventDefault();

    var usuario = {
        UserName: $('#inptUserName').val(),
        Nombre: $('#inptNombre').val(),
        ApellidoPaterno: $('#inptApellidoPaterno').val(),
        ApellidoMaterno: $('#inptApellidoMaterno').val(),
        Email: $('#inptEmail').val(),
        Password: $('#inptPassword').val(),
        FechaNacimiento: $('#datepicker').val(),
        Sexo: 'M',
        Telefono: $('#inptTelefono').val(),
        Celular: $('#inptCelular').val(),
        CURP: $('#inptCURP').val(),
        Imagen: $('#imgUsuario').val(),
        Rol: {
            IdRol: $('#ddlRol').val(),
        },
        Direccion: {
            Calle: $('#inptCalle').val(),
            NumeroInterior: $('#inptNumeroInterior').val(),
            NumeroExterior: $('#inptNumeroExterior').val(),
            Colonia: {
                IdColonia: $('#ddlColonia').val(),
                Municipio: {
                    IdMunicipio: $('#ddlMunicipio').val(),
                    Estado: {
                        IdEstado: $('#ddlEstado').val()
                    }
                    //IdMunicipio: $('#ddlMunicipio').val(),
                }
                //IdColonia: $('#ddlColonia').val()
            }
        }
    };

    $.ajax({
        //console.log(usuario.Nombre);
        url: pathAdd,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(usuario),
        success: function (result) {
            console.log('Usuario guardado: ', result);
            if (result.Correct) {
                console.log('Logrado');
                alert('Usuario guardado correctamente (debería)');
                $('#staticBackdrop').modal('hide');
            }
        },
        error: function (error) {
            console.error('Error al guardar el usuario:', error);
            alert('Hubo un error al guardar el usuario.');
        }
    });
});

function Update() {
    var usuario = {
        IdUsuario: IdUsuario, // Aquí se incluye el Id para diferenciar entre crear y actualizar
        UserName: $('#inptUserName').val(),
        Nombre: $('#inptNombre').val(),
        ApellidoPaterno: $('#inptApellidoPaterno').val(),
        ApellidoMaterno: $('#inptApellidoMaterno').val(),
        Email: $('#inptEmail').val(),
        Password: $('#inptPassword').val(),
        FechaNacimiento: $('#datepicker').val(),
        Sexo: 'M',
        Telefono: $('#inptTelefono').val(),
        Celular: $('#inptCelular').val(),
        CURP: $('#inptCURP').val(),
        Imagen: $('#imgUsuario').val(),
        Rol: {
            IdRol: $('#ddlRol').val(),
        },
        Direccion: {
            Calle: $('#inptCalle').val(),
            NumeroInterior: $('#inptNumeroInterior').val(),
            NumeroExterior: $('#inptNumeroExterior').val(),
            Colonia: {
                IdColonia: $('#ddlColonia').val(),
                Municipio: {
                    IdMunicipio: $('#ddlMunicipio').val(),
                    Estado: {
                        IdEstado: $('#ddlEstado').val()
                    }
                }
            }
        }
    };

    $.ajax({
        url: pathUpdate, // Ruta al controlador
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(usuario),
        success: function (result) {
            if (result.Correct) {
                alert('Usuario actualizado correctamente');
                $('#staticBackdrop').modal('hide');
                GetAll(); // Recargar la lista después de actualizar
            } else {
                alert('Error al actualizar el usuario');
            }
        },
        error: function (error) {
            console.error('Error al actualizar el usuario:', error);
            alert('Hubo un error al actualizar el usuario.');
        }
    });
}



//Drop Down List (LISTO)
function DDLRol() {
    $.ajax({
        url: pathRol, // Ruta del controlador
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data && data.length > 0) {
                let ddl = $('#ddlRol');
                ddl.empty(); // Limpiar opciones anteriores
                ddl.append($('<option>', {
                    value: '',
                    text: 'Selecciona un Rol'
                }));

                // Llenar opciones con los datos obtenidos
                $.each(data, function (index, rol) {
                    ddl.append($('<option>', {
                        value: rol.IdRol,
                        text: rol.Nombre
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

//Drop Down List Cascada (SIMPLE)
function EstadoGetAllSimple() {
    $.ajax({
        url: pathEstadoGetAll,
        type: 'GET',
        dataType: 'JSON',
        success: function (result) {
            if (result.Correct) {
                let ddl = $('#ddlEstado');
                ddl.empty(); // Limpia el dropdown
                ddl.append($('<option>', {
                    value: '',
                    text: 'Selecciona un Estado'
                }));

                //Llenar opciones
                var estados = result.Objects;
                $.each(estados, function (index, estado) {
                    ddl.append($('<option>', {
                        value: estado.IdEstado,
                        text: estado.Nombre
                    }));
                });

                //Limpiar los DDL dependientes
                $('#ddlMunicipio').empty().append('<option value="">Selecciona un Municipio</option>');
                $('#ddlMunicipio').prop('disabled', true); //deshabilitar hasta que seleccione otro estado

                $('#ddlColonia').empty().append('<option value="">Selecciona una Colonia</option>');
                $('#ddlColonia').prop('disabled', true); //deshabilitar hasta que seleccione otro municipio

            }
            else {
                console.error('No se encontraron Estados');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los Estados:', error);
        }
    });
}
function MunicipioGetByIdEstadoSimple() {
    let ddl = $('#ddlEstado').val();    //Guardar el Id del Estado

    $.ajax({
        url: pathGetAllMunicipios + "?idEstado=" + ddl,
        type: "GET",
        datatype: "JSON",
        //data => Es solo para MODELOS
        success: function (result) {    //result nos lo manda el controlador, es una propiedad de JsonResult
            console.log(result);    //VER QUE TRAE EL RESULT
            if (result.Correct) {
                //buscar el ddl donde pintaré los valores
                let ddlMunicipio = $('#ddlMunicipio');

                //Habilitar DDL
                $('#ddlMunicipio').prop('disabled', false);

                //LIMPIAR ANTES DE MOSTRAR NUEVAS OPCIONES
                ddlMunicipio.empty();
                ddlMunicipio.append('<option value="">Selecciona un Municipio</option>');

                $.each(result.Objects, function (i, valor) {    //i es la posición y valor un objeto
                    let option = "<option value=" + valor.IdMunicipio + ">" + valor.Nombre + "</option>";
                    ddlMunicipio.append(option)
                });

                // Limpia las colonias ya que se cambiará el municipio
                $('#ddlColonia').empty().append('<option value="">Selecciona una Colonia</option>');
                $('#ddlColonia').prop('disabled', true); // Deshabilita la colonia hasta que se seleccione un municipio
            }
        },
        error: function (xhr) { //xhr Manejo de errores de JavaScript
            console.log(xhr)
        }
    })
}
function ColoniaGetByIdMunicipioSimple() {
    let ddlMunicipio = $('#ddlMunicipio').val();
    console.log("ID del Municipio seleccionado:", ddlMunicipio);

    if (!ddlMunicipio) {
        console.log("No se ha seleccionado un municipio.");
        $('#ddlColonia').empty().append('<option value="">Selecciona una Colonia</option>');
        $('#ddlColonia').prop('disabled', true);
        return;
    }

    $.ajax({
        url: pathGetAllColonias,
        type: "GET",
        datatype: "JSON",
        data: { idMunicipio: ddlMunicipio }, // Enviar como data
        success: function (result) {
            console.log("Respuesta del servidor:", result);

            if (result.Correct) {
                let ddlColonia = $('#ddlColonia');

                //Habilitar DDL
                $('#ddlColonia').prop('disabled', false);

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


//DDL en cascada con CallBack
function EstadoGetAll(callback) {
    $.ajax({
        url: pathEstadoGetAll,
        type: 'GET',
        dataType: 'JSON',
        success: function (result) {
            if (result.Correct) {
                let ddl = $('#ddlEstado');
                ddl.empty();
                ddl.append($('<option>', {
                    value: '',
                    text: 'Selecciona un Estado'
                }));

                $.each(result.Objects, function (index, estado) {
                    ddl.append($('<option>', {
                        value: estado.IdEstado,
                        text: estado.Nombre
                    }));
                });

                // Limpiar los DDL dependientes
                $('#ddlMunicipio').empty().append('<option value="">Selecciona un Municipio</option>');
                $('#ddlMunicipio').prop('disabled', true);
                $('#ddlColonia').empty().append('<option value="">Selecciona una Colonia</option>');
                $('#ddlColonia').prop('disabled', true);

                if (typeof callback === 'function') callback(); // Ejecutar callback si existe
            } else {
                console.error('No se encontraron Estados');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los Estados:', error);
        }
    });
}
function MunicipioGetByIdEstado(idEstado, callback) {
    $.ajax({
        url: pathGetAllMunicipios + "?idEstado=" + idEstado,
        type: 'GET',
        dataType: 'JSON',
        success: function (result) {
            if (result.Correct) {
                let ddlMunicipio = $('#ddlMunicipio');
                ddlMunicipio.empty();
                ddlMunicipio.append('<option value="">Selecciona un Municipio</option>');

                $.each(result.Objects, function (i, valor) {
                    let option = "<option value=" + valor.IdMunicipio + ">" + valor.Nombre + "</option>";
                    ddlMunicipio.append(option);
                });

                $('#ddlMunicipio').prop('disabled', false);
                $('#ddlColonia').empty().append('<option value="">Selecciona una Colonia</option>');
                $('#ddlColonia').prop('disabled', true);

                if (typeof callback === 'function') callback(); // Ejecutar callback si existe
            }
        },
        error: function (xhr) {
            console.error('Error al cargar municipios:', xhr);
        }
    });
}
function ColoniaGetByIdMunicipio(idMunicipio, callback) {
    if (!idMunicipio) {
        $('#ddlColonia').empty().append('<option value="">Selecciona una Colonia</option>');
        $('#ddlColonia').prop('disabled', true);
        return;
    }

    $.ajax({
        url: pathGetAllColonias,
        type: 'GET',
        dataType: 'JSON',
        data: { idMunicipio: idMunicipio },
        success: function (result) {
            if (result.Correct) {
                let ddlColonia = $('#ddlColonia');
                ddlColonia.empty();
                ddlColonia.append('<option value="">Selecciona una Colonia</option>');

                $.each(result.Objects, function (i, valor) {
                    let option = "<option value='" + valor.IdColonia + "'>" + valor.Nombre + "</option>";
                    ddlColonia.append(option);
                });

                $('#ddlColonia').prop('disabled', false);

                if (typeof callback === 'function') callback(); // Ejecutar callback si existe
            }
        },
        error: function (xhr) {
            console.error('Error al cargar colonias:', xhr);
        }
    });
}

function HabilitarDDL() {
    $('#ddlMunicipio').prop('disabled', false);
    $('#ddlColonia').prop('disabled', false);
}


//VALIDACIONES

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

function validarVacio(event) {
    const valor = event.target.value.trim();
    if (!valor) {
        event.target.nextElementSibling.textContent = 'Este campo es obligatorio';
    } else {
        event.target.nextElementSibling.textContent = '';
    }
}

function validarCorreo(event) {
    const valor = event.target.value.trim();
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!regex.test(valor)) {
        event.target.nextElementSibling.textContent = 'Correo inválido';
    } else {
        event.target.nextElementSibling.textContent = '';
    }
}

function validarPassword(event) {
    const password = document.getElementById('inptPassword').value;
    const confirmarPassword = event.target.value;
    if (password !== confirmarPassword) {
        event.target.nextElementSibling.textContent = 'Las contraseñas no coinciden';
    } else {
        event.target.nextElementSibling.textContent = '';
    }
}

function SoloLetras(event) {
    const key = event.key;
    const regex = /^[a-zA-Z\s]+$/;
    if (!regex.test(key)) {
        event.preventDefault();
    }
}

function validarNumerosCaracter(event) {
    const key = event.key;
    if (!/^[0-9]+$/.test(key)) {
        event.preventDefault();
    }
}

function validarCURP(event) {
    const valor = event.target.value.trim().toUpperCase();
    event.target.value = valor;
}

function validarFormulario(event) {
    event.preventDefault(); // Evita que se envíe el formulario si hay errores
    alert('Formulario enviado correctamente');
}
