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

function cargarDiv(idGrupo) {
	console.log(idGrupo);
	//$("#lista").attr("style", "display:block; z-index: 1;top: 0px;left: 0px; opacity: 0; background-color: #000; height: 1088px;width: 1226px; display: block ");
	//$("#g11").attr("style", "position: absolute; z-index:2");
	var t = 40;
	var l = 350;

	$("#lista").attr("style", "display:block; position: absolute; top:"+t+"px;left:"+ l + "px; opacity: 0.8; background-color: #f00;");

	//$("#lista").attr("style", "display:block; ");

	
}

function cargarDiv2(idGrupo) {
	console.log(idGrupo);	
	var t = 40;
	var l = 300;
	$("#lista").attr("style", "display:block; position: absolute; top:"+t+"px; left:"+l+"px; opacity: 0.8; background-color: #f00;");

}

function triangulo() {
	var t = 40;
	var l = 200;

	$("#conEndodonciaImg").attr("style", "display:block; position: absolute; top:" + t + "px; left:" + l + "px;");
}

function sellante() {
	
	var t = 60;
	var l = 360;
	$("#sellanteImg").attr("style", "display:block; position: absolute; top:" + t + "px; left:" + l + "px;");

	//var enfermedadOdon = "Sellante";
	//var nombre = enfermedadOdon.charAt(enfermedadOdon.length - 1) + "-" + idGrupo.substring(1, 3);

	$("#O-11").attr("cambiocolor", "true");
	$("#O-11").attr("enfermedad", "Sellante");

}

function cargarPopUp(idGrupo) {

	//lista
	console.log(idGrupo);
	var t = 60;
	var l = 260;

	$("#lista").attr("style", "display:block; position: absolute; top:" + t + "px; left:" + l + "px; opacity: 0.8;");



	//modal
	//console.log("hola es una prueba");
	//console.log(idGrupo);
	

	//$("#idGrupo").val(idGrupo);
	//$("#modalImportarDatos").modal("show");
	
	//$('#modalOdontograma').modal('show'); // abrir
//	$('#modalOdontograma').modal('hide'); // cerrar

}



function cambiarColorPrueba(id) {
	console.log(id);

	var idGrupo = $("#idGrupo").val();

	console.log(idGrupo);

	var grupo = $('#svg742')[0].children[3].children[49];
	//var sellante = $('#svg742')[0].children[3].children[49].children["Sellante"];
	//console.log(sellante);

	//var remove = grupo.removeChild(sellante);
	//$('#svg742')[0].children[3].children[49].removeChild($('#svg742')[0].children[3].children[49].children["Sellante"])

	//grupo.appendChild(parseSVG($("#svg742")[0].children[3].children["gEnfermedades"].children["SellanteGeneral"].outerHTML));

	//grupo.appendChild(parseSVG('<path style="fill:#ffffff;stroke-width:0.46805626" d="m 42.089157,252.27809 c -1.062079,-0.2393 -2.854329,-0.75867 -3.982786,-1.15376 l -2.051738,-0.71834 v -3.13904 c 0,-1.7265 0.186753,-3.13909 0.414936,-3.13909 0.22824,0 1.694613,0.61372 3.258641,1.36381 3.531804,1.69378 6.94483,2.31177 10.360695,1.87604 3.729541,-0.47574 5.554991,-1.61499 6.232512,-3.88966 1.048695,-3.52088 -0.931614,-5.6751 -7.526683,-8.18761 -10.275743,-3.91479 -12.637415,-6.33723 -12.164901,-12.47779 0.389082,-5.05625 4.229504,-8.4713 11.024058,-9.803 4.049248,-0.79363 9.322154,-0.39803 12.967947,0.97339 l 1.984668,0.74646 v 2.90044 c 0,1.5952 -0.117278,2.90039 -0.260608,2.90039 -0.143324,0 -1.07843,-0.39199 -2.077984,-0.87151 -2.436993,-1.16866 -7.458364,-1.98264 -9.911635,-1.60669 -2.297486,0.3521 -4.927391,1.72964 -5.618055,2.94273 -0.6119,1.07476 -0.615745,4.34263 -0.0065,5.41295 0.58443,1.02653 2.763673,2.24015 6.529763,3.63642 4.751831,1.76179 7.405611,3.1293 9.462436,4.87602 2.673242,2.27024 3.330743,3.67014 3.330743,7.09171 0,2.54019 -0.204505,3.20858 -1.568976,5.12777 -1.221334,1.71787 -2.299549,2.55155 -4.866258,3.76271 -3.051282,1.43978 -3.681578,1.56535 -8.448324,1.68347 -2.833085,0.0701 -6.020018,-0.0685 -7.082086,-0.30778 z" id="SellanteGeneral" inkscape:connector-curvature="0"></path>'));


	grupo.appendChild(parseSVG('<path inkscape: connector-curvature="0"	id="Extraccion" d="m 20.743144,76.652667 -3.37953,-6.85737 4.19822,-8.14263 4.19822,-8.142631 -4.19822,-8.14263 -4.19822,-8.14263 3.37953,-6.85737 3.37953,-6.85737 4.12356,8.33395 4.12355,8.33396 4.10955,-8.33251 4.10954,-8.3325 3.38737,6.8046 3.38737,6.80461 -4.19489,8.19394 -4.19489,8.19395 4.19489,8.193951 4.19489,8.19394 -3.38737,6.80461 -3.38737,6.8046 -4.10954,-8.3325 -4.10955,-8.33251 -4.12355,8.33396 -4.12356,8.33395 z" style="fill:#ffffff;fill-opacity:1;stroke-width:0.39045626"inkscape: label="Extraccion"></path>'));

	//grupo.appendChild(parseSVG($('#svg742')[0].children[3].children["gEnfermedades"].children["SellanteGeneral"]));

	$("#Extraccion").attr("style", "fill:#FF0080");

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

function GuardarDatosOdontograma(accion) {
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
	let url = "";
	if (accion === "crear") {
		url = '/../Odontogramas/Create';
	}
	else {
		url = '/../Odontogramas/Edit';
	}

	$.ajax({
		url: url,//'/../Odontogramas/Create',
		type: 'POST',
		dataType: 'json',
		data: { odontograma },
		success: function (response) {
			if (response === "Save") {								
				SuccessAlert("Guardados", "/../Odontogramas");						
			}
			else {
				ErrorAlert(response);
			}
		}
	});

}

function obtenerDatosOdontograma(codigoOdontograma) {	
	$.ajax({
		type: "GET",
		url: "/../Odontogramas/ObtenerDatosOdontogramaDetalle",
		data: { codigoOdontograma },
		success: function (response) {
			//console.log(response);	
			cargarColorAlEditar(response);
		}
	});
}

function cargarColorAlEditar(response)
{
	console.log(response);
	var detalle = JSON.parse(response);

	for (let i = 0; i <= detalle.length; i++) {
		let nombre = detalle[i].Region + "-" +detalle[i].Pieza;		
		let enfermedad = detalle[i].Enfermedad;
		let diagnostico = detalle[i].Diagnostico;
		///ojooooooooooooooooooooooooOJO   OJOJOJOJO
		//hayq ue revisar cuadno sean los otros tipos de enfermedad como sellenate

		console.log(nombre);
		if (diagnostico === "Diagnosticado") {
			$("#" + nombre).attr("style", "fill:blue");
		}
		else {
			$("#" + nombre).attr("style", "fill:red");
		}
		
		$("#" + nombre).attr("cambiocolor", "true");
		$("#" + nombre).attr("enfermedad", enfermedad);

	}
}


function parseSVG(s) {




	console.log(s);
	var div = document.createElementNS('http://www.w3.org/1999/xhtml', 'div');

	//var hola = '<svg xmlns="http://www.w3.org/2000/svg">' + s + '</svg>';

	//console.log(hola);

	div.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg">' + s + '</svg>';
	console.log(div.innerHTML);
	var frag = document.createDocumentFragment();
	while (div.firstChild.firstChild)
		frag.appendChild(div.firstChild.firstChild);
	return frag;
}


function conFuncionSVG() {


	var xm = "247.13928"; 
	var ym = "83.69396"; 
	var medida = xm + ',' + ym;


	var d =  medida +' c 0,-0.87415 14.72701,-59.32335 14.99273,- 59.50378 0.14144, -0.096 0.57041, 1.156 0.95325, 2.78233 0.38284, 1.62634 3.70161, 14.94403 7.37505, 29.59488 3.67343, 14.65085 6.67897, 26.85834 6.67897, 27.12776 0, 0.27599 - 6.54855, 0.48985 - 15, 0.48985 - 8.479, 0 - 15, -0.21347 - 15, -0.49104 z m 25.4717, -5.43048 c 0, -0.19627 - 1.82626, -7.65548 - 4.05835, -16.57604 - 2.23209, -8.92055 - 4.55638,	-18.27099 - 5.16509, -20.77876 - 0.60871, -2.50776 - 1.17776, -4.56412 - 1.26455, -4.5697 - 0.0868, -0.006 - 0.38704, 1.01147 - 0.66722,2.2601 - 0.28018, 1.24862 - 2.59715, 10.60985 - 5.14881, 20.80272 - 2.55166, 10.19288 - 4.63938, 18.68686 - 4.63938, 18.87552 0, 0.18865 4.71227, 0.34301 10.4717, 0.34301 5.75943, 0 10.4717,-0.16058 10.4717, -0.35685 ';
	var prim = '<path inkscape: connector - curvature="0" id = "EndodonciaPath13" d="m ' + xm;
	//var com = ',';
	var seg = ym + ' c 0,-0.87415 14.72701,-59.32335 14.99273,- 59.50378 0.14144, -0.096 0.57041, 1.156 0.95325, 2.78233 0.38284, 1.62634 3.70161, 14.94403 7.37505, 29.59488 3.67343, 14.65085 6.67897, 26.85834 6.67897, 27.12776 0, 0.27599 - 6.54855, 0.48985 - 15, 0.48985 - 8.479, 0 - 15, -0.21347 - 15, -0.49104 z m 25.4717, -5.43048 c 0, -0.19627 - 1.82626, -7.65548 - 4.05835, -16.57604 - 2.23209, -8.92055 - 4.55638,	-18.27099 - 5.16509, -20.77876 - 0.60871, -2.50776 - 1.17776, -4.56412 - 1.26455, -4.5697 - 0.0868, -0.006 - 0.38704, 1.01147 - 0.66722,2.2601 - 0.28018, 1.24862 - 2.59715, 10.60985 - 5.14881, 20.80272 - 2.55166, 10.19288 - 4.63938, 18.68686 - 4.63938, 18.87552 0, 0.18865 4.71227, 0.34301 10.4717, 0.34301 5.75943, 0 10.4717,-0.16058 10.4717, -0.35685 z" style = "fill:#ff0080;stroke-width:0.39685088" /> ';

	//var p = prim + d + seg;
	var p = prim + ',' + seg;

	//$('#svg742')[0].children[3].children[54].appendChild(parseSVG('<path inkscape: connector - curvature="0" id = "EndodonciaPath13" d = "m 247.13928,83.69396 c 0,-0.87415 14.72701,-59.32335 14.99273,-59.50378 0.14144,-0.096 0.57041,1.156 0.95325,2.78233 0.38284,1.62634 3.70161,14.94403 7.37505,29.59488 3.67343,14.65085 6.67897,26.85834 6.67897,27.12776 0,0.27599 -6.54855,0.48985 -15,0.48985 -8.479,0 -15,-0.21347 -15,-0.49104 z m 25.4717,-5.43048 c 0,-0.19627 -1.82626,-7.65548 -4.05835,-16.57604 -2.23209,-8.92055 -4.55638,-18.27099 -5.16509,-20.77876 -0.60871,-2.50776 -1.17776,-4.56412 -1.26455,-4.5697 -0.0868,-0.006 -0.38704,1.01147 -0.66722,2.2601 -0.28018,1.24862 -2.59715,10.60985 -5.14881,20.80272 -2.55166,10.19288 -4.63938,18.68686 -4.63938,18.87552 0,0.18865 4.71227,0.34301 10.4717,0.34301 5.75943,0 10.4717,-0.16058 10.4717,-0.35685 z" style = "fill:#ff0080;stroke-width:0.39685088" />'));
	$('#svg742')[0].children[3].children[54].appendChild(parseSVG(p.toString()));

	//$('#svg742')[0].children[3].children[54].appendChild(parseSVG('<path inkscape:="" connector="" -="" curvature="0" id="EndodonciaPath13" d = "m 247.13928,83.69396 c 0,-0.87415 14.72701,-59.32335 14.99273,-59.50378 0.14144, -0.096 0.57041, 1.156 0.95325, 2.78233 0.38284, 1.62634 3.70161, 14.94403 7.37505, 29.59488 3.67343, 14.65085 6.67897, 26.85834 6.67897, 27.12776 0, 0.27599 - 6.54855, 0.48985 - 15, 0.48985 - 8.479, 0 - 15, -0.21347 - 15, -0.49104 z m 25.4717, -5.43048 c 0, -0.19627 - 1.82626, -7.65548 - 4.05835, -16.57604 - 2.23209, -8.92055 - 4.55638, -18.27099 - 5.16509, -20.77876 - 0.60871, -2.50776 - 1.17776, -4.56412 - 1.26455, -4.5697 - 0.0868,	-0.006 - 0.38704, 1.01147 - 0.66722, 2.2601 - 0.28018, 1.24862 - 2.59715, 10.60985 - 5.14881, 20.80272 - 2.55166, 10.19288 - 4.63938, 18.68686 - 4.63938, 18.87552 0, 0.18865 4.71227, 0.34301 10.4717, 0.34301 5.75943, 0 10.4717, -0.16058 10.4717, -0.35685 z" style="fill: #ff0080; stroke - width: 0.39685088"></path>'));
}

function ponerEndodoncia() {


	let id = "pathIdD";
	let d = "m 247.13928,83.69396 c 0,-0.87415 14.72701,-59.32335 14.99273,-59.50378 0.14144,-0.096 0.57041,1.156 0.95325,2.78233 0.38284,1.62634 3.70161,14.94403 7.37505,29.59488 3.67343,14.65085 6.67897,26.85834 6.67897,27.12776 0,0.27599 -6.54855,0.48985 -15,0.48985 -8.479,0 -15,-0.21347 -15,-0.49104 z m 25.4717,-5.43048 c 0,-0.19627 -1.82626,-7.65548 -4.05835,-16.57604 -2.23209,-8.92055 -4.55638,-18.27099 -5.16509,-20.77876 -0.60871,-2.50776 -1.17776,-4.56412 -1.26455,-4.5697 -0.0868,-0.006 -0.38704,1.01147 -0.66722,2.2601 -0.28018,1.24862 -2.59715,10.60985 -5.14881,20.80272 -2.55166,10.19288 -4.63938,18.68686 -4.63938,18.87552 0,0.18865 4.71227,0.34301 10.4717,0.34301 5.75943,0 10.4717,-0.16058 10.4717,-0.35685 z";

	let path = document.createElementNS("http://www.w3.org/2000/svg", "path");	
	path.setAttribute(null, "style", "fill: #ff0080; stroke-width: 0.39685088");
	path.setAttribute("id", id);
	path.setAttribute("d", d);

	

	//path.setAttribute(null, "id", "pathIdD");
	//path.setAttribute("d","m 247.13928,83.69396 c 0,-0.87415 14.72701,-59.32335 14.99273,-59.50378 0.14144,-0.096 0.57041,1.156 0.95325,2.78233 0.38284,1.62634 3.70161,14.94403 7.37505,29.59488 3.67343,14.65085 6.67897,26.85834 6.67897,27.12776 0,0.27599 -6.54855,0.48985 -15,0.48985 -8.479,0 -15,-0.21347 -15,-0.49104 z m 25.4717,-5.43048 c 0,-0.19627 -1.82626,-7.65548 -4.05835,-16.57604 -2.23209,-8.92055 -4.55638,-18.27099 -5.16509,-20.77876 -0.60871,-2.50776 -1.17776,-4.56412 -1.26455,-4.5697 -0.0868,-0.006 -0.38704,1.01147 -0.66722,2.2601 -0.28018,1.24862 -2.59715,10.60985 -5.14881,20.80272 -2.55166,10.19288 -4.63938,18.68686 -4.63938,18.87552 0,0.18865 4.71227,0.34301 10.4717,0.34301 5.75943,0 10.4717,-0.16058 10.4717,-0.35685 z");

	$('#svg742')[0].children[3].children[54].appendChild(path);


	$("#pathIdD").attr("style", "fill:#FF0080");


	//newpath = document.createElementNS("SVG", "path");
	//newpath.setAttribute(null, "style", "fill: #ff0080; stroke-width: 0.39685088");	
	//newpath.setAttribute(null, "d", "m 247.13928,83.69396 c 0,-0.87415 14.72701,-59.32335 14.99273,-59.50378 0.14144,-0.096 0.57041,1.156 0.95325,2.78233 0.38284,1.62634 3.70161,14.94403 7.37505,29.59488 3.67343,14.65085 6.67897,26.85834 6.67897,27.12776 0,0.27599 -6.54855,0.48985 -15,0.48985 -8.479,0 -15,-0.21347 -15,-0.49104 z m 25.4717,-5.43048 c 0,-0.19627 -1.82626,-7.65548 -4.05835,-16.57604 -2.23209,-8.92055 -4.55638,-18.27099 -5.16509,-20.77876 -0.60871,-2.50776 -1.17776,-4.56412 -1.26455,-4.5697 -0.0868,-0.006 -0.38704,1.01147 -0.66722,2.2601 -0.28018,1.24862 -2.59715,10.60985 -5.14881,20.80272 -2.55166,10.19288 -4.63938,18.68686 -4.63938,18.87552 0,0.18865 4.71227,0.34301 10.4717,0.34301 5.75943,0 10.4717,-0.16058 10.4717,-0.35685 z");
	//newpath.setAttribute(null, "id", "pathIdD");
	//newpath.setAttribute(null, "inkscape:connector-curvature", "0");
	

	//$('#svg742')[0].children[3].children[54].appendChild(newpath);


	

	//$("#pathIdD").attr("style", "fill:#FF0080");

	//element.style {
	//	fill: #ffffff;
	//	stroke - width: 0.54221141;
	//}

	//$("#" + nombre).attr("style", "fill:red");


	//path[Attributes Style] {
	//	d: path("m 269.87 44.3202 l 3.92199 -6.73753 h -10.4836 c -8.2735 0 -10.4191 0.186866 -10.1762 0.886521 c 0.16822 0.487593 1.49939 3.51956 2.95959 6.73753 l 2.65377 5.85102 h 7.20216 Z");
	//}
	//var element = document.createElementNS('http://www.w3.org/2000/svg', 'path');

	//element.setAttributeNS(null, "id", "end13");
	//element.setAttributeNS(null, "inkscape", "0");	
	//element.setAttributeNS(null, "style", "fill: #ff0080; stroke - width: 0.39685088");


	//$('#svg742')[0].children[3].children[54].appendChild(element);

	//$("#end13").attr("Attributes Style", "d =m 346.10724, 83.69396 c 0, -0.87415 14.72701, -59.32335 14.99273, -59.50378 0.14144, -0.096 0.57041, 1.156 0.95325, 2.78233 0.38284, 1.62634 3.70161, 14.94403 7.37505, 29.59488 3.67343, 14.65085 6.67897, 26.85834 6.67897, 27.12776 0, 0.27599 - 6.54855, 0.48985 - 15, 0.48985 - 8.479, 0 - 15, -0.21347 - 15, -0.49104 z m 25.4717, -5.43048 c 0, -0.19627 - 1.82626, -7.65548 - 4.05835, -16.57604 - 2.23209, -8.92055 - 4.55638, -18.27099 - 5.16509, -20.77876 - 0.60871, -2.50776 - 1.17776, -4.56412 - 1.26455, -4.5697 - 0.0868, -0.006 - 0.38704, 1.01147 - 0.66722, 2.2601 - 0.28018, 1.24862 - 2.59715, 10.60985 - 5.14881, 20.80272 - 2.55166, 10.19288 - 4.63938, 18.68686 - 4.63938, 18.87552 0, 0.18865 4.71227, 0.34301 10.4717, 0.34301 5.75943, 0 10.4717, -0.16058 10.4717, -0.35685 z");

	//var p = '<path inkscape: connector-curvature="0" id="path535-A1" d= "m ' + x + ',' + y + ' c 0, -0.87415 14.72701, -59.32335 14.99273, -59.50378 0.14144, -0.096 0.57041, 1.156 0.95325, 2.78233 0.38284, 1.62634 3.70161, 14.94403 7.37505, 29.59488 3.67343, 14.65085 6.67897, 26.85834 6.67897, 27.12776 0, 0.27599 - 6.54855, 0.48985 - 15, 0.48985 - 8.479, 0 - 15, -0.21347 - 15, -0.49104 z m 25.4717, -5.43048 c 0, -0.19627 - 1.82626, -7.65548 - 4.05835, -16.57604 - 2.23209, -8.92055 - 4.55638, -18.27099 - 5.16509, -20.77876 - 0.60871, -2.50776 - 1.17776, -4.56412 - 1.26455, -4.5697 - 0.0868, -0.006 - 0.38704, 1.01147 - 0.66722, 2.2601 - 0.28018, 1.24862 - 2.59715, 10.60985 - 5.14881, 20.80272 - 2.55166, 10.19288 - 4.63938, 18.68686 - 4.63938, 18.87552 0, 0.18865 4.71227, 0.34301 10.4717, 0.34301 5.75943, 0 10.4717, -0.16058 10.4717, -0.35685 z" 
	//style = "fill: #0000ff; stroke - width: 0.39685088" /> ';


	//var x = "346.10724";
	//var y = "83.69396";

	//console.log(typeof(x));

	//var element = document.createElementNS('http://www.w3.org/2000/svg', 'path');

	//element.setAttributeNS(null, "id", "end13");
	//element.setAttributeNS(null, "inkscape", "0");
	////element.setAttributeNS(null, "d", "m " + x + ","+ y + " c 0, -0.87415 14.72701, -59.32335 14.99273, -59.50378 0.14144, -0.096 0.57041, 1.156 0.95325, 2.78233 0.38284, 1.62634 3.70161, 14.94403 7.37505, 29.59488 3.67343, 14.65085 6.67897, 26.85834 6.67897, 27.12776 0, 0.27599 - 6.54855, 0.48985 - 15, 0.48985 - 8.479, 0 - 15, -0.21347 - 15, -0.49104 z m 25.4717, -5.43048 c 0, -0.19627 - 1.82626, -7.65548 - 4.05835, -16.57604 - 2.23209, -8.92055 - 4.55638, -18.27099 - 5.16509, -20.77876 - 0.60871, -2.50776 - 1.17776, -4.56412 - 1.26455, -4.5697 - 0.0868, -0.006 - 0.38704, 1.01147 - 0.66722, 2.2601 - 0.28018, 1.24862 - 2.59715, 10.60985 - 5.14881, 20.80272 - 2.55166, 10.19288 - 4.63938, 18.68686 - 4.63938, 18.87552 0, 0.18865 4.71227, 0.34301 10.4717, 0.34301 5.75943, 0 10.4717, -0.16058 10.4717, -0.35685 z");
	////element.setAttributeNS(null, "d", m 346.10724 , 83.69396 c 0, -0.87415 14.72701, -59.32335 14.99273, -59.50378 0.14144, -0.096 0.57041, 1.156 0.95325, 2.78233 0.38284, 1.62634 3.70161, 14.94403 7.37505, 29.59488 3.67343, 14.65085 6.67897, 26.85834 6.67897, 27.12776 0, 0.27599 - 6.54855, 0.48985 - 15, 0.48985 - 8.479, 0 - 15, -0.21347 - 15, -0.49104 z m 25.4717, -5.43048 c 0, -0.19627 - 1.82626, -7.65548 - 4.05835, -16.57604 - 2.23209, -8.92055 - 4.55638, -18.27099 - 5.16509, -20.77876 - 0.60871, -2.50776 - 1.17776, -4.56412 - 1.26455, -4.5697 - 0.0868, -0.006 - 0.38704, 1.01147 - 0.66722, 2.2601 - 0.28018, 1.24862 - 2.59715, 10.60985 - 5.14881, 20.80272 - 2.55166, 10.19288 - 4.63938, 18.68686 - 4.63938, 18.87552 0, 0.18865 4.71227, 0.34301 10.4717, 0.34301 5.75943, 0 10.4717, -0.16058 10.4717, -0.35685 z);
	//element.setAttributeNS(null, "style", "fill: #ff0080; stroke - width: 0.39685088");
	

	//$('#svg742')[0].children[3].children[54].appendChild(element);


	//console.log(p);

	//$('#svg742')[0].children[3].children[54].appendChild(parseSVG(p));


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




