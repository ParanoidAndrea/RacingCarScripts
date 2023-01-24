using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    int hour = 0;
    int msecond = 0;
    int minute = 0;
    int second = 0;
    float timeSpend = 0;
    string timeStr = string.Empty;
    private Text timeshow;
    void Start()
    {
        timeshow = GetComponent<Text>();
    }
    void Update()
    {
        timeSpend += Time.deltaTime;

        hour = (int)timeSpend / 3600;
        minute = ((int)timeSpend - hour * 3600) / 60;
        second = (int)timeSpend - hour * 3600 - minute * 60;
        msecond = (int)((timeSpend - (int)timeSpend) * 100);


    }

    void OnGUI()
    {
        timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}", minute, second, msecond);
        timeshow.text = timeStr;
    }
}
