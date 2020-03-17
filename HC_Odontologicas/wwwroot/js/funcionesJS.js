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

//step



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
				//$("#Descripcion").val(response.descripcion);
				$("#DescripcionReceta").val(response.descripcion);				

			}
			else {	
				//$("#Descripcion").val(response.descripcion);
				$("#DescripcionReceta").val("");
				$("#Indicaciones").val("");
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


function abrir() {
	var div = document.getElementById('menu');
	div.style.display === "block";
}

//odontograma

//mostrar y ocultar la lista de opciones

var div = document.getElementById('menu');
//la funcion que oculta y muestra
function showHide(e) {
	e.preventDefault();
	e.stopPropagation();
	idGrupo = e.target.parentNode.id;
	$("#idPiezaClick").val(idGrupo);
	//console.log('id del parentNode en showhide: ', idGrupo);	
	const top = 30;
	let leftGlobal = 0;
	let left = 0;
	const medida = 60;
	const grupo1 = [11, 12, 13, 14, 15, 16, 17, 18, 51, 52, 53, 54, 55, 81, 82, 83, 84, 85, 41, 42, 43, 44, 45, 46, 47, 48];
	const grupo2 = [21, 22, 23, 24, 25, 26, 27, 28, 61, 62, 63, 64, 65, 71, 72, 73, 74, 75, 31, 32, 33, 34, 35, 36, 37, 38];

	//const medidasLeft1 = [];
	if (idGrupo.charAt(0) === "g") {
		let medidasLeft1;
		//const medidasLeft2;
		if (grupo1.includes(parseInt(idGrupo.substring(1, 3)))) {
			leftGlobal = 400;
			medidasLeft1 = [leftGlobal, leftGlobal - medida, leftGlobal - 2 * medida, leftGlobal - 3 * medida, leftGlobal - 4 * medida, leftGlobal - 5 * medida, leftGlobal - 6 * medida, leftGlobal - 7 * medida];

		}
		else if (grupo2.includes(parseInt(idGrupo.substring(1, 3)))) {
			leftGlobal = 820;
			medidasLeft1 = [leftGlobal - 7 * medida, leftGlobal - 6 * medida, leftGlobal - 5 * medida, leftGlobal - 4 * medida, leftGlobal - 3 * medida, leftGlobal - 2 * medida, leftGlobal - medida, leftGlobal];
		}

		//console.log(medidasLeft1);
		left = medidasLeft1[parseInt(idGrupo.substring(2, 3)) - 1];
		//console.log('ojooooooooo: ', left);
		//console.log('estilo', div.style.display);
		if (div.style.display === "none") {
			//console.log('entro al if de display= none');
			$("#menu").attr("style", "display:block; position: absolute; top:" + top + "px; left:" + left + "px;");
		} else if (div.style.display === "block") {
			//console.log('entro al else if de display= block');
			//console.log('clicl: ', e.target.parentNode.id);
			if (e.target.parentNode.id.charAt(0) === "g") {
				$("#menu").attr("style", "display:block; position: absolute; top:" + top + "px; left:" + left + "px;");
			}
			else {
				div.style.display = "none";
			}
		}
	}
}
//var prueba = [];
//al hacer click en el boton
if (document.getElementById('odontograma') !== null) {
	var contador = $('#svg742')[0].children[3];
	for (var i = 0; i <= contador.childElementCount; i++) {
		if ($('#svg742')[0].children[3].children[i] !== undefined) {
			if ($('#svg742')[0].children[3].children[i].id.charAt(0) === "g") {
				$('#svg742')[0].children[3].children[i].addEventListener("click", showHide, false);
				//prueba.push($('#svg742')[0].children[3].children[i].id);
			}

		}
	}
}
//console.log('prueba:');
//console.log(prueba);
//funcion para cualquier clic en el documento
document.addEventListener("click", function (e) {
	//obtiendo informacion del DOM para  
	var clic = e.target;
	//console.log('clic diferente del div:', clic);
	if (div !== null) {
		if (div.style.display === "block" && clic !== div) {
			div.style.display = "none";
			$("#idPiezaClick").val("");
		}
	}
}, false);

//fin mostrar lista

//cambiar color, establecer enfermedad en la pieza.
var enfermedadesOdontogramasSVG = [
	{ key: "Sellante", valor: "c -1.06206,-0.2393 -2.8543,-0.75867 -3.98274,-1.15375 l -2.05172,-0.71834 v -3.13905 c 0,-1.7265 0.18675,-3.13909 0.41494,-3.13909 0.22823,0 1.69459,0.61372 3.2586,1.3638 3.53178,1.69379 6.94477,2.31178 10.3606,1.87604 3.72951,-0.47573 5.55494,-1.61498 6.23245,-3.88965 1.04869,-3.52088 -0.9316,-5.67509 -7.52661,-8.18761 -10.27564,-3.9148 -12.6373,-6.33724 -12.16479,-12.47779 0.38908,-5.05626 4.22947,-8.47131 11.02396,-9.803 4.04921,-0.79364 9.32207,-0.39803 12.96783,0.97339 l 1.98465,0.74646 v 2.90043 c 0,1.59521 -0.11728,2.90039 -0.26061,2.90039 -0.14333,0 -1.07842,-0.39198 -2.07797,-0.8715 -2.43696,-1.16866 -7.45829,-1.98264 -9.91153,-1.60669 -2.29747,0.3521 -4.92736,1.72964 -5.61801,2.94273 -0.6119,1.07475 -0.61574,4.34263 -0.006,5.41295 0.58443,1.02653 2.76365,2.24015 6.5297,3.63642 4.75179,1.76179 7.40555,3.1293 9.46235,4.87602 2.67321,2.27024 3.33071,3.67014 3.33071,7.09171 0,2.54019 -0.2045,3.20858 -1.56896,5.12777 -1.22133,1.71787 -2.29953,2.55155 -4.86622,3.76271 -3.05125,1.43978 -3.68154,1.56535 -8.44824,1.68347 -2.83305,0.0701 -6.01996,-0.0685 -7.08202,-0.30778 z" },
	{ key: "SellanteIndicado", valor: "c -1.06206,-0.2393 -2.8543,-0.75867 -3.98274,-1.15375 l -2.05172,-0.71834 v -3.13905 c 0,-1.7265 0.18675,-3.13909 0.41494,-3.13909 0.22823,0 1.69459,0.61372 3.2586,1.3638 3.53178,1.69379 6.94477,2.31178 10.3606,1.87604 3.72951,-0.47573 5.55494,-1.61498 6.23245,-3.88965 1.04869,-3.52088 -0.9316,-5.67509 -7.52661,-8.18761 -10.27564,-3.9148 -12.6373,-6.33724 -12.16479,-12.47779 0.38908,-5.05626 4.22947,-8.47131 11.02396,-9.803 4.04921,-0.79364 9.32207,-0.39803 12.96783,0.97339 l 1.98465,0.74646 v 2.90043 c 0,1.59521 -0.11728,2.90039 -0.26061,2.90039 -0.14333,0 -1.07842,-0.39198 -2.07797,-0.8715 -2.43696,-1.16866 -7.45829,-1.98264 -9.91153,-1.60669 -2.29747,0.3521 -4.92736,1.72964 -5.61801,2.94273 -0.6119,1.07475 -0.61574,4.34263 -0.006,5.41295 0.58443,1.02653 2.76365,2.24015 6.5297,3.63642 4.75179,1.76179 7.40555,3.1293 9.46235,4.87602 2.67321,2.27024 3.33071,3.67014 3.33071,7.09171 0,2.54019 -0.2045,3.20858 -1.56896,5.12777 -1.22133,1.71787 -2.29953,2.55155 -4.86622,3.76271 -3.05125,1.43978 -3.68154,1.56535 -8.44824,1.68347 -2.83305,0.0701 -6.01996,-0.0685 -7.08202,-0.30778 z" },
	{ key: "ExtraccionIndicada", valor: "-3.37953,-6.85737 4.19822,-8.14263 4.19822,-8.142631 -4.19822,-8.14263 -4.19822,-8.14263 3.37953,-6.85737 3.37953,-6.85737 4.12356,8.33395 4.12355,8.33396 4.10955,-8.33251 4.10954,-8.3325 3.38737,6.8046 3.38737,6.80461 -4.19489,8.19394 -4.19489,8.19395 4.19489,8.193951 4.19489,8.19394 -3.38737,6.80461 -3.38737,6.8046 -4.10954,-8.3325 -4.10955,-8.33251 -4.12355,8.33396 -4.12356,8.33395 z" },
	{ key: "ConEndodoncia", valor: "c 0,-0.87415 14.72701,-59.32335 14.99273,-59.50378 0.14144,-0.096 0.57041,1.156 0.95325,2.78233 0.38284,1.62634 3.70161,14.94403 7.37505,29.59488 3.67343,14.65085 6.67897,26.85834 6.67897,27.12776 0,0.27599 -6.54855,0.48985 -15,0.48985 -8.479,0 -15,-0.21347 -15,-0.49104 z m 25.4717,-5.43048 c 0,-0.19627 -1.82626,-7.65548 -4.05835,-16.57604 -2.23209,-8.92055 -4.55638,-18.27099 -5.16509,-20.77876 -0.60871,-2.50776 -1.17776,-4.56412 -1.26455,-4.5697 -0.0868,-0.006 -0.38704,1.01147 -0.66722,2.2601 -0.28018,1.24862 -2.59715,10.60985 -5.14881,20.80272 -2.55166,10.19288 -4.63938,18.68686 -4.63938,18.87552 0,0.18865 4.71227,0.34301 10.4717,0.34301 5.75943,0 10.4717,-0.16058 10.4717,-0.35685 z" },
	{ key: "Protesis", valor: "c -0.76749,-1.60173 -0.70043,-6.07016 0.11954,-7.96518 0.61041,-1.41073 1.45989,-1.4993 14.37886,-1.4993 13.42381,0 13.74401,0.037 14.35353,1.65766 0.73422,1.95226 0.82255,5.78777 0.17579,7.63367 -0.39481,1.12681 -2.10246,1.27755 -14.47308,1.27755 -11.36794,0 -14.12571,-0.20926 -14.55464,-1.1044 z m 0.17215,15.34014 c -0.76749,-1.60173 -0.70043,-6.07016 0.11954,-7.96518 0.61041,-1.41072 1.45989,-1.49929 14.37886,-1.49929 13.42381,0 13.74401,0.037 14.35353,1.65765 0.73422,1.95227 0.82255,5.78778 0.17579,7.63367 -0.39481,1.12682 -2.10246,1.27756 -14.47308,1.27756 -11.36794,0 -14.12571,-0.20927 -14.55464,-1.10441 z" },
	{ key: "NecrosisPulpar", valor: "c 0,-0.87415 14.72701,-59.32335 14.99273,-59.50378 0.14144,-0.096 0.57041,1.156 0.95325,2.78233 0.38284,1.62634 3.70161,14.94403 7.37505,29.59488 3.67343,14.65085 6.67897,26.85834 6.67897,27.12776 0,0.27599 -6.54855,0.48985 -15,0.48985 -8.479,0 -15,-0.21347 -15,-0.49104 z m 25.4717,-5.43048 c 0,-0.19627 -1.82626,-7.65548 -4.05835,-16.57604 -2.23209,-8.92055 -4.55638,-18.27099 -5.16509,-20.77876 -0.60871,-2.50776 -1.17776,-4.56412 -1.26455,-4.5697 -0.0868,-0.006 -0.38704,1.01147 -0.66722,2.2601 -0.28018,1.24862 -2.59715,10.60985 -5.14881,20.80272 -2.55166,10.19288 -4.63938,18.68686 -4.63938,18.87552 0,0.18865 4.71227,0.34301 10.4717,0.34301 5.75943,0 10.4717,-0.16058 10.4717,-0.35685 z" },
	{ key: "ProtesisIndicada", valor: "c -0.76749,-1.60173 -0.70043,-6.07016 0.11954,-7.96518 0.61041,-1.41073 1.45989,-1.4993 14.37886,-1.4993 13.42381,0 13.74401,0.037 14.35353,1.65766 0.73422,1.95226 0.82255,5.78777 0.17579,7.63367 -0.39481,1.12681 -2.10246,1.27755 -14.47308,1.27755 -11.36794,0 -14.12571,-0.20926 -14.55464,-1.1044 z m 0.17215,15.34014 c -0.76749,-1.60173 -0.70043,-6.07016 0.11954,-7.96518 0.61041,-1.41072 1.45989,-1.49929 14.37886,-1.49929 13.42381,0 13.74401,0.037 14.35353,1.65765 0.73422,1.95227 0.82255,5.78778 0.17579,7.63367 -0.39481,1.12682 -2.10246,1.27756 -14.47308,1.27756 -11.36794,0 -14.12571,-0.20927 -14.55464,-1.10441 z" },
	{ key: "ClinicamenteAusente", valor: "c -1.64184,-0.817301 -1.82292,-3.224933 -1.82292,-24.237111 0,-22.399025 0.0846,-23.36418 2.12099,-24.194391 1.16653,-0.475583 3.01281,-0.78734 4.10283,-0.692772 4.52812,0.392853 4.75765,1.611264 4.77463,25.345525 0.0157,22.044828 -0.0475,22.730051 -2.1867,23.693767 -2.73493,1.232051 -4.63799,1.255198 -6.98883,0.08498 z" },
	{ key: "CoronaDesadaptada", valor: "c -0.34685,-0.27843 -0.44295,-0.80577 -1.92288,-10.55222 -0.77636,-5.11285 -1.42757,-9.34303 -1.44713,-9.4004 -0.0196,-0.0574 -0.20942,0.0681 -0.42191,0.27873 -0.54181,0.53716 -1.34633,0.56041 -1.90254,0.055 -0.75907,-0.68977 -1.37024,-2.14812 -1.71891,-4.10159 -0.77112,-4.32031 0.0319,-9.45246 1.71891,-10.98544 0.53626,-0.4873 1.30708,-0.4873 1.84334,0 0.72016,0.65441 1.34815,2.10154 1.68331,3.87896 0.22142,1.17424 0.2915,1.89708 0.3341,3.44621 l 0.0336,1.22224 2.13815,0.40766 c 1.17597,0.22421 2.15665,0.35328 2.17927,0.28682 0.0507,-0.1489 1.44703,-8.7746 1.44703,-8.9388 0,-0.065 -0.0992,-0.2794 -0.22045,-0.4765 -0.12124,-0.19711 -0.34352,-0.71936 -0.49394,-1.16056 -1.08105,-3.17083 -0.69877,-8.1123 0.77808,-10.05787 1.58559,-2.0888 3.49886,0.37358 3.77586,4.85956 0.15841,2.56535 -0.35815,5.31833 -1.23348,6.57386 -0.18202,0.26106 -0.33102,0.51601 -0.33113,0.56655 -1.5e-4,0.0822 1.84289,9.29933 1.88019,9.4028 0.008,0.0234 1.08354,-0.42965 2.38913,-1.00673 1.88437,-0.8329 2.37172,-1.09166 2.36364,-1.25496 -0.006,-0.11315 -0.0391,-0.63654 -0.0745,-1.16309 -0.20627,-3.07201 0.5995,-6.24951 1.90388,-7.50786 1.81434,-1.7503 3.88002,0.33099 4.47045,4.50425 0.16021,1.1324 0.16021,3.06079 0,4.19319 -0.32103,2.2691 -1.09369,4.01132 -2.1309,4.8048 -0.24008,0.18367 -0.52066,0.25479 -1.00514,0.25479 -0.71871,0 -1.06932,-0.18559 -1.61479,-0.85476 -0.15846,-0.19441 -0.29238,-0.3365 -0.29759,-0.31576 -0.005,0.0207 -0.93729,4.97473 -2.07129,11.00886 -1.13401,6.03413 -2.1174,11.14437 -2.18531,11.35608 -0.27744,0.86494 -0.0929,0.83531 -5.11538,0.82115 -2.62521,-0.007 -4.65759,-0.0694 -4.75171,-0.14496 z m 9.02773,-5.84821 0.25631,-1.41686 -3.9394,-0.10042 c -2.16667,-0.0552 -4.17609,-0.0791 -4.46538,-0.0531 l -0.52599,0.0473 0.15519,1.00389 c 0.0854,0.55214 0.18709,1.21364 0.22608,1.46999 l 0.0709,0.4661 h 3.98299 3.98299 z m 2.04868,-10.90218 c 0.49673,-2.63441 0.89188,-4.82108 0.87812,-4.85926 -0.0138,-0.0382 -0.92732,0.32672 -2.03012,0.81091 -1.1028,0.48419 -2.09463,0.87997 -2.20407,0.87953 -0.31702,-10e-4 -0.54199,-0.3872 -0.74641,-1.28036 -0.1007,-0.43997 -0.19822,-0.83222 -0.21672,-0.87166 -0.0185,-0.0394 -0.39138,-1.86309 -0.82864,-4.05256 -0.43726,-2.18946 -0.81119,-3.93194 -0.83095,-3.87217 -0.0198,0.0598 -0.33087,1.94796 -0.69136,4.19597 -0.44875,2.79836 -0.70242,4.20887 -0.80442,4.47283 -0.27995,0.7245 -0.34136,0.72946 -2.93341,0.23683 -1.31515,-0.24996 -2.40344,-0.42048 -2.41842,-0.37894 -0.015,0.0415 0.29262,2.18133 0.68353,4.75508 l 0.71076,4.67954 2.87088,0.039 c 1.57899,0.0214 3.94801,0.0381 5.26449,0.037 l 2.39359,-0.002 z m -15.24019,-7.45594 c 0.2043,-0.28913 0.37655,-0.85007 0.59291,-1.93089 0.24677,-1.23267 -0.0467,-3.19035 -0.60001,-4.00265 -0.30245,-0.44402 -0.90137,-0.44402 -1.20382,0 -0.39634,0.58185 -0.69057,1.85045 -0.69057,2.97745 0,1.39936 0.38743,2.75076 0.93025,3.24483 0.21818,0.19858 0.73558,0.0448 0.97124,-0.28874 z m 21.02087,-2.59991 c 0.51914,-0.5449 0.86322,-1.65689 0.78391,-2.53344 -0.0557,-0.61607 -0.43128,-1.54139 -0.77775,-1.91637 -0.44155,-0.47788 -1.21219,-0.47972 -1.6482,-0.004 -0.51472,0.56166 -0.8628,1.67419 -0.78676,2.51461 0.0459,0.50765 0.32202,1.33494 0.55601,1.66609 0.54581,0.77242 1.29321,0.88139 1.87279,0.27306 z m -11.07082,-11.85921 c 0.2106,-0.34119 0.40113,-1.11036 0.40113,-1.61932 0,-0.50424 -0.18995,-1.27719 -0.39585,-1.61077 -0.21821,-0.35353 -0.61237,-0.46656 -0.8935,-0.25622 -0.25925,0.19397 -0.54802,0.90872 -0.60022,1.48566 -0.076,0.84055 0.1545,1.80899 0.51955,2.18245 0.26494,0.27104 0.74497,0.18097 0.96889,-0.1818 z" },
	{ key: "RecesionGingival", valor: "c 0.004,-0.12649 1.05811,-3.48142 2.34214,-7.45542 l 2.33463,-7.22544 12.87478,-0.0751 12.87478,-0.0751 2.21962,7.15603 c 1.22079,3.93582 2.27445,7.32493 2.34148,7.53134 0.0937,0.28874 -0.32112,0.35541 -1.79857,0.28902 l -1.92044,-0.0863 -1.71599,-5.49134 -1.71599,-5.49134 h -10.32078 -10.32078 l -1.71599,5.49134 -1.716,5.49134 -1.8852,0.0854 c -1.03687,0.047 -1.88183,-0.018 -1.87769,-0.1445 z" },
	{ key: "Sano", valor: "c 5.00658,-12.7267 9.10288,-23.42882 9.10288,-23.78252 0,-0.35369 -0.73551,-2.3252 -1.63446,-4.38114 l -1.63447,-3.73807 -8.46113,21.53699 -8.46114,21.53699 -3.59594,-8.85649 -3.59593,-8.8565 -1.44575,4.11145 c -1.28658,3.65884 -1.38633,4.27594 -0.90611,5.60562 0.2968,0.8218 2.55053,6.76543 5.0083,13.20806 l 4.46867,11.71388 1.0261,-2.47942 c 0.56436,-1.36367 5.12239,-12.89216 10.12898,-25.61885 z" },
	{ key: "CoronaAdaptada", valor: "c -0.34685,-0.27843 -0.44295,-0.80577 -1.92288,-10.55222 -0.77636,-5.11285 -1.42757,-9.34303 -1.44713,-9.4004 -0.0196,-0.0574 -0.20942,0.0681 -0.42191,0.27873 -0.54181,0.53716 -1.34633,0.56041 -1.90254,0.055 -0.75907,-0.68977 -1.37024,-2.14812 -1.71891,-4.10159 -0.77112,-4.32031 0.0319,-9.45246 1.71891,-10.98544 0.53626,-0.4873 1.30708,-0.4873 1.84334,0 0.72016,0.65441 1.34815,2.10154 1.68331,3.87896 0.22142,1.17424 0.2915,1.89708 0.3341,3.44621 l 0.0336,1.22224 2.13815,0.40766 c 1.17597,0.22421 2.15665,0.35328 2.17927,0.28682 0.0507,-0.1489 1.44703,-8.7746 1.44703,-8.9388 0,-0.065 -0.0992,-0.2794 -0.22045,-0.4765 -0.12124,-0.19711 -0.34352,-0.71936 -0.49394,-1.16056 -1.08105,-3.17083 -0.69877,-8.1123 0.77808,-10.05787 1.58559,-2.0888 3.49886,0.37358 3.77586,4.85956 0.15841,2.56535 -0.35815,5.31833 -1.23348,6.57386 -0.18202,0.26106 -0.33102,0.51601 -0.33113,0.56655 -1.5e-4,0.0822 1.84289,9.29933 1.88019,9.4028 0.008,0.0234 1.08354,-0.42965 2.38913,-1.00673 1.88437,-0.8329 2.37172,-1.09166 2.36364,-1.25496 -0.006,-0.11315 -0.0391,-0.63654 -0.0745,-1.16309 -0.20627,-3.07201 0.5995,-6.24951 1.90388,-7.50786 1.81434,-1.7503 3.88002,0.33099 4.47045,4.50425 0.16021,1.1324 0.16021,3.06079 0,4.19319 -0.32103,2.2691 -1.09369,4.01132 -2.1309,4.8048 -0.24008,0.18367 -0.52066,0.25479 -1.00514,0.25479 -0.71871,0 -1.06932,-0.18559 -1.61479,-0.85476 -0.15846,-0.19441 -0.29238,-0.3365 -0.29759,-0.31576 -0.005,0.0207 -0.93729,4.97473 -2.07129,11.00886 -1.13401,6.03413 -2.1174,11.14437 -2.18531,11.35608 -0.27744,0.86494 -0.0929,0.83531 -5.11538,0.82115 -2.62521,-0.007 -4.65759,-0.0694 -4.75171,-0.14496 z m 9.02773,-5.84821 0.25631,-1.41686 -3.9394,-0.10042 c -2.16667,-0.0552 -4.17609,-0.0791 -4.46538,-0.0531 l -0.52599,0.0473 0.15519,1.00389 c 0.0854,0.55214 0.18709,1.21364 0.22608,1.46999 l 0.0709,0.4661 h 3.98299 3.98299 z m 2.04868,-10.90218 c 0.49673,-2.63441 0.89188,-4.82108 0.87812,-4.85926 -0.0138,-0.0382 -0.92732,0.32672 -2.03012,0.81091 -1.1028,0.48419 -2.09463,0.87997 -2.20407,0.87953 -0.31702,-10e-4 -0.54199,-0.3872 -0.74641,-1.28036 -0.1007,-0.43997 -0.19822,-0.83222 -0.21672,-0.87166 -0.0185,-0.0394 -0.39138,-1.86309 -0.82864,-4.05256 -0.43726,-2.18946 -0.81119,-3.93194 -0.83095,-3.87217 -0.0198,0.0598 -0.33087,1.94796 -0.69136,4.19597 -0.44875,2.79836 -0.70242,4.20887 -0.80442,4.47283 -0.27995,0.7245 -0.34136,0.72946 -2.93341,0.23683 -1.31515,-0.24996 -2.40344,-0.42048 -2.41842,-0.37894 -0.015,0.0415 0.29262,2.18133 0.68353,4.75508 l 0.71076,4.67954 2.87088,0.039 c 1.57899,0.0214 3.94801,0.0381 5.26449,0.037 l 2.39359,-0.002 z m -15.24019,-7.45594 c 0.2043,-0.28913 0.37655,-0.85007 0.59291,-1.93089 0.24677,-1.23267 -0.0467,-3.19035 -0.60001,-4.00265 -0.30245,-0.44402 -0.90137,-0.44402 -1.20382,0 -0.39634,0.58185 -0.69057,1.85045 -0.69057,2.97745 0,1.39936 0.38743,2.75076 0.93025,3.24483 0.21818,0.19858 0.73558,0.0448 0.97124,-0.28874 z m 21.02087,-2.59991 c 0.51914,-0.5449 0.86322,-1.65689 0.78391,-2.53344 -0.0557,-0.61607 -0.43128,-1.54139 -0.77775,-1.91637 -0.44155,-0.47788 -1.21219,-0.47972 -1.6482,-0.004 -0.51472,0.56166 -0.8628,1.67419 -0.78676,2.51461 0.0459,0.50765 0.32202,1.33494 0.55601,1.66609 0.54581,0.77242 1.29321,0.88139 1.87279,0.27306 z m -11.07082,-11.85921 c 0.2106,-0.34119 0.40113,-1.11036 0.40113,-1.61932 0,-0.50424 -0.18995,-1.27719 -0.39585,-1.61077 -0.21821,-0.35353 -0.61237,-0.46656 -0.8935,-0.25622 -0.25925,0.19397 -0.54802,0.90872 -0.60022,1.48566 -0.076,0.84055 0.1545,1.80899 0.51955,2.18245 0.26494,0.27104 0.74497,0.18097 0.96889,-0.1818 z" }

];

function cambiarColor(enfermedadOdon, idGrupoDato, region) {
	//console.log("enfermedadOdont en cambiar color:", enfermedadOdon);
	var idGrupo = "";
	if (idGrupoDato === undefined) {
		
		idGrupo = $("#idPiezaClick").val();
	}
	else {
		
		idGrupo = "g" + idGrupoDato;
	}	
	if (enfermedadOdon.includes("Caries") || enfermedadOdon.includes("Resina") || enfermedadOdon.includes("Amalgama")) {
		if (region) {
			nombre = region + "-" + idGrupo.substring(1, 3);
			enfermedadOdon = enfermedadOdon + region;
		}
		else {
			nombre = enfermedadOdon.charAt(enfermedadOdon.length - 1) + "-" + idGrupo.substring(1, 3);
		}
		if (enfermedadOdon.includes("Caries")) {
			$("#" + nombre).attr("style", "fill:red");
		}
		else if (enfermedadOdon.includes("Resina")) {
			$("#" + nombre).attr("style", "fill:blue");
		}
		else if (enfermedadOdon.includes("Amalgama")) {
			$("#" + nombre).attr("style", "fill:black");
		}
		$("#" + nombre).attr("cambiocolor", "true");
		$("#" + nombre).attr("enfermedad", enfermedadOdon);
	}
	else if (enfermedadOdon.includes("Limpio")) {
		console.log('limpio');
		eliminarPath(idGrupo);
	}
	else {
		idPath = enfermedadOdon + "-" + idGrupo.substring(1, 3);
		console.log("nombe a poner:", idPath);
		agregarPath(idPath, idGrupo, enfermedadOdon);
	}
}

function eliminarPath(idGrupo) {
	var contador = $('#svg742')[0].children[3];
	for (var i = 0; i <= contador.childElementCount; i++) {
		if ($('#svg742')[0].children[3].children[i] !== undefined) {
			if ($('#svg742')[0].children[3].children[i].id.charAt(0) === "g") {
				let grupo = $('#svg742')[0].children[3].children[i].id;
				//console.log("grupo", grupo);
				if (grupo === idGrupo ) {
					console.log("ingreso al if idgrupo");
					let contadorGrupo = $('#svg742')[0].children[3].children[i].childElementCount;
					if (contadorGrupo === 6) {
						for (var l = 1; l < contadorGrupo; l++) {
							let id = $('#svg742')[0].children[3].children[i].children[l].id;
							if (id.charAt(0) !== "T") {
								let estilo = $('#svg742')[0].children[3].children[i].children[l].attributes.style.value;
								let color = estilo.split(":");
								if (color[1] !== "#ffffff") {
									$("#" + id).attr("style", "fill:#ffffff");
								}
							}
							

						}
					}
					else if (contadorGrupo > 6) {
						let elementosAEliminar = [];
						for (var j = 6; j < contadorGrupo; j++) {
							elementosAEliminar.push($('#svg742')[0].children[3].children[i].children[j]);
						}
						//console.log("elementos a eliminar");
						//console.log(elementosAEliminar);
						for (var k = 0; k < elementosAEliminar.length; k++) {
							$('#svg742')[0].children[3].children[i].removeChild(elementosAEliminar[k]);
						}
					}				

				}
				
			}

		}
	}

}

function agregarPath(idPath, idGrupo, enfermedad) {
	let d = "";
	for (var i = 0; i < enfermedadesOdontogramasSVG.length; i++) {
		if (enfermedad === enfermedadesOdontogramasSVG[i].key) {
			d = enfermedadesOdontogramasSVG[i].valor;
			break;
		}
	}
	const xm = obtenerMedida(idGrupo, "x", enfermedad);//"246.36398"; //obtenerMedida(idGrupo,"x");//"347.80274"; //"209.64356";//"247.13928"; x de T
	const ym = obtenerMedida(idGrupo, "y", enfermedad);// "329.36161";//obtenerMedida(idGrupo, "y");//"83.69396"; y de Lingual

	//console.log('mx', xm);
	//console.log('my', ym);
	const medida = xm + ', ' + ym;
	const d1 = "m " + medida + " " + d;

	let path = document.createElementNS("http://www.w3.org/2000/svg", "path");
	path.setAttribute("style", "fill: #ff0080; stroke-width: 0.39685088");
	path.setAttribute("id", idPath);
	path.setAttribute("d", d1);

	$('#svg742')[0].children[3].children[idGrupo].appendChild(path);

	if (enfermedad.includes("Indicado") || enfermedad.includes("Indicada") || enfermedad.includes("NecrosisPulpar") || enfermedad.includes("Desadaptada")) {
		$("#" + idPath).attr("style", "fill:red");
	}
	else if (enfermedad.includes("Limpio")) {
		$("#" + idPath).attr("style", "fill:white");
	}
	else if (enfermedad.includes("Sano")) {

		$("#" + idPath).attr("style", "fill:green");
	}
	else if (enfermedad.includes("RecesionGingival")) {
		$("#" + idPath).attr("style", "fill:#ff0080");
	}
	else {
		$("#" + idPath).attr("style", "fill:blue");
	}

	$("#" + idPath).attr("cambiocolor", "true");
	$("#" + idPath).attr("enfermedad", enfermedad);

}

function obtenerMedida(idGrupo, tipoMedida, enfermedad) {
	//console.log("idgr: tmed");
	//console.log(idGrupo, tipoMedida);
	let numero = idGrupo.substring(1, 3);
	let tipo = "";
	if (tipoMedida === "x") {
		tipo = "T";
	}
	else {
		if (enfermedad.includes("RecesionGingival")) {
			tipo = "B";
		}
		else {
			tipo = "L";
		}
	}

	let nombreObtener = tipo + "-" + numero;

	//console.log("nombre a obtener: ", nombreObtener);
	let dato = $('#svg742')[0].children[3].children[idGrupo].children[nombreObtener].attributes.d.value;
	//console.log("dato atributo d:", dato);
	let division = dato.substring(2, 20).split(",");
	let medidaX = division[0];
	let medidaY = division[1];
	let medidaGeneral = "";

	if (parseInt(numero) > 48) {
		if (tipoMedida === "x") {
			if (enfermedad.includes("Sellante")) {
				medidaGeneral = parseFloat(medidaX) - 7;
			}
			else if (enfermedad.includes("ExtraccionIndicada"))
			{
				medidaGeneral = parseFloat(medidaX) - 8;
			}
			else if (enfermedad.includes("ConEndodoncia")) {
				medidaGeneral = parseFloat(medidaX) - 8;
			}
			else if (enfermedad.includes("NecrosisPulpar")) {
				medidaGeneral = parseFloat(medidaX) - 8;
			}
			else if (enfermedad.includes("Corona")) {
				medidaGeneral = parseFloat(medidaX) - 3;
			}			
			else if (enfermedad.includes("Sano")) {
				medidaGeneral = parseFloat(medidaX) + 12;
			}
			else if (enfermedad.includes("RecesionGingival")) {
				medidaGeneral = parseFloat(medidaX) - 14;
			}
			else if (enfermedad.includes("Protesis")) {
				medidaGeneral = parseFloat(medidaX) - 10;
			}

			else {

				medidaGeneral = medidaX;
			}
		}
		else {
			if (enfermedad.includes("Sellante")) {
				medidaGeneral = parseFloat(medidaY) - 7;
			}
			else if (enfermedad.includes("Protesis")) {
				medidaGeneral = parseFloat(medidaY) - 26;
			}
			else if (enfermedad.includes("Sano")) {
				medidaGeneral = parseFloat(medidaY) -28;
			}			
			else if (enfermedad.includes("RecesionGingival")) {
				medidaGeneral = parseFloat(medidaY) - 8;
			}
			
			else {

				medidaGeneral = medidaY;
			}
		}

	}
	else if (parseInt(numero) <= 48) {
		//console.log("menor48");
		if (tipoMedida === "x") {
			if (enfermedad.includes("Sano")) {
				medidaGeneral = parseFloat(medidaX) + 24;
			}
			else if (enfermedad.includes("Sellante")) {
				medidaGeneral = parseFloat(medidaX) + 8;
			}
			else if (enfermedad.includes("ClinicamenteAusente")) {
				medidaGeneral = parseFloat(medidaX) + 10;
			}
			else if (enfermedad.includes("Corona")) {
				medidaGeneral = parseFloat(medidaX) + 10;
			}
			else if (enfermedad.includes("RecesionGingival")) {
				medidaGeneral = medidaX - 4;
			}
			else {
				medidaGeneral = medidaX;
			}
		}
		else {
			if (enfermedad.includes("RecesionGingival")) {
				medidaGeneral = medidaY;
			}
			else if (enfermedad.includes("Sano")) {
				medidaGeneral = parseFloat(medidaY) - 20;
			}
			else if (enfermedad.includes("Protesis")) {
				medidaGeneral = parseFloat(medidaY) - 17;
			}
			else {
				medidaGeneral = parseFloat(medidaY) + 10;
			}
		}
	}
	//console.log("medida q regresa: ", medidaGeneral);
	return medidaGeneral.toString();
}

//guardar Datos.

function GuardarDatosOdontograma(accion) {
	//var data = new FormData();
	//data.append('imagen', $('#svg742')[0].children[3]);

	//const codigoPaciente = $('#CodigoPaciente')[0].value;
	//const codigoPersonal = $('#CodigoPersonal')[0].value;
	let codigoCitaOdontologica = $('#Codigo')[0].value;
	//let codigo = null;
	//if (accion === "editar") {
	//	codigoCitaOdontologica = $('#CodigoCitaOdontologica')[0].value;
	//	codigo = $('#Codigo')[0].value;
	//}
	
	//leer los detalles de pieza.
	var listaOdontogramaDetalle = [];
	const odontogramaDato = $('#svg742')[0].children[3];
	for (var j = 0; j < odontogramaDato.childElementCount; j++) {
		if (odontogramaDato.children[j].id.charAt(0) === "g") {
			datos = odontogramaDato.children[j]; 
			for (var i = 0; i < datos.childElementCount; i++) {				
				
				dato = odontogramaDato.children[j].children[i];
				cambioColor = false;
				if (dato.attributes.cambiocolor !== undefined) {
					cambioColor = dato.attributes.cambiocolor.value;
				}
				if (cambioColor) {
					let pieza = dato.id.substring(2, 4);
					let region = "";
					let enfermedad = dato.attributes.enfermedad.value;
					if (enfermedad.includes("Caries") || enfermedad.includes("Resina") || enfermedad.includes("Amalgama")) {
						pieza = dato.id.substring(2, 4);
						region = dato.id.substring(0, 1);						
							enfermedad = dato.attributes.enfermedad.value.substring(0, dato.attributes.enfermedad.value.length - 1);
					}
					else {
						pieza = dato.id.split("-")[1];
						region = "T";
						
					}
					diagnosticoDato = dato.style.fill;
					diagnostico = "";
					if (diagnosticoDato === "red") {
						diagnostico = "Recomendado";
					}
					else if (diagnosticoDato === "blue") {
						diagnostico = "Diagnosticado";
					}
					else if (diagnosticoDato === "green") {
						diagnostico = "Sano";
					}
					
					else {
						diagnostico = enfermedad;
					}

					var detalle = { Pieza: pieza, Region: region, Enfermedad: enfermedad, Valor: true, Diagnostico: diagnostico };

					listaOdontogramaDetalle.push(detalle);
				}
			}

		}

	}

	console.log(listaOdontogramaDetalle);
	var odontograma = [
		{
			CodigoCitaOdontologica: codigoCitaOdontologica,
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
	console.log(odontograma);
	guardarOdontogramaController(url, odontograma);

}

//dataType: 'json',
function guardarOdontogramaController(url, odontograma) {

	$.ajax({
		type: 'POST',
		url: url,		
		data: { odontograma },
		success: function (c) {
			console.log("response: ", c);
			//respuesta(c);
		}
	});

}

//function respuesta(c) {
//	if (c === "Save") {
//		SuccessAlert("Guardados", "/../Odontogramas");
//	}
//	else {
//		ErrorAlert(c);
//	}
//}

///////////////////////////////////////////////////////////PRUEBAS////////////////////77


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

function cargarColorAlEditar(response) {	
	var detalle = JSON.parse(response);
	console.log(detalle);
	console.log(detalle.length);

	for (let i = 0; i < detalle.length; i++) {
		let nombre = detalle[i].Region + "-" + detalle[i].Pieza;
		let enfermedad = detalle[i].Enfermedad;
		let diagnostico = detalle[i].Diagnostico;

		cambiarColor(enfermedad, detalle[i].Pieza, detalle[i].Region );


		///ojooooooooooooooooooooooooOJO   OJOJOJOJO
		//hayq ue revisar cuadno sean los otros tipos de enfermedad como sellenate
		//console.log(nombre);
		//if (enfermedad.includes("Caries") || enfermedad.includes("Resina") || RecesionGingival.includes("Amalgama")) {
		//	if (diagnostico === "Diagnosticado") {
		//		$("#" + nombre).attr("style", "fill:blue");
		//	}
		//	else if (diagnostico === "Recomendado") {
		//		$("#" + nombre).attr("style", "fill:red");
		//	}
		//	else if (diagnostico === "Amalgama") {
		//		$("#" + nombre).attr("style", "fill:black");
		//	}
		//}
		//else {

		//if (diagnostico === "Sano") {
		//		$("#" + nombre).attr("style", "fill:green");
		//	}

		//	else if (diagnostico === "RecesionGingival") {
		//		$("#" + nombre).attr("style", "fill:#ff0080");
		//	}
		//}
		//$("#" + nombre).attr("cambiocolor", "true");
		//$("#" + nombre).attr("enfermedad", enfermedad);

	}
}

//-----------PACIENTE----importar datos  /////

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
				paging: false,
				ordering: false,
				pagingType: false,
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

function mostrarCamposSegunSeleccionado(response) {
	var sure = $(response).find("option:selected").val();
	console.log('sure', sure);
	if (sure === undefined) {
		sure = $('#tipoPaciente')[0].value;
	}
	//var sure = $('#tipoPaciente')[0].selectedOptions[0].value;
	console.log(sure);
	if (sure === "0") {
		$('#datosLaborales').hide();
		$('#datosEstudiantiles').hide();
	}
	if (sure === "E") {
		$('#datosLaborales').hide();
		$('#datosEstudiantiles').show();
	}
	if (sure === "EB") {
		$('#datosLaborales').hide();
		$('#datosEstudiantiles').show();
	}
	if (sure === "EC") {
		$('#datosLaborales').hide();
		$('#datosEstudiantiles').show();
	}
	if (sure === "EN") {
		$('#datosLaborales').hide();
		$('#datosEstudiantiles').show();
	}
	if (sure === "D") {
		$('#datosLaborales').show();
		$('#datosEstudiantiles').hide();
	}
	if (sure === "PA") {
		$('#datosLaborales').show();
		$('#datosEstudiantiles').hide();
	}

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




