using Eddens.App.DataModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Eddens.App
{
	/// <summary>
	/// A page that displays an overview of a single group, including a preview of the items
	/// within the group.
	/// </summary>
	public sealed partial class SectionPage : ItemsPageBase
    {
        public SectionPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var group = await ControlInfoDataSource.Instance.GetGroupAsync((string)e.Parameter);

            var menuItem = NavigationRootPage.Current.NavigationView.MenuItems.Cast<Microsoft.UI.Xaml.Controls.NavigationViewItemBase>().Single(i => (string)i.Tag == group.UniqueId);
            menuItem.IsSelected = true;
            NavigationRootPage.Current.NavigationView.Header = menuItem.Content;

            Items = group.Items.OrderBy(i => i.Title).ToList();
        }

        protected override bool GetIsNarrowLayoutState()
        {
            return LayoutVisualStates.CurrentState == NarrowLayout;
        }
    }
}
