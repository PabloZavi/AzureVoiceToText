using DotNetEnv;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

class Program
{    static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
     {
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                break;
            case ResultReason.NoMatch:
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                break;
            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
        }
    }

    async static Task Main(string[] args)
    {
        // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION".
        // Please put your .env path here
        Env.Load(@"C:\Users\MSI\source\repos\AzureVoiceToText\.env");

        string? speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        string? speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        // Important! Put here the language you speak (in this example, is Spanish from Spain)  
        speechConfig.SpeechRecognitionLanguage = "es-ES";

        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        Console.WriteLine("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        OutputSpeechRecognitionResult(speechRecognitionResult);
    }
}