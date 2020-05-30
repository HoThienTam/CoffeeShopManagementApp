﻿using Prism;
using Prism.Ioc;
using Mobile.ViewModels;
using Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Mobile
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("vi-VN");

            if (Application.Current.Properties.ContainsKey("token"))
            {
                await NavigationService.NavigateAsync("NavigationPage/MainPage");
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/LoginPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<CategoryPage, CategoryPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemPage, ItemPageViewModel>();
            containerRegistry.RegisterForNavigation<DiscountPage, DiscountPageViewModel>();
            containerRegistry.RegisterForNavigation<ZonePage, ZonePageViewModel>();
            containerRegistry.RegisterForNavigation<TablePage, TablePageViewModel>();
            containerRegistry.RegisterForNavigation<EmployeePage, EmployeePageViewModel>();
            containerRegistry.RegisterForNavigation<NewItemPage, NewItemPageViewModel>();
            containerRegistry.RegisterForNavigation<NewEmployeePage, NewEmployeePageViewModel>();
        }
    }
}
