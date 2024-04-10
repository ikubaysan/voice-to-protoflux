import azure.cognitiveservices.speech as speechsdk


def main():
    # Set up the Speech SDK with your Azure subscription key and service region
    speech_key, service_region = "43f11dfc6b874275a60d33acad039d93", "westus"
    speech_config = speechsdk.SpeechConfig(subscription=speech_key, region=service_region)

    # Set up the audio config for the microphone
    audio_config = speechsdk.audio.AudioConfig(use_default_microphone=True)

    # Create a speech recognizer with the given settings
    speech_recognizer = speechsdk.SpeechRecognizer(speech_config=speech_config, audio_config=audio_config)

    # Setting up a list of phrases that are more likely to be recognized
    phrases = ["dynamic", "impulse", "trigger", "with", "data", "of", "type", "string", "integer", "boolean", "bool",
               "float", "if", "hello", "test", "testing"]

    phrase_list_grammar = speechsdk.PhraseListGrammar.from_recognizer(recognizer=speech_recognizer)

    for phrase in phrases:
        phrase_list_grammar.addPhrase(phrase)

    print("Speak into your microphone.")

    # Starts speech recognition, and returns after a single utterance is recognized. The end of a
    # single utterance is determined by listening for silence at the end or until a maximum of 15
    # seconds of audio is processed.  The task returns the recognition text as result.
    result = speech_recognizer.recognize_once()

    # Checks result.
    if result.reason == speechsdk.ResultReason.RecognizedSpeech:
        print("Recognized: {}".format(result.text))
    elif result.reason == speechsdk.ResultReason.NoMatch:
        print("No speech could be recognized")
    elif result.reason == speechsdk.ResultReason.Canceled:
        cancellation_details = result.cancellation_details
        print("Speech Recognition canceled: {}".format(cancellation_details.reason))
        if cancellation_details.reason == speechsdk.CancellationReason.Error:
            print("Error details: {}".format(cancellation_details.error_details))


if __name__ == "__main__":
    main()
