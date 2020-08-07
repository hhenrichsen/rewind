using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image imageElement;
    [SerializeField]
    private Text titleElement;
    [SerializeField]
    private Text bodyElement;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private PlayerInput input;
    private bool toggle;
    private bool interacting;
    private Dialog displayDialog;
    public Dialog startDialog;

    public void ShowDialog(Dialog dialog)
    {
        Debug.Log("Showing dialog " + dialog.Title);
        displayDialog = dialog;
        toggle = true;
        interacting = true;
    }

    public void Start()
    {
        ShowDialog(startDialog);
    }

    public void Update()
    {
        if (toggle)
        {
            if (interacting)
            {
                Debug.Log("Displaying");
                icon.sprite = displayDialog.icon;
                titleElement.text = displayDialog.Title;
                bodyElement.text = displayDialog.Description;
                input.SwitchCurrentActionMap("UI");
                imageElement.gameObject.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                Debug.Log("Closing display");
                input.SwitchCurrentActionMap("Player");
                imageElement.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
            interacting = !interacting;
            toggle = false;
        }
    }

    public void AcknowledgeDialog()
    {
        toggle = true;
    }
}
