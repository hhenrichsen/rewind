using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Spark : MonoBehaviour {
    public bool active = false;
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    private SpriteRenderer renderer;

    void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = active ? activeSprite : inactiveSprite;
    }

    public void UpdateSprite() {
        if (active) {
            renderer.sprite = inactiveSprite;
        }
        else {
            renderer.sprite = activeSprite;
        }
        active = !active;
    }
}