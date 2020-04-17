function ErrorAlert(msg) {
	//$("#loading").modal("hide");
	swal({
		title: "HC-Odontológicas",
		text: msg,
		icon: "error",

		buttons: {
			cancel: false,
			confirm: {
				text: "Aceptar",
				className: "btn-primary",
				closeModal: true
			}
		}
	});
}

function SuccessAlert(msg, url) {
	if (!msg.includes("planilla"))
		msg = "Datos " + msg + " Correctamente";
	swal({
		title: "HC-Odontológicas",
		text: msg,
		icon: "success",
		buttons: {
			confirm: {
				text: "Aceptar",
				className: "btn-primary",
				closeModal: true
			}
		}
	}).then(function () {
		//$("#loading").modal();
		window.location.href = url
	});
}

function WarningAlert(msg, url) {
	swal({
		title: "HC-Odontológicas",
		text: msg,
		icon: "warning",
		buttons: {
			confirm: {
				text: "Aceptar",
				className: "btn-primary",
				closeModal: true
			}
		}
	}).then(function () {
		//$("#loading").modal();
		window.location.href = url
	});
}

function WarningAlert2btn(msg, url, tipo) {
	swal({
		title: "HC-Odontológicas",
		text: msg,
		icon: "warning",
		buttons: {
			cancel: {
				text: "Cancelar",
				visible: true,
				className: "btn-default",
				closeModal: true
			},
			confirm: {
				text: "Aceptar",
				className: "btn-primary",
				closeModal: true
			}
		}
	}).then(function (isConfirm) {
		if (isConfirm) {
			//$("#loading").modal();
			if (tipo == "salir") {
				var form = document.getElementById("logoutForm");
				if (typeof (form) !== "undefined")
					form.submit();
			} else
				window.location.href = url

		}
	});
}

function Guardar(url) {
	swal({
		title: "GUARDAR CAMPOS",
		confirmButtonText: 'ACEPTAR',
		html: true
	}, function () {
		if (url != null && url.trim() != "") {
			//$("#loading").modal();
			window.top.location.href = url;
		}
	})
}


//importar datos

function AdvertenciaGuardarImportados(urlIndex, url) {
	swal({
		title: "ABAKO",
		text: "¿Está seguro de guardar los datos?",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "Aceptar",
		cancelButtonText: "Cancelar",
		closeOnConfirm: true
	}, function (result) {
		if (result) {
			var data = new FormData($('#Importar')[0]);
			data.append('Documento', $('#Documento')[0].files[0]);
			var opts = {
				url: url,
				data: data,
				cache: false,
				contentType: false,
				processData: false,
				method: 'POST',
				type: 'POST',
				success: function (response) {
					if (response === "Save") {
						toastr.options = {
							closeButton: true,
							debug: false,
							progressBar: true,
							positionClass: 'toast-top-right',
							onclick: null,
							timeOut: 500
						};
						toastr.options.onHidden = function () { window.location.href = urlIndex; };
						toastr.success("Datos guardados correctamente", "Tipo Impuesto");
					}
					else {
						toastr.error(response, "Tipo Impuesto");
						setTimeout(function () {
							Ladda.stopAll();
						});
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
	}
	);
}

function AdvertenciaGuardarImportadosError() {
	swal({
		title: "ABAKO",
		text: "Datos con errores",
		type: "error",
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "Aceptar",
		showCancelButton: false,

		//cancelButtonText: "Cancelar",
		closeOnConfirm: false

	});
}
