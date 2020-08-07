using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class VariableReader : MonoBehaviour
{
    private Text text;
    public FloatReference variable;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int time = Mathf.RoundToInt(variable.Value);
        int minutes = time / 60;
        int seconds = time % 60;
        text.text = minutes + ":" + seconds.ToString("00");
    }
}
