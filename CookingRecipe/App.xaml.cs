﻿using CookingRecipe.Common;
using CookingRecipe.DataModel;
using CookingRecipe.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CookingRecipe
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += App_Resuming;
            this.RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                this.FocusVisualKind = AnalyticsInfo.VersionInfo.DeviceFamily == "Xbox" ? FocusVisualKind.Reveal : FocusVisualKind.HighVisibility;
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
			//Frame rootFrame = Window.Current.Content as Frame;

			//// Do not repeat app initialization when the Window already has content,
			//// just ensure that the window is active
			//if (rootFrame == null)
			//{
			//	// Create a Frame to act as the navigation context and navigate to the first page
			//	rootFrame = new Frame();

			//	rootFrame.NavigationFailed += OnNavigationFailed;

			//	if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
			//	{
			//		//TODO: Load state from previously suspended application
			//	}

			//	// Place the frame in the current Window
			//	Window.Current.Content = rootFrame;
			//}

			CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            await EnsureWindow(e);

			//if (e.PrelaunchActivated == false)
			//{
			//	if (rootFrame.Content == null)
			//	{
			//		// When the navigation stack isn't restored navigate to the first page,
			//		// configuring the new page by passing required information as a navigation
			//		// parameter
			//		rootFrame.Navigate(typeof(NavigationRootPage), e.Arguments);
			//	}
			//	// Ensure the current window is active
			//	Window.Current.Activate();
			//}
		}

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private async void App_Resuming(object sender, object e)
        {
            // We are being resumed, so lets restore our state!
            try
            {
                await SuspensionManager.RestoreAsync();
            }
            finally
            {
                switch (NavigationRootPage.RootFrame?.Content)
                {
                    //case ItemPage itemPage:
                    //    itemPage.SetInitialVisuals();
                    //    break;
                    //case NewControlsPage _:
                    //case AllControlsPage _:
                    //    NavigationRootPage.Current.NavigationView.AlwaysShowHeader = false;
                    //    break;
                }
            }

        }

        private async Task EnsureWindow(IActivatedEventArgs args)
        {
			// No matter what our destination is, we're going to need control data loaded - let's knock that out now.
			// We'll never need to do this again.
			await ControlInfoDataSource.Instance.GetGroupsAsync();

			Frame rootFrame = GetRootFrame();

            ThemeHelper.Initialize();

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated
                    || args.PreviousExecutionState == ApplicationExecutionState.Suspended)
            {
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException)
                {
                    //Something went wrong restoring state.
                    //Assume there is no state and continue
                }

                Window.Current.Activate();

                UpdateNavigationBasedOnSelectedPage(rootFrame);
                return;
            }

            //Type targetPageType = typeof(NewControlsPage);
            //string targetPageArguments = string.Empty;

            //if (args.Kind == ActivationKind.Launch)
            //{
            //    targetPageArguments = ((LaunchActivatedEventArgs)args).Arguments;
            //}
            //else if (args.Kind == ActivationKind.Protocol)
            //{
            //    Match match;

            //    string targetId = string.Empty;

            //    switch (((ProtocolActivatedEventArgs)args).Uri?.AbsoluteUri)
            //    {
            //        case string s when IsMatching(s, "(/*)category/(.*)"):
            //            targetId = match.Groups[2]?.ToString();
            //            if (targetId == "AllControls")
            //            {
            //                targetPageType = typeof(AllControlsPage);
            //            }
            //            else if (targetId == "NewControls")
            //            {
            //                targetPageType = typeof(NewControlsPage);
            //            }
            //            else if (ControlInfoDataSource.Instance.Groups.Any(g => g.UniqueId == targetId))
            //            {
            //                targetPageType = typeof(SectionPage);
            //            }
            //            break;

            //        case string s when IsMatching(s, "(/*)item/(.*)"):
            //            targetId = match.Groups[2]?.ToString();
            //            if (ControlInfoDataSource.Instance.Groups.Any(g => g.Items.Any(i => i.UniqueId == targetId)))
            //            {
            //                targetPageType = typeof(ItemPage);
            //            }
            //            break;
            //    }

            //    targetPageArguments = targetId;

            //    bool IsMatching(string parent, string expression)
            //    {
            //        match = Regex.Match(parent, expression);
            //        return match.Success;
            //    }
            //}

            //rootFrame.Navigate(targetPageType, targetPageArguments);

            //if (targetPageType == typeof(NewControlsPage))
            //{
            //    ((Microsoft.UI.Xaml.Controls.NavigationViewItem)((NavigationRootPage)Window.Current.Content).NavigationView.MenuItems[0]).IsSelected = true;
            //}
            //else if (targetPageType == typeof(ItemPage))
            //{
            //    NavigationRootPage.Current.EnsureNavigationSelection(targetPageArguments);
            //}

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private Frame GetRootFrame()
        {
            Frame rootFrame;
            if (!(Window.Current.Content is NavigationRootPage rootPage))
            {
                rootPage = new NavigationRootPage();
                rootFrame = (Frame)rootPage.FindName("rootFrame");
                if (rootFrame == null)
                {
                    throw new Exception("Root frame not found");
                }
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootPage;
            }
            else
            {
                rootFrame = (Frame)rootPage.FindName("rootFrame");
            }

            return rootFrame;
        }

        private static void UpdateNavigationBasedOnSelectedPage(Frame rootFrame)
        {
            // Check if we brought back an ItemPage
            //UNDONE: Uncomment
            //if (rootFrame.Content is ItemPage itemPage)
            //{
            //    // We did, so bring the selected item back into view
            //    string name = itemPage.Item.Title;
            //    if (Window.Current.Content is NavigationRootPage nav)
            //    {
            //        // Finally brings back into view the correct item.
            //        // But first: Update page layout!
            //        nav.EnsureItemIsVisibleInNavigation(name);
            //    }
            //}
        }

        public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }
    }
}
