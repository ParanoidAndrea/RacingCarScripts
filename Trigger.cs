Xusing System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    Text text1, text2;
    void Start()
    {
        text1 = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        text2 = GameObject.FindGameObjectWithTag("end").GetComponent<Text>();

    }
    void End()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    private void OnTriggerEnter(Collider collision)
    {
        text2.text = text1.text;

        Invoke("End", 3);
    }

}
