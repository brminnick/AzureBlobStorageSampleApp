using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using AzureBlobStorageSameApp;
using AzureBlobStorageSameApp.iOS;

[assembly: ExportRenderer(typeof(ImageCell), typeof(PhotosListImageCellCustomRenderer))]
namespace AzureBlobStorageSameApp.iOS
{
	public class PhotosListImageCellCustomRenderer : ImageCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);

			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			return cell;
		}
	}
}
