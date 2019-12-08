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
				data: {Codigo},
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