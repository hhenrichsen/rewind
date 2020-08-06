using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogResponse : MonoBehaviour
{
    [SerializeField]
    private string message;

    public void Respond()
    {
        Debug.Log(message);
    }
}
