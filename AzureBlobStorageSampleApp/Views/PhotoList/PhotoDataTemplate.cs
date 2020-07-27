using AzureBlobStorageSampleApp.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static AzureBlobStorageSampleApp.MarkupExtensions;
using static Xamarin.Forms.Markup.GridRowsColumns;

namespace AzureBlobStorageSampleApp
{
    public class PhotoDataTemplate : DataTemplate
    {
        public PhotoDataTemplate() : base(CreatePhotoDataTemplate)
        {
        }

        static Grid CreatePhotoDataTemplate() => new Grid
        {
            RowDefinitions = Rows.Define(AbsoluteGridLength(50)),

            ColumnDefinitions = Columns.Define(
                (Column.Image, AbsoluteGridLength(50)),
                (Column.Title, Star)),

            Children =
            {
                new Image { BackgroundColor = ColorConstants.PageBackgroundColor }.Center().Margin(0, 5)
                    .Column(Column.Image)
                    .Bind(Image.SourceProperty, nameof(PhotoModel.Url)),

                new Label().TextCenterVertical()
                    .Column(Column.Title)
                    .Bind(Label.TextProperty, nameof(PhotoModel.Title))
            }
        }.Padding(10, 0);

        enum Column { Image, Title }
    }
}
