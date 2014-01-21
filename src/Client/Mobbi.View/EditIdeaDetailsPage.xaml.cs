using Windows.UI.Xaml;
using Mobbi.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mobbi.ViewModel;

namespace Mobbi.View
{
    public sealed partial class EditIdeaDetailsPage : Page
    {
        private IdeaDetailsPageViewModel _viewModel = new IdeaDetailsPageViewModel();

        public IdeaDetailsPageViewModel ViewModel
        {
            get { return this._viewModel; }
        }

        public NavigationHelper NavigationHelper { get; private set; }

        public EditIdeaDetailsPage()
        {
            this.InitializeComponent();
            this.NavigationHelper = new NavigationHelper(this);
            this.NavigationHelper.LoadState += navigationHelper_LoadState;
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            await _viewModel.Initialise((String)e.NavigationParameter);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.Save();
            this.Frame.Navigate(typeof(IdeaDetailsPage), _viewModel.Idea.ExternalId.ToString());
        }
    }
}
