using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Eddens.App.Dialogs
{
	public sealed partial class SaveChangesDialog : ContentDialog
	{
		public SaveChangesDialog()
		{
			this.InitializeComponent();
		}

        /// <summary>
        /// Gets or sets the user's choice. 
        /// </summary>
        public SaveChangesDialogResult Result { get; private set; } = SaveChangesDialogResult.Cancel;

        /// <summary>
        /// Occurs when the user chooses to save. 
        /// </summary>
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = SaveChangesDialogResult.Save;
            Hide();
        }

        /// <summary>
        /// Occurs when the user chooses to discard changes.
        /// </summary>
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = SaveChangesDialogResult.DontSave;
            Hide();
        }

        /// <summary>
        /// Occurs when the user chooses to cancel the operation that triggered the event.
        /// </summary>
        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = SaveChangesDialogResult.Cancel;
            Hide();
        }
    }

    /// <summary>
    /// Defines the choices available to the user. 
    /// </summary>
    public enum SaveChangesDialogResult
    {
        Save,
        DontSave,
        Cancel
    }
}
