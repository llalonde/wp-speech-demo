using DearDiary.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace DearDiary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        SpeechHelper helper;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.DataContext = App.ViewModel;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = sender as AppBarButton;
            if (btn.Label == "new entry")
            {
                this.Frame.Navigate(typeof(AddDiaryEntry));
            }
            else if (btn.Label == "speak")
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
                    await helper.SetSpeechRecognizerPromptsAsync("What would you like to do?", "Ex: new journal entry, show last entry");
                }

                SpeechRecognitionResult recognitionResult = await helper.ShowSpeechUIAsync();

                if (recognitionResult.Status == SpeechRecognitionResultStatus.Success)
                {
                    string result = recognitionResult.Text;
                    string voiceCommand = (result.ToLower().Contains("new journal entry") || result.ToLower().Contains("add entry")) ? "AddEntry" :
                        (result.ToLower().Contains("dear diary")) ? "EagerEntry" :
                        ((result.ToLower().StartsWith("view") || result.ToLower().StartsWith("show")) && result.ToLower().Contains("entry")) ? "ViewEntry" : "";

                    switch (voiceCommand)
                    {
                        case "ViewEntry":
                            this.Frame.Navigate(typeof(ViewDiaryEntry), result);
                            break;
                        case "AddEntry":
                            this.Frame.Navigate(typeof(AddDiaryEntry), "");
                            break;
                        case "EagerEntry":
                            this.Frame.Navigate(typeof(AddDiaryEntry), result);
                            break;
                        default:
                            displayMessage = "Didn't understand that. Try again.";
                            break;
                    }

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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewDiaryEntry), e.AddedItems[0]);

        }

    }
}
