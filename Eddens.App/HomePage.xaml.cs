using Eddens.App.Navigation;

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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Eddens.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : ItemsPageBase
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

            var menuItem = NavigationRootPage.Current.NavigationView.MenuItems.Cast<Microsoft.UI.Xaml.Controls.NavigationViewItem>().ElementAt(0);
            menuItem.IsSelected = true;
            NavigationRootPage.Current.NavigationView.Header = string.Empty;
        }

		protected override bool GetIsNarrowLayoutState()
		{
            return LayoutVisualStates.CurrentState == NarrowLayout;
        }
	}
}
