#pragma checksum "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1dfe883582347446e2159beaf8915b7d32229a25"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Odontogramas_Create), @"mvc.1.0.view", @"/Views/Odontogramas/Create.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1dfe883582347446e2159beaf8915b7d32229a25", @"/Views/Odontogramas/Create.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c422712020532d3975c1726980cbdf05484be000", @"/Views/_ViewImports.cshtml")]
    public class Views_Odontogramas_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HC_Odontologicas.Models.Odontograma>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "CodigoCitaOdontologica", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("control-label"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "CodigoPaciente", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "CodigoPersonal", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
  
	ViewData["Title"] = "Create";
	Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Create</h1>\r\n\r\n<h4>Odontograma</h4>\r\n<hr />\r\n<div class=\"row\">\r\n\t<div class=\"col-lg-12 col-md-12 col-sm-12 col-xs-12\">\r\n\t\t");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1dfe883582347446e2159beaf8915b7d32229a256765", async() => {
                WriteLiteral("\r\n\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1dfe883582347446e2159beaf8915b7d32229a257029", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#nullable restore
#line 15 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary = global::Microsoft.AspNetCore.Mvc.Rendering.ValidationSummary.ModelOnly;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-summary", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "1dfe883582347446e2159beaf8915b7d32229a258726", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
#nullable restore
#line 16 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CodigoCitaOdontologica);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Name = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "1dfe883582347446e2159beaf8915b7d32229a2510785", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
#nullable restore
#line 17 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.Codigo);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n\t\t\t<fieldset>\r\n\t\t\t\t<legend class=\"titulo text-uppercase\" style=\"font-size:inherit\">Datos informativos</legend>\r\n\t\t\t\t<div class=\"row\">\r\n\t\t\t\t\t<div class=\"col-lg-4 col-md-4 col-sm-4 col-xs-12\">\r\n\t\t\t\t\t\t<div class=\"form-group\">\r\n\t\t\t\t\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1dfe883582347446e2159beaf8915b7d32229a2512900", async() => {
                    WriteLiteral("Paciente*");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#nullable restore
#line 24 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CodigoPaciente);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1dfe883582347446e2159beaf8915b7d32229a2514573", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
#nullable restore
#line 25 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CodigoPaciente);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
#nullable restore
#line 25 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items = ViewBag.CodigoPaciente;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-items", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Name = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1dfe883582347446e2159beaf8915b7d32229a2516910", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
#nullable restore
#line 26 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CodigoPaciente);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"col-lg-4 col-md-4 col-sm-4 col-xs-12\">\r\n\t\t\t\t\t\t<div class=\"form-group\">\r\n\t\t\t\t\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1dfe883582347446e2159beaf8915b7d32229a2518755", async() => {
                    WriteLiteral("Personal*");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#nullable restore
#line 31 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CodigoPersonal);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1dfe883582347446e2159beaf8915b7d32229a2520428", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
#nullable restore
#line 32 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CodigoPersonal);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
#nullable restore
#line 32 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items = ViewBag.CodigoPersonal;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-items", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Name = (string)__tagHelperAttribute_5.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1dfe883582347446e2159beaf8915b7d32229a2522765", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
#nullable restore
#line 33 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CodigoPersonal);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
						</div>
					</div>
				</div>
			</fieldset>

			<svg xmlns:dc=""http://purl.org/dc/elements/1.1/""
				 xmlns:cc=""http://creativecommons.org/ns#""
				 xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
				 xmlns:svg=""http://www.w3.org/2000/svg""
				 xmlns=""http://www.w3.org/2000/svg""
				 xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
				 xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
				 width=""810""
				 height=""400""
				 version=""1.1""
				 viewBox=""0 0 810 400""
				 id=""svg742""
				 name=""svg742""
				 sodipodi:docname=""odontv5.svg""
				 inkscape:version=""0.92.4 (5da689c313, 2019-01-14)"" >
				<defs id=""defs746"" />



				<sodipodi:namedview pagecolor=""#ffffff""
									bordercolor=""#666666""
									borderopacity=""1""
									objecttolerance=""10""
									gridtolerance=""10""
									guidetolerance=""10""
									inkscape:pageopacity=""0""
									inkscape:pageshadow=""2""
									inkscape:window-width=""1366""
									inkscape:wi");
                WriteLiteral(@"ndow-height=""705""
									id=""namedview744""
									showgrid=""false""
									inkscape:zoom=""1.218668""
									inkscape:cx=""351.15051""
									inkscape:cy=""428.05059""
									inkscape:window-x=""-8""
									inkscape:window-y=""-8""
									inkscape:window-maximized=""1""
									inkscape:current-layer=""svg742"" />



				<metadata id=""metadata2"">
					<rdf:RDF>
						<cc:Work");
                BeginWriteAttribute("rdf:about", " rdf:about=\"", 2811, "\"", 2823, 0);
                EndWriteAttribute();
                WriteLiteral(@">
							<dc:format>image/svg+xml</dc:format>
							<dc:type rdf:resource=""http://purl.org/dc/dcmitype/StillImage"" />



							<dc:title></dc:title>
						</cc:Work>
					</rdf:RDF>
				</metadata>
				<g id=""g-odontograma""
				   transform=""matrix(0.1554,0,0,0.16435,-113.44,-104.76)""
				   style=""stroke-width:3.39280009"">
					<path d=""M 729.98,1854.3 V 637.4 h 5212.4 V 3071.2 H 729.98 Z""
						  id=""path4""
						  inkscape:connector-curvature=""0"" />



					<path d=""M 3344.2,2458.4 V 1858.58 H 738 v 1199.6 h 2606.2 z""
						  id=""path6""
						  inkscape:connector-curvature=""0""
						  style=""fill:#ffffff"" />



					<path d=""m 866.03,2721.7 v -146.72 h 178.73 v 293.44 H 866.03 Z""
						  id=""path8""
						  inkscape:connector-curvature=""0"" />


										
					<g id=""g12"">
						<path inkscape:connector-curvature=""0""
							  id=""T-12""
							  d=""M 2645.3,991.24 V 844.52 h 178.72 v 293.44 H 2645.3 Z"" />



						<path d=""m 2771.8,1084.2 -21.1,-32.163 -22.748,-0.2019");
                WriteLiteral(@" -22.737,-0.2023 -19.875,32.365 -19.876,32.364 h 127.44 z""
							  id=""L-12""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



						<path d=""M 2810.7,998.88 V 885.69 l -24.001,38.658 -24.012,38.658 v 79.297 l 23.346,34.841 c 12.833,19.162 23.636,34.858 24.002,34.88 0.3661,0.023 0.6656,-50.894 0.6656,-113.15 z""
							  id=""D-12""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



						<path d=""m 2698.7,999.42 v -33.037 l -20.008,-42.166 -20.009,-42.166 v 214.7 l 40.017,-64.297 z""
							  id=""M-12""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



						<path d=""m 2749.3,999.87 v -30.207 h -37.344 v 60.414 h 37.344 z""
							  id=""O-12""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



						<path d=""m 2778.7,907.09 25.238,-40.995 h -67.461 c -53.24,0 -67.047,1.137 -65.495,5.3941 1.0931,2.9668 9.6593,21.415 19.056,40.995 l 17.078,35.601 h 46.346 z""
							  id=""B-12""
	");
                WriteLiteral(@"						  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



					</g>
					<g id=""g11"" onclick=""cargarPopUp(this.id);"" >
						<path inkscape:connector-curvature=""0""
							  id=""T-11"" 
							  d=""M 2968.1,991.24 V 844.52 h 178.74 v 293.44 H 2968.1 Z"" />



						<path  d=""m 3094.6,1084.2 -21.1,-32.163 -22.748,-0.2019 -22.738,-0.2023 -19.875,32.365 -19.876,32.364 h 127.44 z""
							  id=""L-11""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



						<path d=""M 3133.5,998.88 V 885.69 l -24.001,38.658 -24.013,38.658 v 79.297 l 23.336,34.841 c 12.844,19.162 23.646,34.858 24.013,34.88 0.3658,0.023 0.6652,-50.894 0.6652,-113.15 z""
							  id=""D-11""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



						<path d=""m 3021.4,999.42 v -33.037 l -40.017,-84.333 v 214.7 l 40.017,-64.297 z""
							  id=""M-11""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



						<path d=""m 3072.1,999.87");
                WriteLiteral(@" v -30.207 h -37.344 v 60.414 h 37.344 z""
							  id=""O-11""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" />



						<path d=""m 3101.5,907.09 25.238,-40.995 h -67.462 c -53.24,0 -67.047,1.137 -65.494,5.3941 1.0929,2.9668 9.6589,21.415 19.056,40.995 l 17.077,35.601 h 46.346 z""
							  id=""B-11""
							  inkscape:connector-curvature=""0""
							  style=""fill:#ffffff"" cambiocolor=""false"" />

					</g>
				</g>
			</svg>
			<br />
			<br />
			<div class=""row"">
				<div class=""col-lg-12"">
					<div class=""form-group"">
						<a class=""btn btn-white"" onclick=""regresar('../Odontogramas/Index','');"">Regresar</a>
						<button type=""button"" class=""btn btn-primary"" onclick=""GuardarDatosOdontograma();""><i class=""fa fa-save""></i> Guardar</button>
					</div>
				</div>
			</div>
		");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\t</div>\r\n</div>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            WriteLiteral(@"
<div id=""modalImportarDatos"" class=""modal fade"">
	<input hidden id=""idGrupo""  value=""hola""/>

	<div class=""modal-dialog modal-lg"">
		<div class=""modal-content"">
			<div class=""modal-header"">
				<h3 class=""m-t-none m-b"">Importar Datos</h3>
				<button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"" onclick=""limpiarDatos();"">
					<span aria-hidden=""true"">&times;</span>
				</button>
			</div>
			<div class=""modal-body"">
				<div class=""container"">
					<h2>Vertical Nav</h2>
					<p>Use the .flex-column class to create a vertical nav:</p>
					<ul class=""nav flex-column"">
						<li class=""nav-item"">
							<div class=""dropdown dropright"">
								<button class=""btn btn-primary dropdown-toggle"" data-toggle=""dropdown"" href=""#"">Caries</button>

								<div class=""dropdown-menu"">
									<a class=""dropdown-item"" id=""CariesB"" href=""#"" onclick=""cambiarColor(this.id);"">Bucal</a>
									<a class=""dropdown-item"" id=""CariesL"" href=""#"">Lingual</a>
									<a class=""dropdo");
            WriteLiteral(@"wn-item"" href=""#"">Distal</a>
									<a class=""dropdown-item"" href=""#"">Mesial</a>
									<a class=""dropdown-item"" href=""#"">Oclusal</a>
								</div>
							</div>
						</li>
						<li class=""nav-item"">
							<div class=""dropdown dropright"">
								<button class=""btn btn-primary dropdown-toggle"" data-toggle=""dropdown"" href=""#""> Recina</button>

								<div class=""dropdown-menu"">
									<a class=""dropdown-item"" href=""#"">Bucal</a>
									<a class=""dropdown-item"" href=""#"">Lingual</a>
									<a class=""dropdown-item"" href=""#"">Distal</a>
									<a class=""dropdown-item"" href=""#"">Mesial</a>
									<a class=""dropdown-item"" href=""#"">Oclusal</a>
								</div>
							</div>
						</li>
						<li class=""nav-item"">
							<div class=""dropdown dropright"">
								<button class=""btn btn-primary  dropdown-toggle"" data-toggle=""dropdown"" href=""#"">Amalgama</button>
								<div class=""dropdown-menu"">
									<a class=""dropdown-item"" href=""#"">Bucal</a>
									<a class=""dropdown-item"" h");
            WriteLiteral(@"ref=""#"">Lingual</a>
									<a class=""dropdown-item"" href=""#"">Distal</a>
									<a class=""dropdown-item"" href=""#"">Mesial</a>
									<a class=""dropdown-item"" href=""#"">Oclusal</a>
								</div>
							</div>
						</li>
						<li class=""nav-item"">
							<a class=""nav-link"" href=""#"">Sellante</a>
						</li>
						<li class=""nav-item"">
							<a class=""nav-link"" href=""#"">Extraccion</a>
						</li>
					</ul>
				</div>

			</div>
		</div>
	</div>

	
</div>



");
#nullable restore
#line 294 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
 if (ViewBag.Message != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("\t<script type=\"text/javascript\">\r\n        window.onload = function () {\r\n            if (\"");
#nullable restore
#line 298 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
            Write(ViewBag.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" === \"Save\") {\r\n                SuccessAlert(\"Guardados\",\"/../Odontogramas\");\r\n            }\r\n            else {\r\n                ErrorAlert(\"");
#nullable restore
#line 302 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
                       Write(ViewBag.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("\");\r\n            }\r\n        };\r\n\t</script>\r\n");
#nullable restore
#line 306 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
}

#line default
#line hidden
#nullable disable
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 308 "C:\Users\Laura\Documents\GitHub\HCOdontologicas-Tesis\HC_Odontologicas\Views\Odontogramas\Create.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HC_Odontologicas.Models.Odontograma> Html { get; private set; }
    }
}
#pragma warning restore 1591
