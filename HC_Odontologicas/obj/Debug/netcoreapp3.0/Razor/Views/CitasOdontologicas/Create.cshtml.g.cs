#pragma checksum "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\CitasOdontologicas\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d89a0fbc6dfc446beed97f939c64e87cd8dd5623"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CitasOdontologicas_Create), @"mvc.1.0.view", @"/Views/CitasOdontologicas/Create.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\_ViewImports.cshtml"
using HC_Odontologicas;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\_ViewImports.cshtml"
using HC_Odontologicas.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d89a0fbc6dfc446beed97f939c64e87cd8dd5623", @"/Views/CitasOdontologicas/Create.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c422712020532d3975c1726980cbdf05484be000", @"/Views/_ViewImports.cshtml")]
    public class Views_CitasOdontologicas_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HC_Odontologicas.Models.CitaOdontologica>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/dhtmlx/codebase/locale/locale_es.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("charset", new global::Microsoft.AspNetCore.Html.HtmlString("utf-8"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\CitasOdontologicas\Create.cshtml"
  
	ViewData["Title"] = "Create";
	Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<style>\r\n\r\n\thtml, body {\r\n\t\tmargin: 0px;\r\n\t\tpadding: 0px;\r\n\t}\r\n</style>\r\n\r\n");
            WriteLiteral(@"


<div id=""scheduler_here"" class=""dhx_cal_container"" style='width:100%; height:100vh;'>
	<div class=""dhx_cal_navline"">
		<div class=""dhx_cal_prev_button"">&nbsp;</div>
		<div class=""dhx_cal_next_button"">&nbsp;</div>
		<div class=""dhx_cal_today_button""></div>
		<div class=""dhx_cal_date""></div>
		<div class=""dhx_cal_tab"" name=""day_tab""></div>
		<div class=""dhx_cal_tab"" name=""week_tab""></div>
		<div class=""dhx_cal_tab"" name=""month_tab""></div>
	</div>
	<div class=""dhx_cal_header""></div>
	<div class=""dhx_cal_data""></div>
</div>



");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 107 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\CitasOdontologicas\Create.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t<script src=\"https://cdn.dhtmlx.com/scheduler/edge/dhtmlxscheduler.js\"></script>\r\n\t<link rel=\"stylesheet\" href=\"https://cdn.dhtmlx.com/scheduler/edge/dhtmlxscheduler_material.css\" type=\"text/css\" charset=\"utf-8\">\r\n\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d89a0fbc6dfc446beed97f939c64e87cd8dd56235624", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"

	<script>
		scheduler.config.multi_day = true;

		scheduler.config.event_duration = 30;
		scheduler.config.auto_end_date = true;
		scheduler.config.details_on_create = true;
		scheduler.config.details_on_dblclick = true;

		var paciente_opts = cargarPacientes();
		var doctor_opts = cargarDoctores();

		scheduler.config.lightbox.sections = [
			{ name: ""Pacientes"", height: 40, map_to: ""Paciente"", type: ""select"", options: paciente_opts },
			{ name: ""Doctores"", height: 40, map_to: ""Doctor"", type: ""select"", options: doctor_opts },
			{ name: ""Observación"", height: 50, map_to: ""Observaciones"", type: ""textarea"", focus: true },
			{ name: ""Tiempo"", height: 72, type: ""time"", map_to: ""auto"" },

		];

		scheduler.config.xml_date = ""%Y-%m-%d %H:%i"";

		//para la etiqueta del evento.
		scheduler.attachEvent(""onEventSave"", function (id, ev, is_new) {
			if (ev.Paciente != '') {
				ev.text = ""P"";
				var arr = ev.Paciente.split(',');
				for (var i = 0; i < arr.length; i++) {
					ev.text +");
                WriteLiteral(@"= (ev.text.length) ? ("": "" + getLabelPaciente(arr[i])) : (getLabelPaciente(arr[i]));
				}
			}

			if (ev.Doctor != '') {
				ev.text += ""<br>D"";
				var arr = ev.Doctor.split(',');
				for (var i = 0; i < arr.length; i++) {
					ev.text += (ev.text.length) ? ("": "" + getLabelDoctor(arr[i])) : (getLabelDoctor(arr[i]));
				}
			}

			ev.color = obtenerColor(ev.Doctor);
			return true;
		})

		function getLabelPaciente(key) {
			for (var i = 0; i < paciente_opts.length; i++) {
				console.log(""getLabelPaciente label:"",paciente_opts[i].label);
				if (key == paciente_opts[i].key) { //ce ti laura
					var nombre = paciente_opts[i].label.split("" "");
					console.log(nombre);
					return nombre[0] + "" "" +nombre[2];
				}
			}
		}

		function getLabelDoctor(key) {
			for (var i = 0; i < doctor_opts.length; i++) {
				if (key == doctor_opts[i].key)
					return doctor_opts[i].label;
			}
		}


		function obtenerColor(key) {
			//llenar arreglo de colores.
			var colores = [
				{");
                WriteLiteral(@" key: 0, label: '#835134'},
				{ key: 1, label: '#338A28' },
				{ key: 2, label: '#BC61DA' },
				{ key: 3, label: '#F3C13C' },
				{ key: 4, label: '#98948B' },
				{ key: 5, label: '#3AE7AB' }
			]

			var coloresDoctor = [];
			for (var i = 0; i < doctor_opts.length; i++) {
				coloresDoctor.push({ key: doctor_opts[i].key, label: colores[i].label });
			}

			for (var i = 0; i < doctor_opts.length; i++) {
				if (key == coloresDoctor[i].key)
					return coloresDoctor[i].label;
			}
		}

		//iniciar el calendario en la fecha de hoy.
		var d = new Date();
		scheduler.init('scheduler_here', new Date(d.getFullYear(), d.getMonth(), d.getDate()), ""week"");


		//scheduler.parse(small_events_list, ""json"");

		scheduler.load(""/api/events"", ""json"");



		// connect backend to scheduler
		var dp = new dataProcessor(""/api/events"");
		dp.init(scheduler);
		// set data exchange mode
		dp.setTransactionMode(""REST"");



	</script>

");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HC_Odontologicas.Models.CitaOdontologica> Html { get; private set; }
    }
}
#pragma warning restore 1591
