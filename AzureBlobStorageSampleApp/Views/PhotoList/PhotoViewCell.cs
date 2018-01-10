using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoViewCell : ViewCell
    {
        public PhotoViewCell()
        {
            var photo = new Image
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,5),
                Aspect = Aspect.AspectFit,
                BackgroundColor = ColorConstants.PageBackgroundColor
            };
            photo.SetBinding(Image.SourceProperty, nameof(PhotoModel.Url));

            var title = new Label { VerticalTextAlignment = TextAlignment.Center };
            title.SetBinding(Label.TextProperty, nameof(PhotoModel.Title));

            var grid = new Grid
            {
                Margin = new Thickness(10, 0),
                ColumnSpacing = 10,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) },},
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },}
            };
            grid.Children.Add(photo, 0, 0);
            grid.Children.Add(title, 1, 0);

            View = grid;
        }
    }
}
