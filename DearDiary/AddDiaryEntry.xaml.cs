using DearDiary.Common;
using DearDiary.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.SpeechRecognition;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DearDiary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddDiaryEntry : Page
    {
        private NavigationHelper navigationHelper;

        SpeechHelper helper;

        public AddDiaryEntry()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

       

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            string voiceResult = string.Empty;
            if (e.Parameter != null)
                voiceResult = e.Parameter.ToString();

            if (voiceResult.ToLower().StartsWith("add new entry"))
                voiceResult = "";

            App.ViewModel.CurrentEntry = new Models.DiaryEntry { EntryDate = DateTime.Now, Details = voiceResult };
            this.DataContext = App.ViewModel.CurrentEntry;
            PromptUserToSpeak();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = sender as AppBarButton;
            if (btn.Label == "save")
            {
                App.ViewModel.AddNewEntry(App.ViewModel.CurrentEntry);
                await helper.DisplayMessage("Saved!");
                this.Frame.GoBack();
            }
            else
            {
                PromptUserToSpeak();
            }

        }

        private async void PromptUserToSpeak()
        {
            string displayMessage = string.Empty;

            try
            {
                if (helper == null)
                {
                    helper = new SpeechHelper();
                    await helper.SetSpeechRecognizerPromptsAsync("Tell me more...", "");
                }

                SpeechRecognitionResult recognitionResult = await helper.ShowSpeechUIAsync();

                if (recognitionResult.Status == SpeechRecognitionResultStatus.Success)
                {
                    string resultText = recognitionResult.Text;
                    App.ViewModel.CurrentEntry.Details += resultText;
                }
            }
            catch (Exception ex)
            {
                displayMessage = ex.Message;
            }

            if (!string.IsNullOrEmpty(displayMessage))
            {
                await helper.DisplayMessage(displayMessage);
            }
        }


    }
}
