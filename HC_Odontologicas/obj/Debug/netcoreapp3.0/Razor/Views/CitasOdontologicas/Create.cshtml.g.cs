#pragma checksum "C:\Users\Dario\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\CitasOdontologicas\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ed9363127a1b2a7e347fbc35ef253c39a6bf2058"
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
#line 1 "C:\Users\Dario\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\_ViewImports.cshtml"
using HC_Odontologicas;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Dario\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\_ViewImports.cshtml"
using HC_Odontologicas.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ed9363127a1b2a7e347fbc35ef253c39a6bf2058", @"/Views/CitasOdontologicas/Create.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c422712020532d3975c1726980cbdf05484be000", @"/Views/_ViewImports.cshtml")]
    public class Views_CitasOdontologicas_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HC_Odontologicas.Models.CitaOdontologica>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/dhtmlx/codebase/dhtmlxscheduler.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("charset", new global::Microsoft.AspNetCore.Html.HtmlString("utf-8"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/dhtmlx/codebase/dhtmlxscheduler_material.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/dhtmlx/codebase/locale/locale_es.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 3 "C:\Users\Dario\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\CitasOdontologicas\Create.cshtml"
  
	ViewData["Title"] = "Create";
	Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<style>

	html, body {
		margin: 0px;
		padding: 0px;
	}
</style>

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
#line 33 "C:\Users\Dario\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\CitasOdontologicas\Create.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ed9363127a1b2a7e347fbc35ef253c39a6bf20586797", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "ed9363127a1b2a7e347fbc35ef253c39a6bf20587894", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ed9363127a1b2a7e347fbc35ef253c39a6bf20589244", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
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
		scheduler.config.first_hour = 8;
		scheduler.config.last_hour = 18;

		var paciente_opts = cargarPacientes();
		var doctor_opts = cargarDoctores();

		scheduler.config.lightbox.sections = [
			{ name: ""Paciente"", height: 40, map_to: ""paciente"", type: ""select"", options: paciente_opts },
			{ name: ""Doctor"", height: 40, map_to: ""doctor"", type: ""select"", options: doctor_opts },
			{ name: ""Observación"", height: 50, map_to: ""observaciones"", type: ""textarea"", focus: true },
			{ name: ""Tiempo"", height: 72, type: ""time"", map_to: ""auto"" },

		];

		scheduler.config.xml_date = ""%Y-%m-%d %H:%i"";
		//para la etiqueta del evento despues de guardar.
		scheduler.attachEvent(""onEventSave"", function (id, ev, is_new) {
			if (ev.paciente != '') {
				ev.text = ""P"";
				//console");
                WriteLiteral(@".log(ev.paciente);
				var arr = ev.paciente.split(',');
				for (var i = 0; i < arr.length; i++) {
					ev.text += (ev.text.length) ? ("": "" + getLabelPaciente(arr[i])) : (getLabelPaciente(arr[i]));
				}
			}

			if (ev.doctor != '') {
				ev.text += ""<br>D"";
				var arr = ev.doctor.split(',');
				for (var i = 0; i < arr.length; i++) {
					ev.text += (ev.text.length) ? ("": "" + getLabelDoctor(arr[i])) : (getLabelDoctor(arr[i]));
				}
			}

			ev.color = obtenerColor(ev.doctor);
			return true;
		})

		function getLabelPaciente(key) {
			for (var i = 0; i < paciente_opts.length; i++) {
				if (key == paciente_opts[i].key) {
					var nombre = paciente_opts[i].label.split("" "");
					console.log(nombre);
					return nombre[0] + "" "" + nombre[2];
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
			//llenar arreglo de color");
                WriteLiteral(@"es.
			var colores = [				
				{ key: 0, label: '#338A28' },
				{ key: 1, label: '#BC61DA' },
				{ key: 2, label: '#F3C13C' },
				{ key: 3, label: '#98948B' },
				{ key: 4, label: '#3AE7AB' }
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
		//para mapear los datos antes de mostrar

		scheduler.attachEvent(""onEventLoading"", function (ev) {

			if (ev.paciente != '') {
				ev.text = ""P"";
				console.log(ev.paciente);
				var arr = ev.paciente.split(',');
				for (var i = 0; i < arr.length; i++) {
					ev.text += (ev.text.length) ? ("": "" + getLabelPaciente(arr[i])) : (getLabelPaciente(arr[i]));
				}
			}

			if (ev.doctor != '') {
				ev.text += ""<br>D"";
				var arr = ev.doctor.split(',');
				for (var i = 0; i < arr.length; ");
                WriteLiteral(@"i++) {
					ev.text += (ev.text.length) ? ("": "" + getLabelDoctor(arr[i])) : (getLabelDoctor(arr[i]));
				}
			}

			ev.color = obtenerColor(ev.doctor);
			return true;
		});


		//iniciar el calendario en la fecha de hoy.
		var d = new Date();
		scheduler.init('scheduler_here', new Date(d.getFullYear(), d.getMonth(), d.getDate()), ""week"");
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
