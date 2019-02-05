using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public abstract class BaseContentPage<T> : ContentPage where T : BaseViewModel, new()
    {
        protected BaseContentPage()
        {
            BindingContext = ViewModel;
            BackgroundColor = ColorConstants.PageBackgroundColor;
            this.SetBinding(IsBusyProperty, nameof(ViewModel.IsInternetConnectionActive));
        }

        protected T ViewModel { get; } = new T();
    }
}
