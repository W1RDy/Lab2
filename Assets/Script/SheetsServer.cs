using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class SheetsServer : MonoBehaviour
{
    public AudioClip fastSpeak;
    public AudioClip normalSpeak;
    public AudioClip slowSpeak;
    private AudioSource selectAudio;
    private Dictionary<string, float> dataSet = new Dictionary<string, float>();
    private bool statusStart = false;
    private int i = 1;

    void Start()
    {
        selectAudio = GetComponent<AudioSource>();
        StartCoroutine(GoogleSheets());
    }

    void Update()
    {
        if (statusStart == false && i <= dataSet.Count)
        {
            if (dataSet["Mon_" + i.ToString()] >= 66)
                StartCoroutine(PlaySelectAudioGood());

            if (dataSet["Mon_" + i.ToString()] > 50 & dataSet["Mon_" + i.ToString()] < 66)
                StartCoroutine(PlaySelectAudioNormal());

            if (dataSet["Mon_" + i.ToString()] <= 50)
                StartCoroutine(PlaySelectAudioBad());

            Debug.Log(dataSet["Mon_" + i.ToString()]);
        }
    }

    IEnumerator GoogleSheets()
    {
        UnityWebRequest curentResp = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/10F00A3ixlkkQu_quD6qg9YGjyoNOyws-TAKtKeAKtaE/values/Лист1?key=AIzaSyADGvMwrE5A6w-jCQu6HdstBmpZjSjGrzM");
        yield return curentResp.SendWebRequest();
        string rawResp = curentResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);
        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;
            dataSet.Add(("Mon_" + selectRow[0]), float.Parse(selectRow[2]));
        }
    }

    IEnumerator PlaySelectAudioGood()
    {
        statusStart = true;
        selectAudio.clip = fastSpeak;
        selectAudio.volume = 0.4f;
        selectAudio.Play();
        yield return new WaitForSeconds(selectAudio.clip.length + 1f);
        statusStart = false;
        i++;
    }
    IEnumerator PlaySelectAudioNormal()
    {
        statusStart = true;
        selectAudio.clip = normalSpeak;
        selectAudio.volume = 1f;
        selectAudio.Play();
        yield return new WaitForSeconds(selectAudio.clip.length + 1f);
        statusStart = false;
        i++;
    }
    IEnumerator PlaySelectAudioBad()
    {
        statusStart = true;
        selectAudio.clip = slowSpeak;
        selectAudio.volume = 0.2f;
        selectAudio.Play();
        yield return new WaitForSeconds(selectAudio.clip.length + 1f);
        statusStart = false;
        i++;
    }
}
