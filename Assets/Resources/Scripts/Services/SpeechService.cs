using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

[Serializable]
public class RecognizeEvent : UnityEvent
{ }

[Serializable]
public class RecognizeEventConstructor
{
    public string _keyword;
    public RecognizeEvent _event;
}

public class SpeechService : MonoBehaviour
{
    [SerializeField] private List<RecognizeEventConstructor> _recognizeEvents;

    private KeywordRecognizer _recognizer;
    

    void Start()
    {
        var keywords = new string[_recognizeEvents.Count];
        for (var i = 0; i < keywords.Length; i++)
        {
            keywords[i] = _recognizeEvents[i]._keyword;
        }
        
        _recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Medium);
        _recognizer.OnPhraseRecognized += OnPhraseRecognized;
        _recognizer.Start();
        
        Debug.Log("Keyword recognizer: Listening");
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        var builder = new StringBuilder();
        
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        _recognizeEvents.ForEach(x =>
        {
            if(x._keyword == args.text)
                x._event.Invoke();
        });
    }
}
