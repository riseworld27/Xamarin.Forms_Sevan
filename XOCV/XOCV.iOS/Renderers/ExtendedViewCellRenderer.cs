using XOCV.Views;
using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using CoreGraphics;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(XOCV.Views.ExtendedViewCell), typeof(XOCV.iOS.Renderers.ExtendedViewCellRenderer))]
namespace XOCV.iOS.Renderers
{
    public class ExtendedViewCellRenderer : ViewCellRenderer
    {
        private readonly Dictionary<ExtendedViewCell, UITableViewCell> _cellMappings = new Dictionary<ExtendedViewCell, UITableViewCell>();
        private UITableView _tableView;

        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var extendedCell = (ExtendedViewCell)item;
            _tableView = _tableView ?? tv;

            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
            {
                cell.SelectionStyle = extendedCell.HighlightSelection ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;

                cell.BackgroundColor = extendedCell.BackgroundColor.ToUIColor();
                cell.SeparatorInset = new UIEdgeInsets((float)extendedCell.SeparatorPadding.Top, (float)extendedCell.SeparatorPadding.Left,
                    (float)extendedCell.SeparatorPadding.Bottom, (float)extendedCell.SeparatorPadding.Right);
                SetDiscolosure(extendedCell, cell);

                if (!_cellMappings.ContainsKey(extendedCell))
                    extendedCell.PropertyChanged += ExtendedCell_PropertyChanged;

                _cellMappings[extendedCell] = cell;
            }
            if (!extendedCell.ShowSeparator)
                tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tv.SeparatorColor = extendedCell.SeparatorColor.ToUIColor();
            return cell;
        }

        private void ExtendedCell_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var extendedCell = (ExtendedViewCell)sender;
            if (e.PropertyName != ExtendedViewCell.ShowDisclosureProperty.PropertyName)
                return;

            UITableViewCell cell;
            if (!_cellMappings.TryGetValue((ExtendedViewCell)sender, out cell)) return;
            try
            {
                SetDiscolosure(extendedCell, cell);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update cell: " + ex.Message);
            }
        }

        private void SetDiscolosure(ExtendedViewCell extendedCell, UITableViewCell cell)
        {
            if (extendedCell.ShowDisclosure)
            {
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                if (!string.IsNullOrEmpty(extendedCell.DisclosureImage))
                {
                    var detailDisclosureButton = UIButton.FromType(UIButtonType.Custom);
                    detailDisclosureButton.SetImage(UIImage.FromBundle(extendedCell.DisclosureImage), UIControlState.Normal);
                    detailDisclosureButton.SetImage(UIImage.FromBundle(extendedCell.DisclosureImage), UIControlState.Selected);
                    detailDisclosureButton.Frame = new CGRect(0f, 0f, 30f, 30f);
                    detailDisclosureButton.TouchUpInside += (sender, e) =>
                    {
                        try
                        {
                            var index = _tableView.IndexPathForCell(cell);
                            _tableView.SelectRow(index, true, UITableViewScrollPosition.None);
                            _tableView.Source.RowSelected(_tableView, index);
                        }
                        catch
                        {
                            Console.Write("Xamarin Forms Labs Weird stuff : You_Should_Not_Call_base_In_This_Method happend");
                        }
                    };
                    cell.AccessoryView = detailDisclosureButton;
                }
            }
            else
            {
                cell.Accessory = UITableViewCellAccessory.None;
                cell.AccessoryView = null;
            }
        }
    }
}