// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

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
#nullable restore
#line 2 "C:\Users\Micke\source\repos\WeatherDataExam\Pages\Seasons.razor"
using WeatherDataExam.Model.Entities;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/seasons")]
    public partial class Seasons : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 79 "C:\Users\Micke\source\repos\WeatherDataExam\Pages\Seasons.razor"
       
    private IList<WeatherData> meteorologicalAutumn = new List<WeatherData>();
    private IList<WeatherData> meteorologicalWinter = new List<WeatherData>();
    
    protected override async Task OnInitializedAsync()
    {
        bool autumn = true;
        bool winter = false;
        meteorologicalAutumn = await WData.MeteorologicalSeason(autumn);
        meteorologicalWinter = await WData.MeteorologicalSeason(winter);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private WeatherData WData { get; set; }
    }
}
#pragma warning restore 1591
