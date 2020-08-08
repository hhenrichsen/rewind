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
    public Dialog thanksDialog;
    public Dialog victoryDialog;
    public Image fade;
    private bool fading;
    private float alpha = 0;
    public float fadeSpeed;
    private bool ended;
    private bool quit;

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
        imageElement.transform.SetAsLastSibling();
    }

    public void FadeToBlack()
    {
        fading = true;
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
        if (fading)
        {
            if (alpha < 1f)
            {
                alpha += fadeSpeed * Time.unscaledDeltaTime;
            }
            else
            {
                alpha = 1f;
                Time.timeScale = 0f;
                fading = false;
                ShowDialog(victoryDialog);
                ended = true;
            }
            Color color = fade.color;
            color.a = alpha;
            fade.color = color;
        }
    }
    

    public void AcknowledgeDialog()
    {
        toggle = true;
        if (ended)
        {
            ShowDialog(thanksDialog);
        }
        if (quit)
        {
            Application.Quit();
        }
    }
}
