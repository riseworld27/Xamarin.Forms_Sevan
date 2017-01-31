using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace XOCV.Views
{
	public class CarouselLayout : ScrollView
	{
		readonly StackLayout _stack;

		int _selectedIndex;

		public CarouselLayout()
		{
			Orientation = ScrollOrientation.Horizontal;

			_stack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = 0
			};

			Content = _stack;
		}


		public IList<View> Children
		{
			get
			{
				return _stack.Children;
			}
		}

		private bool _layingOutChildren;
		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			base.LayoutChildren(x, y, width, height);
			if (_layingOutChildren) return;

			_layingOutChildren = true;
			foreach (var child in Children) child.WidthRequest = width;
			_layingOutChildren = false;
		}

		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create(
				nameof(SelectedIndex),
				typeof(int),
				typeof(CarouselLayout),
				0,
				BindingMode.TwoWay,
				propertyChanged: async (bindable, oldValue, newValue) =>
				{
					await ((CarouselLayout)bindable).UpdateSelectedItem();
				}
			);

		public int SelectedIndex
		{
			get
			{
				return (int)GetValue(SelectedIndexProperty);
			}
			set
			{
				SetValue(SelectedIndexProperty, value);
			}
		}

		async Task UpdateSelectedItem()
		{
			await Task.Delay(300);
			SelectedItem = SelectedIndex > -1 ? Children[SelectedIndex].BindingContext : null;
		}

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(
				nameof(ItemsSource),
				typeof(IList),
				typeof(CarouselLayout),
				null,
				propertyChanging: (bindableObject, oldValue, newValue) =>
				{
					((CarouselLayout)bindableObject).ItemsSourceChanging();
				},
				propertyChanged: (bindableObject, oldValue, newValue) =>
				{
					((CarouselLayout)bindableObject).ItemsSourceChanged();
				}
			);

		public IList ItemsSource
		{
			get
			{
				return (IList)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		void ItemsSourceChanging()
		{
			if (ItemsSource == null) return;
			_selectedIndex = ItemsSource.IndexOf(SelectedItem);
		}

		void ItemsSourceChanged()
		{
			_stack.Children.Clear();
			try
			{
				foreach (var item in ItemsSource)
				{
					var view = (View)ItemTemplate.CreateContent();
					var bindableObject = view as BindableObject;
					if (bindableObject != null)
						bindableObject.BindingContext = item;
					_stack.Children.Add(view);
				}

				if (_selectedIndex >= 0) {
					SelectedIndex = _selectedIndex;
				}
				else if (ItemsSource != null && ItemsSource.Count > 0) {
					SelectedIndex = 0;
				}
				else {
					SelectedIndex = -1;
				}
			}
			catch
			{
				
			}
		}

		public DataTemplate ItemTemplate
		{
			get;
			set;
		}

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create(
				nameof(SelectedItem),
				typeof(object),
				typeof(CarouselLayout),
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{
					((CarouselLayout)bindable).UpdateSelectedIndex();
				}
			);

		public object SelectedItem
		{
			get
			{
				return GetValue(SelectedItemProperty);
			}
			set
			{
				SetValue(SelectedItemProperty, value);
			}
		}

		void UpdateSelectedIndex()
		{
			try
			{
				if (SelectedItem == BindingContext) return;

				SelectedIndex = Children
					.Select(c => c.BindingContext)
					.ToList()
					.IndexOf(SelectedItem);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public event EventHandler LongPressActivated;

		public void HandleLongPress(object sender, EventArgs e)
		{
			if (LongPressActivated != null)
			{
				LongPressActivated(this, new EventArgs());
			}
		}

		public event EventHandler SinglePressActivated;

		public void HandleSinglePress(object sender, EventArgs e)
		{
			if (LongPressActivated != null)
			{
				SinglePressActivated(this, new EventArgs());
			}
		}
	}
}
