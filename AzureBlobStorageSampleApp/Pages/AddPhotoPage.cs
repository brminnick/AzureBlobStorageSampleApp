using System;
using Xamarin.Forms;

using AzureBlobStorageSampleApp.Mobile.Shared;
using EntryCustomReturn.Forms.Plugin.Abstractions;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoPage : BaseContentPage<AddPhotoViewModel>
    {
        #region Constant Fields
        readonly ToolbarItem _saveToobarItem, _cancelToolbarItem;
        readonly CustomReturnEntry _photoTitleEntry;
        readonly Image _photoImage;
        readonly Button _takePhotoButton;
        #endregion

        #region Constructors
        public AddPhotoPage()
        {
            _photoTitleEntry = new CustomReturnEntry { Placeholder = "Title" };

            _saveToobarItem = new ToolbarItem
            {
                Text = "Save",
                Priority = 0,
                AutomationId = AutomationIdConstants.SaveButton,
            };
            _saveToobarItem.SetBinding(MenuItem.CommandProperty, nameof(ViewModel.SavePhotoCommand));

            _cancelToolbarItem = new ToolbarItem
            {
                Text = "Cancel",
                Priority = 1,
                AutomationId = AutomationIdConstants.CancelButton
            };

            _takePhotoButton = new Button { Text = "Take Photo" };
        }
        #endregion

        #region Methods
        protected override void SubscribeEventHandlers()
        {
            throw new NotImplementedException();
        }

        protected override void UnsubscribeEventHandlers()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
