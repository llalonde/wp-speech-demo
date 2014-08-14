using DearDiary.Common;
using DearDiary.Helpers;
using DearDiary.Models;
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
using Windows.Media.SpeechSynthesis;
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
    public sealed partial class ViewDiaryEntry : Page
    {
        private NavigationHelper navigationHelper;

        SpeechHelper helper;

        public ViewDiaryEntry()
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
            if (e.Parameter != null)
            {
                if (e.Parameter is DiaryEntry)
                {
                    App.ViewModel.CurrentEntry = e.Parameter as DiaryEntry;
                }
                else
                {
                    ManageActions(e.Parameter.ToString());
                }
            }

            this.DataContext = App.ViewModel.CurrentEntry;
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            string fullText = string.Format("Dear Diary, {0} Entered on {1}", App.ViewModel.CurrentEntry.Details, App.ViewModel.CurrentEntry.EntryDate.ToString("MMM dd yyyy"));
            InitiateSpeech(fullText);
        }

        private async void InitiateSpeech(string textToSpeech)
        {
            helper = new SpeechHelper();
            SpeechSynthesisStream synthesisStream = await helper.SpeakAsync(textToSpeech);

            if (synthesisStream != null)
            {
                this.media.AutoPlay = true;
                this.media.SetSource(synthesisStream, synthesisStream.ContentType);
                this.media.Play();
            }
        }

        private async void ManageActions(string result)
        {
            if (result.Contains("first") || result.Contains("1st"))
            {
                App.ViewModel.CurrentEntry = App.ViewModel.DiaryEntries.FirstOrDefault();
            }
            else if (result.Contains("last"))
            {
                App.ViewModel.CurrentEntry = App.ViewModel.DiaryEntries.LastOrDefault();
            }
            else if (result.Contains("yesterday"))
            {
                App.ViewModel.CurrentEntry = App.ViewModel.DiaryEntries.Where(d => DateTime.Now.Subtract(d.EntryDate) >= new TimeSpan(1, 0, 0, 0)).FirstOrDefault();
                if (App.ViewModel.CurrentEntry == null)
                {
                    InitiateSpeech("You didn't record an entry yesterday.");
                }
            }
            else if (result.Contains("this week"))
            {
                App.ViewModel.CurrentEntry = App.ViewModel.DiaryEntries.Where(d => DateTime.Now.Subtract(d.EntryDate) >= new TimeSpan(7, 0, 0, 0)).FirstOrDefault();
                if (App.ViewModel.CurrentEntry == null)
                {
                    InitiateSpeech("You didn't record an entry this week.");
                }
            }
            else
            {
                if (helper == null)
                    helper = new SpeechHelper();

                await helper.DisplayMessage("Didn't understand that. Try again.");
            }
        }
    }
}
