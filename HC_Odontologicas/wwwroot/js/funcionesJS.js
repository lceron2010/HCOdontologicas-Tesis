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