using System.Linq;
using Xamarin.Forms;
using XOCV.Helpers.ControlHelpers;
using XOCV.Interfaces;
using XOCV.Models;
using XOCV.PageModels;

namespace XOCV.Pages.MasterDetailPage
{
    public partial class MenuPage : ContentPage
    {
        private new ContentModel Content { get; set; }

        public MenuPage ()
        {
            BindingContext = new MenuPageModel (FreshMvvm.FreshIOC.Container.Resolve<IWebApiHelper> ());
            var pageModel = BindingContext as MenuPageModel;

            InitializeComponent();

            if (pageModel != null)
            {
                Content = pageModel.Content;
				BuildMenu();
			}
            
            NavigationPage.SetHasNavigationBar (this, false);
        }

        private void BuildMenu ()
        {
            //menuPage.Children.Add(new Image() { Source = "Line2.png", HorizontalOptions = LayoutOptions.StartAndExpand, HeightRequest = 2, BackgroundColor = Color.White });
            //var menuButton = ButtonHelper.SetPolButtonProperties(Content.SetOfForms[0]);
            //menuPage.Children.Add(menuButton);
            //menuPage.Children.Add(new Image() { Source = "Line2.png", HorizontalOptions = LayoutOptions.StartAndExpand, HeightRequest = 2, BackgroundColor = Color.White });
			menuPage.Children.Add(new Image() { Source = "Line2.png", HorizontalOptions = LayoutOptions.StartAndExpand, HeightRequest = 2, BackgroundColor = Color.White });
            foreach (var company in Content.ListOfCompanies)
            { 
                var companyButton = new Button
                {
                    Text = company.Name,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    HeightRequest = 30,
                    TextColor = Color.FromHex("0B2D90"),
                    BackgroundColor = Color.White,
                    FontFamily = "Roboto",
                    CommandParameter = company,
                    FontSize = 20,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                menuPage.Children.Add(companyButton);
				menuPage.Children.Add(new Image() { Source = "Line2.png", HorizontalOptions = LayoutOptions.StartAndExpand, HeightRequest = 2, BackgroundColor = Color.White });
                foreach (var program in company.ListOfPrograms)
                {
                    //menuPage.Children.Add(new Image() { Source = "Line2.png", HorizontalOptions = LayoutOptions.StartAndExpand, HeightRequest = 2, BackgroundColor = Color.White });
                    var programButton = new Button
                    {
                        Text = program.Name,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        HeightRequest = 30,
                        TextColor = Color.FromHex("0B2D90"),
                        BackgroundColor = Color.White,
                        FontFamily = "Roboto",
                        CommandParameter = company,
                        FontSize = 18,
                        Margin = new Thickness(20, 0, 0, 0)
                    };
                    menuPage.Children.Add(programButton);
					menuPage.Children.Add(new Image() { Source = "Line2.png", HorizontalOptions = LayoutOptions.StartAndExpand, HeightRequest = 2, BackgroundColor = Color.White });
                    foreach (var form in program.SetOfForms)
                    {
                        //menuPage.Children.Add(new Image() { Source = "Line2.png", HorizontalOptions = LayoutOptions.StartAndExpand, HeightRequest = 2, BackgroundColor = Color.White });
                        var formButton = ButtonHelper.SetPolButtonProperties(form);
						formButton.FontSize = 16;
                        formButton.Margin = new Thickness(30, 0, 0, 0);
                        menuPage.Children.Add(formButton);
                        menuPage.Children.Add(new Image() { Source = "Line2.png", HorizontalOptions = LayoutOptions.StartAndExpand, HeightRequest = 2, BackgroundColor = Color.White });
                    }
                }
            }
        }
    }
}