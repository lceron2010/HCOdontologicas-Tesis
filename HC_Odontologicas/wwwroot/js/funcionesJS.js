function obtenerMenu() {
	$('#config').remove();
	$.ajax({
		type: "POST",
		url: '/../Menu/_Menu',
		data: {},
		success: function (response) {
			$.each(response, function (key, registro) {
				var text;
				if (registro.subMenu.length === 0) {
					text = "<li><a href='" + registro.accion + "'>" + registro.nombre + "</a>";
				}
				else {
					text = "<li class='dropdown'>"
						+ "<a href='" + registro.accion + "' class='dropdown-toggle a-header' data-toggle='dropdown'>" + registro.nombre + " <span class='" + registro.icono + "'></span></a>"
						+ "<ul class='dropdown-menu'>";
					$.each(registro.subMenu, function (key1, child) {
						text += "<li>"
							+ "<a class='menu' dir='/" + child.accion + "'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-11 pull-right' style='margin:0; padding-left:0' > " + child.nombre + "</div><span class='" + child.icono + "'></span></a>"
							+ "</li>";
					});
					text += "</ul>";
				}
				$("ul[id='menu']").append("</li>" + text);
			});
		}
	});

}

function regresar(url, tipo) {
	//if (modified)
	//	WarningAlert2btn("¿Está seguro de salir de la página?", url, tipo);
	//else {
	if (tipo === "salir") {
		var form = document.getElementById("logoutForm");
		if (typeof (form) !== "undefined")
			form.submit();
	} else
		window.location.href = url;
	//}
}

function eliminar(Codigo, url, urlRet) {
	swal({
		title: 'HC-Odontologicas',
		text: "¿Seguro de borrar el registro?",
		icon: 'warning',
		buttons: {
			cancel: {
				text: "Cancelar",
				visible: true,
				closeModal: true
			},
			confirm: {
				text: "Aceptar",
				closeModal: true,
				className: "btn-primary"
			}
		}
	}).then(function (result) {
		if (result) {
			//$("#loading").modal();
			$.ajax({
				type: "POST",
				data: { Codigo },
				url: url,
				success: function (r) {
					if (r === "Delete") {
						SuccessAlert("Eliminados", urlRet);
					}
					else {
						ErrorAlert(r);
					}
				}
			});
		}
	});
}

/// cargarDatosConsentimiento

function cargarDatosConsentimiento(PlantillaConsentimiento) {
	
	var CodigoPlantilla = $(PlantillaConsentimiento).find("option:selected").val();
	$.ajax({
		type: "POST",
		url: "/../ConsentimientoInformado/CargarDatosPlantillaConsentimiento",
		data: { CodigoPlantilla },
		success: function (response) {
			if (response !== undefined) {
				$("#Nombre").val(response.nombre);
				$("#Descripcion").val(response.descripcion);
			}
			else {
				$("#Nombre").val("");
				$("#Descripcion").val("");
			}	
		}
	});

}

function cargarDatosReceta(PlantillaReceta) {
	var CodigoPlantilla = $(PlantillaReceta).find("option:selected").val();
	$.ajax({
		type: "POST",
		url: "/../RecetaMedica/CargarDatosPlantillaReceta",
		data: { CodigoPlantilla },
		success: function (response) {
			console.log(response);
			if (response !== undefined) {
				$("#Nombre").val(response.nombre);
				$("#Descripcion").val(response.descripcion);
			}
			else {
				$("#Nombre").val("");
				$("#Descripcion").val("");
			}
		}
	});
}
	//OJOOOOO......cambiar al controlador de carrera
function cargarDatosCarrera(Facultad) {
	var CodigoFacultad = $(Facultad).find("option:selected").val();
	$.ajax({
		type: "POST",
		url: "/../Pacientes/CargarDatosCarrera",	
		data: { CodigoFacultad },
		success: function (response) {
			console.log(response);
			$("#CodigoCarrera").find('option').remove();
			$.each(response, function (key, registro) {
				$("#CodigoCarrera").append($('<option>', {
					value: registro.value,
					text: registro.text
				}));
			});
		}
	});

}


//odontograma

function cargarPopUp(idGrupo) {
	console.log("hola es una prueba");
	console.log(idGrupo);
	

	$("#idGrupo").val(idGrupo);
	$("#modalImportarDatos").modal("show");
	
	//$('#modalOdontograma').modal('show'); // abrir
//	$('#modalOdontograma').modal('hide'); // cerrar

}

function cambiarColor(enfermedadOdon) {
	var idGrupo = $("#idGrupo").val();

	//console.log("cambiar color");
	
	//console.log(idGrupo);

	//console.log('enfermedad', enfermedadOdon);

	//nombre = enfermedadOdon.substring(1) + "-" + idGrupo.substring(1,3);
	nombre = enfermedadOdon.charAt(enfermedadOdon.length - 1) + "-" + idGrupo.substring(1, 3);
	//console.log("nombe", nombre);
	$("#" + nombre).attr("style", "fill:red");
	$("#" + nombre).attr("cambiocolor", "true");
	$("#" + nombre).attr("enfermedad", enfermedadOdon);

	//para prueba

	$("#O-11").attr("cambiocolor", "true");
	$("#O-11").attr("enfermedad", "ResinaO");

	$('#modalOdontograma').modal('hide'); // cerrar
}


function GuardarDatosOdontograma() {
	//var data = new FormData();
	//data.append('imagen', $('#svg742')[0].children[3]);
	
	const codigoPaciente = $('#CodigoPaciente')[0].value; 
	const codigoPersonal = $('#CodigoPersonal')[0].value;

	//leer los detalles de pieza.
	var listaOdontogramaDetalle = [];
	datos = $('#svg742')[0].children[3].children[4];
	for (var i = 0; i < datos.childElementCount; i++) {
		dato = $('#svg742')[0].children[3].children[4].children[i]; //ene l [3] hay que recorrer
		//propiedadColor = dato.attributes.cambiocolor;
		cambioColor = false;
		if (dato.attributes.cambiocolor !== undefined) {
			cambioColor = dato.attributes.cambiocolor.value;
		}			
		if (cambioColor) {
			pieza = dato.id.substring(2, 4);
			region = dato.id.substring(0, 1);
			enfermedad = dato.attributes.enfermedad.value.substring(0, dato.attributes.enfermedad.value.length -1);
			diagnosticoDato = dato.style.fill;
			diagnostico = "Diagnosticado";
			if (diagnosticoDato === "red") {
				diagnostico = "Recomendado";
			}

			var detalle = { Pieza: pieza, Region: region, Enfermedad: enfermedad, Valor: true, Diagnostico: diagnostico };

			listaOdontogramaDetalle.push(detalle);
		}
	}

	var odontograma = [
		{
			CodigoPaciente: codigoPaciente, CodigoPersonal: codigoPersonal,
			OdontogramaDetalle: listaOdontogramaDetalle				
		}
	];

	$.ajax({
		url: '/../Odontogramas/Create',
		type: 'POST',
		dataType: 'json',
		data: { odontograma },
		success: function (response) {
			if (response === "Save") {								
				SuccessAlert("Guardados", "/../Pacientes");						
			}
			else {
				ErrorAlert(response);
			}
		}
	});

}

function obtenerDatosOdontograma(codigo) {	
	$.ajax({
		type: "GET",
		url: "/../Odontogramas/Edit",
		data: { codigo },
		success: function (response) {
			console.log(response);			
		}
	});
}



//importar datos

function cargarDatosTablaImportar() {
	var data = new FormData();	
	data.append('Documento', $('#Documento')[0].files[0]);
	
	var opts = {
		url: '../Pacientes/CargarDatosTabla',
		data: data,
		cache: false,
		contentType: false,
		processData: false,
		method: 'POST',
		type: 'POST',
		success: function (response) {			
			console.log(response);
			var table_data = jQuery.parseJSON(response);			
			$('#tablaImportarImpuestos').dataTable({
				data: table_data,
				bSort: false,
				processing: true, // for show progress bar  
				serverSide: false, // for process server side  
				filter: false, // this is for disable filter (search box) 
				columns: [
					{ "data": "Nro", "name": "Nro", "autoWidth": true },
					{ "data": "Código", "name": "Código", "autoWidth": true },
					{ "data": "Nombre", "name": "Nombre", "autoWidth": true },
					{ "data": "Cédula", "name": "Cédula", "autoWidth": true },
					{ "data": "FechaNac", "name": "FechaNac", "autoWidth": true },
					{ "data": "Genero", "name": "Genero", "autoWidth": true },
					{ "data": "Dirección.", "name": "Dirección.", "autoWidth": true },
					{ "data": "Teléfono", "name": "Teléfono", "autoWidth": true },
					{ "data": "Celular", "name": "Celular", "autoWidth": true },
					{ "data": "Email", "name": "Email", "autoWidth": true },
					{ "data": "Carrera", "name": "Carrera", "autoWidth": true },
					{ "data": "Facultad", "name": "Facultad", "autoWidth": true },
					{ "data": "EmailEPN.", "name": "EmailEPN.", "autoWidth": true },
					{ "data": "Graduado", "name": "Graduado", "autoWidth": true },
					{ "data": "Titulacion", "name": "Titulacion", "autoWidth": true },
					{ "data": "Etnia", "name": "Etnia", "autoWidth": true },				

				]
			});
		}
	};

	if (data.fake) {
		// Make sure no text encoding stuff is done by xhr
		opts.xhr = function () { var xhr = jQuery.ajaxSettings.xhr(); xhr.send = xhr.sendAsBinary; return xhr; }
		opts.contentType = "multipart/form-data; boundary=" + data.boundary;
		opts.data = data.toString();
	}
	jQuery.ajax(opts);

}


function GuardarDatosImportardos() {
	var data = new FormData();	
	data.append('Documento', $('#Documento')[0].files[0]);
	
	var opts = {
		url: '../Pacientes/GuardarDatosImportados',
		data: data,
		cache: false,
		contentType: false,
		processData: false,
		method: 'POST',
		type: 'POST',
		success: function (response) {			
			if (response === "Save") {
				$('#modalOdontograma').modal('hide');
				limpiarDatos();
				SuccessAlert("Guardados", "/../Pacientes");				
			}
			else {
				ErrorAlert(response);
			}
		}
	};

	if (data.fake) {
		// Make sure no text encoding stuff is done by xhr
		opts.xhr = function () { var xhr = jQuery.ajaxSettings.xhr(); xhr.send = xhr.sendAsBinary; return xhr; }
		opts.contentType = "multipart/form-data; boundary=" + data.boundary;
		opts.data = data.toString();
	}
	jQuery.ajax(opts);

}

var tiposAdjunto = ["application/excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"];

function validarTamanioArchivo(response) {

	$("#tablaImportarImpuestos > tbody").empty();
	//$('#btnGuardar').attr("disabled", true);

	if (tiposAdjunto.includes($(response)[0].files[0].type)) {
		var tamanoBytes = $(response)[0].files[0].size;
		var tamanoMB = parseFloat(tamanoBytes) / parseFloat(Math.pow(1024, 2));
		var tamanoAdjunto = $("#TamanoAdjunto").val();
		if (parseFloat(tamanoMB) >= parseFloat(tamanoAdjunto)) {
			$("#DocumentoMensaje").text('El archivo excede el tamaño permitido (' + tamanoAdjunto + ' MB)');
			$("#Documento").val("");
		}
		else
			$("#DocumentoMensaje").text("");
	} else {
		$("#DocumentoMensaje").text("Extensión de archivo incorrecta.");
		$("#Documento").val('');
	}



}


function limpiarDatos() {
	//$('#btnGuardar').attr("disabled", true);
	$("#tablaImportarImpuestos > tbody").empty();
	$("#Documento").val("");	
	$('.custom-file-label').removeClass("selected").html("");

}

//agenda

function cargarPacientes() {
	var pacientes_opt = [];
	$.ajax({
		type: "GET",
		url: "/../Pacientes/CargarPacientes",
		data: {},
		success: function (response) {
			console.log(response);			
			$.each(response, function (key, registro) {
				pacientes_opt.push({ key: registro.value, label: registro.text });			
			});
		}
	});
	console.log(pacientes_opt);
	return pacientes_opt;
}

function cargarDoctores() {
	var doctores_opt = [];
	$.ajax({
		type: "GET",
		url: "/../Personal/CargarDoctores",
		data: {},
		success: function (response) {
			console.log(response);
			$.each(response, function (key, registro) {
				doctores_opt.push({ key: registro.value, label: registro.text });
			});
		}
	});
	console.log(doctores_opt);
	return doctores_opt;
}




