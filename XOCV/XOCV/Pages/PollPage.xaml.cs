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
using XOCV.Models;
using XOCV.Views;

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
		public ObservableCollection<ObservableCollection<ObservableCollection<CachedImage>>> lastImageTumbnails { get; set; }
		public ObservableCollection<ObservableCollection<ObservableCollection<Button>>> takePictureButtons { get; set; }
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
			imageCarouselGrids = new ObservableCollection<ObservableCollection<ObservableCollection<Grid>>>();
			lastImageTumbnails = new ObservableCollection<ObservableCollection<ObservableCollection<CachedImage>>>();
			takePictureButtons = new ObservableCollection<ObservableCollection<ObservableCollection<Button>>>();
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
			lastImageTumbnails.Clear();
			takePictureButtons.Clear();
			Carousel = null;
			foreach (var pageItemCollectiom in BuildModel.MultiComplexItems)
			{
				bool builded = true;

				imageCarouselExpanders.Add(new ObservableCollection<ObservableCollection<Button>>());
				imageCarouselGrids.Add(new ObservableCollection<ObservableCollection<Grid>>());
				lastImageTumbnails.Add(new ObservableCollection<ObservableCollection<CachedImage>>());
				takePictureButtons.Add(new ObservableCollection<ObservableCollection<Button>>());
				foreach (var pageItem in pageItemCollectiom.ComplexItems)
				{
					int imageSectionsCount = 0;
					int imageKeyCount = 0;
					if (pageItem.Items.Any(i => i.Name == "imageKey"))
					{
						imageCarouselExpanders.Last().Add(new ObservableCollection<Button>());
						imageCarouselGrids.Last().Add(new ObservableCollection<Grid>());
						lastImageTumbnails.Last().Add(new ObservableCollection<CachedImage>());
						takePictureButtons.Last().Add(new ObservableCollection<Button>());
					}

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

				        var evaluationsLabel = LabelHelper.SetLabelProperties(new Item {Properties = new PropertyModel {Text = "  1\t  2\t   3\t   4\t   5"}});
                        evaluationsLabel.Margin = new Thickness(20, 0, 0, 0);
                        complexGrid.Children.Add(evaluationsLabel, 1, 0);

                        string previousItem = string.Empty;
                        var rowIndex = 0;

				        foreach (var item in pageItem.Items)
				        {
                            switch (item.Name)
                            {
                                case "labelKey":
                                    {
                                        if (previousItem == "labelKey")
                                        {
                                            rowIndex++;
                                            complexGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                        }

                                        var label = LabelHelper.SetLabelProperties(item);
                                        
                                        complexGrid.Children.Add(label, 0, rowIndex);

                                        previousItem = item.Name;
                                        break;
                                    }
                                case "entryKey":
                                {
                                        var label = LabelHelper.SetLabelProperties(new Item
                                        {
                                            Properties = new PropertyModel
                                            {
                                                Text = $"\t{pageItem.Items.FirstOrDefault().Properties.Text} Comment"
                                            }
                                        });
                                        complexGrid.Children.Add(label, 0, rowIndex);

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
                                        complexGrid.Children.Add(imageBlock, 0, 2, rowIndex, rowIndex+1);

                                        imageKeyCount++;
                                        imageSectionsCount++;
                                        rowIndex++;
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
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			//grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			var buildPage = BindingContext as PollPageModel;
			var expandCarouselButton = new Button { TextColor = Color.Black, FontSize = 20, BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
			var lastImageTumbnail = new CachedImage { WidthRequest = 100, HeightRequest = 100 };
			var takePictureButton = new Button { Text = "Take a picture", WidthRequest = 150, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.FromHex("#b1e3ac"), TextColor = Color.White };
			takePictureButton.SetBinding(Button.CommandProperty, new Binding("TakePictureCommand"));
			takePictureButton.CommandParameter = new int[] { id, subId, carouselId };
			imageCarouselExpanders[id][subId].Add(expandCarouselButton);
			lastImageTumbnails[id][subId].Add(lastImageTumbnail);
			expandCarouselButton.Clicked += (sender, e) => 
			{
				var buildModel = BindingContext as PollPageModel;
				buildModel.SetCarouselItems(id, subId, carouselId);
			};
			expandCarouselButton.Text = (buildPage.PollPictures[id][subId].Pictures[carouselId] == 1) ? 
								string.Format("{0} photo", buildPage.PollPictures[id][subId].Pictures[carouselId]) : 
							string.Format("{0} photos", buildPage.PollPictures[id][subId].Pictures[carouselId]);
			lastImageTumbnail.IsVisible = !(buildPage.PollPictures[id][subId].Pictures[carouselId] == 0);
			if (lastImageTumbnail.IsVisible)
			{
				lastImageTumbnail.ImageUrl = buildPage.BuildModel.MultiComplexItems[id].ComplexItems[subId].ItemImageModels[carouselId].Last ();
			}
			grid.Children.Add(expandCarouselButton, 1, 0);
			grid.Children.Add(lastImageTumbnail, 2, 0);
			grid.Children.Add(takePictureButton, 0, 0);
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

                if (navButtons.Count == 0)
                {
                    BuildNavigationBar(pageModel);
                }

                this.SetBinding(CurrentPageNumberProperty, new Binding("CurrentCapturePosition", BindingMode.TwoWay));
				imageCarouselExpanders.Clear();

                BuildPage();
			}
		}
		#endregion

        #region Additionals methods
        private void BuildNavigationBar(PollPageModel pageModel)
        {
            foreach (var item in pageModel.FormNames)
            {
                var navButton = new CustomButton
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
								lastImageTumbnails[i][j][z].IsVisible = !(pageModel.PollPictures[i][j].Pictures[z] == 0);
								if (lastImageTumbnails[i][j][z].IsVisible)
								{
									lastImageTumbnails[i][j][z].ImageUrl = pageModel.BuildModel.MultiComplexItems[i].ComplexItems[j].ItemImageModels[z].Last();
								}
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
    }
}