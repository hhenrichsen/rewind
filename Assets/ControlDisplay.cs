using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class ControlDisplay : MonoBehaviour
{
    [SerializeField]
    public PlayerInput input;
    [SerializeField]
    public Sprite gamepad;
    [SerializeField]
    public Sprite keyboard;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ChangeInput();
    }

    public void ChangeInput()
    {
        if (input.currentControlScheme == "Keyboard")
        {
            spriteRenderer.sprite = keyboard;
        }
        else
        {
            spriteRenderer.sprite = gamepad;
        }
    }
}
