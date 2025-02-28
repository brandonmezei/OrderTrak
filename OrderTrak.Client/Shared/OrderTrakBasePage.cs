using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Layout;
using OrderTrak.Client.Models;

namespace OrderTrak.Client.Shared
{
    public partial class OrderTrakBasePage : ComponentBase
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Inject]
        public ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        public IMapper MapperService { get; set; } = default!;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = default!;

        public bool IsLoading { get; set; }
    }
}
