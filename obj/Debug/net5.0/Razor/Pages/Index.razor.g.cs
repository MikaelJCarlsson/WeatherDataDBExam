#pragma checksum "C:\Users\Micke\source\repos\WeatherDataExam\Pages\Index.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "27811361ddffa32308132f8ae7182dff1d4ebf3a"
// <auto-generated/>
#pragma warning disable 1591
namespace WeatherDataExam.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using WeatherDataExam;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Micke\source\repos\WeatherDataExam\_Imports.razor"
using WeatherDataExam.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, @"<div class=""container""><div class=""jumbotron""><h1 class=""display-4"">Hello, User!</h1>
            <p class=""lead"">This is my simple weatherdata calculation app.</p>
            <hr class=""my-4"">
            <p>It calculates data from querys and displays different statistics.</p>
            <p class=""lead""></p></div></div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JS { get; set; }
    }
}
#pragma warning restore 1591
