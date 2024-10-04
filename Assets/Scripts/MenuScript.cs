using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.Linq;

public class MenuScript : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start()
    {
        keywords.Add("OSO", ChangeScene);

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;

        keywordRecognizer.Start();
    }

    private void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (keywords.ContainsKey(args.text))
        {
            keywords[args.text].Invoke();
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("PruebaPersonaje");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("El juego se cerró.");
    }

    void OnDestroy()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}
