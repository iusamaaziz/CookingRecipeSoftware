﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using muxc = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CookingRecipe.UserControls
{
    public sealed partial class CollapsibleSearchBox : UserControl
    {
        public CollapsibleSearchBox()
        {
            InitializeComponent();
			this.Loaded += CollapsableSearchBox_Loaded;
			this.Unloaded += CollapsibleSearchBox_Unloaded;
			Window.Current.SizeChanged += Current_SizeChanged;
            myAutoSuggestBox = SearchBox;
        }

        public double CollapseWidth
        {
            get { return (double)GetValue(CollapseWidthProperty); }
            set { SetValue(CollapseWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CollapseWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollapseWidthProperty =
            DependencyProperty.Register("CollapseWidth", typeof(double), typeof(CollapsibleSearchBox), new PropertyMetadata(0.0));

        private AutoSuggestBox myAutoSuggestBox;
        public AutoSuggestBox AutoSuggestBox
        {
            get { return myAutoSuggestBox; }
            private set { myAutoSuggestBox = value; }
        }

        private void CollapsableSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            SearchButton.AddHandler(PointerPressedEvent,
                new PointerEventHandler(ToggleButton_PointerPressed), true);
            SearchButton.AddHandler(UIElement.PointerReleasedEvent,
                new PointerEventHandler(ToggleButton_PointerReleased), true);

            SetState(Window.Current.Bounds.Width);
        }

        private void CollapsibleSearchBox_Unloaded(object sender, RoutedEventArgs e)
        {
            SearchButton.RemoveHandler(UIElement.PointerPressedEvent,
                (PointerEventHandler)ToggleButton_PointerPressed);
            SearchButton.RemoveHandler(UIElement.PointerReleasedEvent,
                (PointerEventHandler)ToggleButton_PointerReleased);
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            SetState(e.Size.Width);
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetState(Window.Current.Bounds.Width);
            SearchButton.IsChecked = false;
        }

        private void SetState(double width)
        {
            if (width <= CollapseWidth)
            {
                VisualStateManager.GoToState(this, "CollapsedState", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "OpenState", true);
            }
        }

        private void SearchButton_Checked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OpenState", true);
            if (SearchBox != null)
            {
                SearchBox.Focus(FocusState.Programmatic);
            }
        }

        // Set states for animated icon in toggle button.
        private void ToggleButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            muxc.AnimatedIcon.SetState((UIElement)sender, "PointerOver");
        }

        private void ToggleButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            muxc.AnimatedIcon.SetState((UIElement)sender, "Pressed");
        }

        private void ToggleButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            muxc.AnimatedIcon.SetState((UIElement)sender, "Normal");
        }

        private void ToggleButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            muxc.AnimatedIcon.SetState((UIElement)sender, "Normal");
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(sender.Text))
            {
                VisualStateManager.GoToState(this, "NonFilteredState", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "FilteredState", true);
            }
        }
    }
}
