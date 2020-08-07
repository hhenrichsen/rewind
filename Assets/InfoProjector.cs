using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoProjector : MonoBehaviour
{
    public Dialog dialog;
    private UIManager manager;

    public void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();
    }

    public void Respond()
    {
        manager.ShowDialog(dialog);
    }
}
