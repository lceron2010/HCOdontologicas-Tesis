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