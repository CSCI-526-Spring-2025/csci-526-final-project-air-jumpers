using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;

    private long _sessionID;
    private int _newPlatformCount;
    private int _reachedLevel;
    // Start is called before the first frame update

    private void Awake()
    {
        _sessionID = DateTime.Now.Ticks;

        Send();
    }

    private IEnumerator Post(string sessionID, string newPlatformCount, string reachedLevel)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.595435075", sessionID);
        form.AddField("entry.220358108", newPlatformCount);
        form.AddField("entry.1107099891", reachedLevel);

        // Debug.Log(sessionID);


        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    public void Send()
    {
        _newPlatformCount = 10;
        _reachedLevel = 2;

        StartCoroutine(Post(_sessionID.ToString(), _newPlatformCount.ToString(), _reachedLevel.ToString()));
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
