using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using AzureBlobStorageSampleApp;
using AzureBlobStorageSameApp.iOS;

[assembly: ExportRenderer(typeof(PhotoViewCell), typeof(PhotosListImageCellCustomRenderer))]
namespace AzureBlobStorageSameApp.iOS
{
	public class PhotosListImageCellCustomRenderer : ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);

			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			return cell;
		}
	}
}
