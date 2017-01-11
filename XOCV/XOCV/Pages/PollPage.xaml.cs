using System.Collections.ObjectModel;
using Xamarin.Forms;
using System;
using System.Diagnostics;
using XOCV.Helpers.ControlHelpers;
using XOCV.Models.ResponseModels;
using XOCV.PageModels;
using XOCV.Pages.Base;
using XOCV.Views.RadioButtons;
using System.Linq;
using Acr.UserDialogs;
using XOCV.Enums;
using XOCV.Models;
using XOCV.Views;
using System.Threading.Tasks;

namespace XOCV.Pages
{
	public partial class PollPage : XOCVPage
	{
		#region Fields
		private int currentComplexItem;
		#endregion

		#region Properties
		public static FormModel BuildModel { get; set; }
		public ObservableCollection<ObservableCollection<ObservableCollection<Button>>> imageCarouselExpanders { get; set; }
		public ObservableCollection<ObservableCollection<ObservableCollection<Grid>>> imageCarouselGrids { get; set; }
		public ObservableCollection<Button> navButtons { get; set; }
		public Grid CurrentCarouselPosition { get; set; }
		public StackLayout Carousel { get; set; }
		public CarouselLayout CarouselLayout { get; set; }
		public bool isInited { get; set; }

		public int CurrentPageNumber
		{
			get { return (int)GetValue(CurrentPageNumberProperty); }
			set { SetValue(CurrentPageNumberProperty, value); }
		}
		public ObservableCollection<ObservableCollection<ComplexItemPhotosModel>> PollPictures
		{
			get { return (ObservableCollection<ObservableCollection<ComplexItemPhotosModel>>)GetValue(PollPicturesProperty); }
			set { SetValue(PollPicturesProperty, value); }
		}
		#endregion

		#region Bindable properties
		public static readonly BindableProperty CurrentPageNumberProperty =
			BindableProperty.Create(
				nameof(CurrentPageNumber),
				typeof(object),
				typeof(PollPage),
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{
					((PollPage)bindable).UpdateNavButtons();
				}
			);

		public static readonly BindableProperty PollPicturesProperty =
			BindableProperty.Create(
				nameof(PollPictures),
				typeof(object),
				typeof(PollPage),
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{
					((PollPage)bindable).UpdatePollPhotoSections();
				}
			);
		#endregion

		#region Constructor
		public PollPage()
		{
			InitializeComponent();

			imageCarouselExpanders = new ObservableCollection<ObservableCollection<ObservableCollection<Button>>>();
			imageCarouselGrids= new ObservableCollection<ObservableCollection<ObservableCollection<Grid>>>();
			navButtons = new ObservableCollection<Button>();
			MessagingCenter.Subscribe<PollPageModel>(this, "onImageInteracted", (page) =>
														{
															UpdatePollPhotoSections();
														}
													  );

			NavigationPage.SetHasNavigationBar(this, false);
		}
		#endregion

		#region Building page
		private void BuildPage()
		{
			var pageModel = BindingContext as PollPageModel;
			isInited = false;
			imageCarouselExpanders.Clear();
			imageCarouselGrids.Clear();
			Carousel = null;
			foreach (var pageItemCollectiom in BuildModel.MultiComplexItems)
			{
				bool builded = true;

				imageCarouselExpanders.Add(new ObservableCollection<ObservableCollection<Button>>());
				imageCarouselGrids.Add(new ObservableCollection<ObservableCollection<Grid>>());
				foreach (var pageItem in pageItemCollectiom.ComplexItems)
				{
					int imageSectionsCount = 0;
					if (pageItem.Items.Any(i => i.Name == "imageKey"))
					{
						imageCarouselExpanders.Last().Add(new ObservableCollection<Button>());
						imageCarouselGrids.Last().Add(new ObservableCollection<Grid>());
					}

                    // Test for GRID design
				    //pageItem.IsGridStyle = true;

				    if (pageItem.IsGridStyle)
				    {
                        #region Building GRID STYLE complex item

                        var complexGrid = new Grid
                        {
                            ColumnDefinitions =
                            {
                                new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star)},
                                new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star)},
                                new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star)}
                            },
                            RowDefinitions =
                            {
                                new RowDefinition { Height = GridLength.Auto }
                            }
                        };

				        var evaluationsLabel = LabelHelper.SetLabelProperties(new Item {Properties = new PropertyModel {Text = "  1\t  2\t  3\t  4\t  5"}});
                        evaluationsLabel.Margin = new Thickness(20, 0, 0, 0);
                        complexGrid.Children.Add(evaluationsLabel, 1, 0);

                        string previousItem = string.Empty;
				        var rowIndex = 0;

				        foreach (var item in pageItem.Items)
				        {
                            int imageKeyCount = 0;


                            switch (item.Name)
                            {
                                case "labelKey":
                                    {
                                        var label = LabelHelper.SetLabelProperties(item);
                                        
                                        complexGrid.Children.Add(label, 0, rowIndex);

                                        if (previousItem == "labelKey")
                                        {
                                            rowIndex++;
                                            complexGrid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                                        }

                                        previousItem = item.Name;
                                        break;
                                    }
                                case "entryKey":
                                {
                                        var label = LabelHelper.SetLabelProperties(new Item
                                        {
                                            Properties = new PropertyModel
                                            {
                                                Text = $"{pageItem.Items.FirstOrDefault().Properties.Text} Comment"
                                            }
                                        });
                                        complexGrid.Children.Add(label, 1, rowIndex);

                                        var editor = EntryHelper.SetEntryProperties(item); 
                                        editor.SetBinding(Editor.TextProperty, new Binding("Value", BindingMode.TwoWay, null, null, null, item));
                                        complexGrid.Children.Add(editor, 1, rowIndex);

                                        rowIndex++;
                                        previousItem = item.Name;
                                        complexGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                        break;
                                    }
                                case "radioGroupKey":
                                    {
                                        var radioGroup = BindableRadioGroupHelper.SetBindableHorizontalRadioGroupProperties(item);
                                        radioGroup.SetBinding(BindableHorizontalRadioGroup.SelectedIndexProperty, new Binding("Value", BindingMode.TwoWay, null, null, null, item));
                                        complexGrid.Children.Add(radioGroup, 1, rowIndex);
                                        rowIndex++;
                                        previousItem = item.Name;
                                        complexGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                        break;
                                    }
                                case "hintKey":
                                    {
                                        var hint = HintHelper.SetHintProperties(item);
                                        complexGrid.Children.Add(hint, 0, rowIndex);
                                        rowIndex++;
                                        previousItem = item.Name;
                                        complexGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                        break;
                                    }
                                case "imageKey":
                                    {
                                      

                                        var subId = pageItemCollectiom.ComplexItems.IndexOf(pageItem);

                                        var imageBlock = BuildImageBlock(currentComplexItem, subId, imageKeyCount);

                                        imageCarouselGrids[currentComplexItem][subId].Add(imageBlock);
										complexGrid.Children.Add(imageBlock, 0, 3, rowIndex, 1 + rowIndex);

                                        imageKeyCount++;
                                        imageSectionsCount++;
                                        previousItem = item.Name;
                                        complexGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                        break;
                                    }
                            }
                        }
                        currentPage.Children.Add(complexGrid);
                        #endregion
                    }
                    else
				    {
                        #region Building DEFAULT STYLE complex item
                        foreach (var item in pageItem.Items)
                        {
                            int imageKeyCount = 0;
                            switch (item.Name)
                            {
                                case "labelKey":
                                    {
                                        var label = LabelHelper.SetLabelProperties(item);
                                        currentPage.Children.Add(label);
                                        break;
                                    }
                                case "entryKey":
                                    {
                                        var editor = EntryHelper.SetEntryProperties(item);
                                        editor.SetBinding(Editor.TextProperty, new Binding("Value", BindingMode.TwoWay, null, null, null, item));
                                        currentPage.Children.Add(editor);
                                        break;
                                    }
                                case "radioGroupKey":
                                    {
                                        if (item.IsMultipleSelection)
                                        {
                                            var radioGroup = BindableRadioGroupHelper.SetMultipleSelectionRadioGroupProperties(item);
                                            radioGroup.SetBinding(BindableMultipleSelectionRagioGroup.MultiSelectedIndexesProperty, new Binding("Values", BindingMode.TwoWay, null, null, null, item));
                                            currentPage.Children.Add(radioGroup);
                                        }
                                        else
                                        {
                                            var radioGroup = BindableRadioGroupHelper.SetRadioGroupProperties(item);
                                            radioGroup.SetBinding(BindableRadioGroup.SelectedIndexProperty, new Binding("Value", BindingMode.TwoWay, null, null, null, item));
                                            currentPage.Children.Add(radioGroup);
                                        }
                                        break;
                                    }
                                case "hintKey":
                                    {
                                        var hint = HintHelper.SetHintProperties(item);
                                        currentPage.Children.Add(hint);
                                        break;
                                    }
                                case "imageKey":
                                    {

                                        var subId = pageItemCollectiom.ComplexItems.IndexOf(pageItem);

                                        var imageBlock = BuildImageBlock(currentComplexItem, subId, imageKeyCount);

                                        imageCarouselGrids[currentComplexItem][subId].Add(imageBlock);
                                        currentPage.Children.Add(imageBlock);

                                        imageKeyCount++;
                                        imageSectionsCount++;
                                        break;
                                    }
                            }
                        }
                        #endregion
                    }

                    #region Repeating section
                    if (pageItem.IsRepeatableSection || pageItem.IsRepeatedSection)
					{
						var stack = new Grid();
						stack.HorizontalOptions = LayoutOptions.EndAndExpand;
						var repeateButton = new Button
						{
							BackgroundColor = Color.Transparent,
							Text = "+",
							TextColor = Color.Green,
							FontSize = 45,
							FontAttributes = FontAttributes.Bold,
							VerticalOptions = LayoutOptions.Start,
							HorizontalOptions = LayoutOptions.CenterAndExpand,
						};

						repeateButton.Clicked += (sender, e) =>
						{
							pageModel.UserDialog.ShowLoading("Adding section...");
							int index = BuildModel.MultiComplexItems.IndexOf(pageItemCollectiom);
							int photoSectionsCount = pageModel.PollPictures[index].Last().Pictures.Count();
							pageModel.PollPictures[index].Add(new ComplexItemPhotosModel());
							for (int i = 0; i < photoSectionsCount; i++)
							{
								pageModel.PollPictures[index].Last().Pictures.Add(0);
							}
							var repeatedSection = pageItem.GetRepeatedSection();
							pageItemCollectiom.ComplexItems.Add(repeatedSection);
							currentPage.Children.Clear();
							currentComplexItem = 0;
							imageSectionsCount = 0;
							OnBindingContextChanged();
							pageModel.UserDialog.HideLoading();
						};
						stack.Children.Add(repeateButton, 0, 0);

						if (pageItem.IsRepeatedSection)
						{
							var unrepeateButton = new Button
							{
								BackgroundColor = Color.Transparent,
								Text = "-",
								TextColor = Color.Red,
								FontSize = 45,
								FontAttributes = FontAttributes.Bold,
								VerticalOptions = LayoutOptions.Start,
								HorizontalOptions = LayoutOptions.CenterAndExpand,
							};

							unrepeateButton.Clicked += (sender, e) =>
							{
								if (pageItem != pageItemCollectiom.ComplexItems.Last()) return;
								pageModel.UserDialog.ShowLoading("Removing section...");
								int index = BuildModel.MultiComplexItems.IndexOf(pageItemCollectiom);
								pageModel.PollPictures[index].Remove(pageModel.PollPictures[index].Last());
								pageItemCollectiom.ComplexItems.Remove(pageItem);
								currentPage.Children.Clear();
								currentComplexItem = 0;
								imageSectionsCount = 0;
								OnBindingContextChanged();
								pageModel.UserDialog.HideLoading();
							};
							stack.Children.Add(unrepeateButton, 1, 0);
						}
						currentPage.Children.Add(stack);
					}
                    #endregion

                    var line = new BoxView
					{
						HeightRequest = 2,
						BackgroundColor = Color.FromHex("#63bd5b")
					};
					currentPage.Children.Add(line);
					this.SetBinding(PollPicturesProperty, "PollPictures");
					UpdatePollPhotoSections();

					isInited = true;
				}

				if (builded)
				{
					currentComplexItem++;
				}
				GC.Collect();
			}
		}

        Grid BuildImageBlock(int id, int subId, int carouselId)
		{
			var grid = new Grid { HorizontalOptions = LayoutOptions.FillAndExpand };
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			var buildPage = BindingContext as PollPageModel;
			var expandCarouselButton = new Button { TextColor = Color.Black, FontSize = 20, BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
			imageCarouselExpanders[id][subId].Add(expandCarouselButton);
			expandCarouselButton.Clicked += (sender, e) => 
			{
				var buildModel = BindingContext as PollPageModel;
				buildModel.SetCarouselItems(id, subId, carouselId);
			};
			expandCarouselButton.Text = (buildPage.PollPictures[id][subId].Pictures[carouselId] == 1) ? 
								string.Format("{0} photo", buildPage.PollPictures[id][subId].Pictures[carouselId]) : 
							string.Format("{0} photos", buildPage.PollPictures[id][subId].Pictures[carouselId]);
			grid.Children.Add(expandCarouselButton, 1, 0);
			return grid;
		}

		private void NextButtonOnClicked(object sender, EventArgs eventArgs)
		{
			currentPage.Children.Clear();
			currentComplexItem = 0;
			OnBindingContextChanged();
			currentScrollView.ScrollToAsync(currentFrame, ScrollToPosition.Start, false);
		}
		#endregion

		#region OnBindingContextChanged
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			GC.Collect();

			var pageModel = BindingContext as PollPageModel;
			if (pageModel != null)
			{
				BuildModel = pageModel.BuildModel;

				BuildNavigationBar(pageModel);

				this.SetBinding(CurrentPageNumberProperty, new Binding("CurrentCapturePosition", BindingMode.TwoWay));
				imageCarouselExpanders.Clear();

			    //BuildMockUpTableForKenn();  // TEST
                BuildPage();
			}
		}
		#endregion

		#region Additionals methods
		private void BuildNavigationBar(PollPageModel pageModel)
		{
			navButtons.Clear();
			navigationPage.Children.Clear();
			foreach (var item in pageModel.FormNames)
			{
				var navButton = new Button
				{
					WidthRequest = 150,
					HeightRequest = 40,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Text = item.FormName,
					TextColor = Color.White,
					BackgroundColor = Color.Black,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					BorderRadius = 5,
					BorderColor = Color.Gray,
					BorderWidth = 2
				};
				navButtons.Add(navButton);
				navButton.CommandParameter = navButtons.IndexOf(navButton);
				navButton.SetBinding(Button.CommandProperty, "GoToCaptureCommand");

				navButton.Clicked += NextButtonOnClicked;
				navigationPage.Children.Add(navButton);
			}
		}

		private void BuildNavigationBarForNAVOnly(PollPageModel pageModel)
		{
			navButtons.Clear();
			navigationPage.Children.Clear();
			foreach (var item in pageModel.FormNames.Where(x => x.IsNAVOnly))
			{
				var navButton = new Button
				{
					WidthRequest = 150,
					HeightRequest = 40,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					Text = item.FormName,
					TextColor = Color.White,
					BackgroundColor = Color.Black,
					BorderRadius = 5,
					BorderColor = Color.Gray,
					BorderWidth = 2
				};
				navButtons.Add(navButton);
				navButton.CommandParameter = navButtons.IndexOf(navButton);
				navButton.SetBinding(Button.CommandProperty, "GoToCaptureCommand");

				navButton.Clicked += NextButtonOnClicked;
				navigationPage.Children.Add(navButton);
			}
		}

		private void UpdateNavButtons()
		{
			foreach (var button in navButtons)
			{
				button.BackgroundColor = Color.Black;
			}
			navButtons[CurrentPageNumber].BackgroundColor = Color.FromHex("#63bd5b");
			GC.Collect();
		}
		#endregion

		#region Updation photo section
		public void UpdatePollPhotoSections()
		{
			try
			{
				var pageModel = BindingContext as PollPageModel;

				for (var i = 0; i < PollPictures.Count; i++)
				{
					for (var j = 0; j < PollPictures[i].Count; j++)
					{
						for (var z = 0; z < PollPictures[i][j].Pictures.Count; z++)
						{
							string sCount = imageCarouselExpanders[i][j][z].Text.Trim()[0].ToString();
							int count = int.Parse(sCount);
							if (count != PollPictures[i][j].Pictures[z])
							{
								imageCarouselExpanders[i][j][z].Text = (PollPictures[i][j].Pictures[z] == 1) ?
																	string.Format("{0} photo", PollPictures[i][j].Pictures[z]) :
																	string.Format("{0} photos", PollPictures[i][j].Pictures[z]);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				var exMes = ex.Message;
				Debug.WriteLine(exMes);
			}
			GC.Collect();
		}
        #endregion

        #region Test building
        //ToDo: remove it!
        private void BuildMockUpTableForKenn()
	    {
	        var table = new Grid
	        {
	            RowDefinitions =
	            {
	                new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto}
                }
            };

	        var labelTitle = LabelHelper.SetLabelProperties(new Item {Properties = new PropertyModel {Text = "Title"} });
	        var label1 = LabelHelper.SetLabelProperties(new Item { Properties = new PropertyModel { Text = "Question 1" } });
            var label2 = LabelHelper.SetLabelProperties(new Item { Properties = new PropertyModel { Text = "Question 2" } });
            var label3 = LabelHelper.SetLabelProperties(new Item { Properties = new PropertyModel { Text = "Question 3" } });
            var label4 = LabelHelper.SetLabelProperties(new Item { Properties = new PropertyModel { Text = "Question 4" } });

	        var labelCases = LabelHelper.SetLabelProperties(new Item {Properties = new PropertyModel {Text = "  1    2    3   4   5"}});
	        labelCases.Margin = new Thickness(20, 0, 0, 0);
            var radioGroupButtons1 = BindableRadioGroupHelper.SetRadioGroupProperties(new Item { RadioButtonItemsSource = { "", "", "", "", "", "" } });
            var radioGroupButtons2 = BindableRadioGroupHelper.SetRadioGroupProperties(new Item { RadioButtonItemsSource = { "", "", "", "", "", "" } });
            var radioGroupButtons3 = BindableRadioGroupHelper.SetRadioGroupProperties(new Item { RadioButtonItemsSource = { "", "", "", "", "", "" } });
            var radioGroupButtons4 = BindableRadioGroupHelper.SetRadioGroupProperties(new Item { RadioButtonItemsSource = { "", "", "", "", "", "" } });

            var labelOptions = LabelHelper.SetLabelProperties(new Item { Properties = new PropertyModel { Text = "Options"} });
            var buttonTakePicture1 = new Button { Text = "Take a picture", VerticalOptions = LayoutOptions.Center };
            var buttonTakePicture2 = new Button { Text = "Take a picture", VerticalOptions = LayoutOptions.Center };
            var buttonTakePicture3 = new Button { Text = "Take a picture", VerticalOptions = LayoutOptions.Center };
            var buttonTakePicture4 = new Button { Text = "Take a picture", VerticalOptions = LayoutOptions.Center };

            table.Children.Add(labelTitle, 0, 0);
            table.Children.Add(label1, 0,1);
            table.Children.Add(label2, 0,2);
            table.Children.Add(label3, 0,3);
            table.Children.Add(label4, 0,4);
            table.Children.Add(labelCases, 1, 0);
            table.Children.Add(radioGroupButtons1, 1, 1);
            table.Children.Add(radioGroupButtons2, 1, 2);
            table.Children.Add(radioGroupButtons3, 1, 3);
            table.Children.Add(radioGroupButtons4, 1, 4);
            table.Children.Add(labelOptions, 2, 0);
            table.Children.Add(buttonTakePicture1, 2, 1);
            table.Children.Add(buttonTakePicture2, 2, 2);
            table.Children.Add(buttonTakePicture3, 2, 3);
            table.Children.Add(buttonTakePicture4, 2, 4);

            currentPage.Children.Add(table);
        }
        #endregion
    }
}