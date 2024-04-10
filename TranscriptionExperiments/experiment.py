import speech_recognition as sr

# Define a list of recognizable words
#recognizable_words = ['dynamic', 'impulse', 'trigger', 'example', 'phrase']
recognizable_words = [('dynamic', 1), ('impulse', 1), ('trigger', 1), ('example', 1), ('phrase', 1)]

# Initialize the recognizer
r = sr.Recognizer()

# Use the default microphone as the audio source
with sr.Microphone() as source:
    print("Listening...")
    # Adjust the recognizer sensitivity to ambient noise
    r.adjust_for_ambient_noise(source)

    # Continuously listen and process audio
    while True:
        try:
            # Listen to the audio and extract audio data
            audio = r.listen(source)

            # Recognize speech using Google's speech recognition
            recognized_speech = r.recognize_google(audio_data=audio).lower()
            recognized_speech = r.recognize_sphinx(audio_data=audio, keyword_entries=recognizable_words).lower()
            print(f"Recognized Speech: {recognized_speech}")

            # Split the recognized speech into words
            words = recognized_speech.split()

            # Filter the words to only include those in the recognizable words list
            filtered_words = [word for word in words if word in recognizable_words]

            if filtered_words:
                print("Recognized words:", ' '.join(filtered_words))
            else:
                print("No recognizable words were spoken.")

        except sr.UnknownValueError:
            print("Could not understand audio, trying again...")
        except sr.RequestError as e:
            print(f"Could not request results from Google Speech Recognition service; {e}. Trying again...")

