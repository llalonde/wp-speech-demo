using System;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Popups;

namespace DearDiary.Helpers
{
    public class SpeechHelper
    {
        private SpeechRecognizer speechRecognizerUI;

        public async Task InitializeSpeechRecognizerAsync()
        {
            speechRecognizerUI = new SpeechRecognizer();

            speechRecognizerUI.UIOptions.IsReadBackEnabled = true;
            speechRecognizerUI.UIOptions.ShowConfirmation = true;

            SpeechRecognitionTopicConstraint topicConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "Dear Diary");
            speechRecognizerUI.Constraints.Add(topicConstraint);
            await speechRecognizerUI.CompileConstraintsAsync();
        }

        public async Task SetSpeechRecognizerPromptsAsync(string audiblePrompt, string exampleText)
        {
            if (speechRecognizerUI == null)
               await InitializeSpeechRecognizerAsync();

            speechRecognizerUI.UIOptions.AudiblePrompt = audiblePrompt;
            speechRecognizerUI.UIOptions.ExampleText = exampleText;
        }

        public async Task<SpeechRecognitionResult> ShowSpeechUIAsync()
        {
            try
            {
                SpeechRecognitionResult recognitionResult = await speechRecognizerUI.RecognizeWithUIAsync();
                return recognitionResult;
            }
            catch { }

            return null;
        }

        public async Task<SpeechSynthesisStream> SpeakAsync(string textToSpeech)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();

            string errorMessage = string.Empty;

            try
            {
                SpeechSynthesisStream synthesisStream = await synthesizer.SynthesizeTextToStreamAsync(textToSpeech);
                return synthesisStream;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                await DisplayMessage(string.Format("Failed to convert text to speech. Error: {0}", errorMessage));
            }

            return null;
        }

        public async Task DisplayMessage(string messageToDisplay)
        {
            MessageDialog dlg = new MessageDialog(messageToDisplay);
            await dlg.ShowAsync();
        }
    }
}
