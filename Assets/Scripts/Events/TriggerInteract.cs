using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerInteract : MonoBehaviour {

    private Collider collider;
    [SerializeField]
    public UnityEvent triggerEnter;
    [SerializeField]
    public string tag;

    public void Start() {
        this.collider = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other) {
        if (tag != null && tag != "")
        {
            if (other.gameObject.tag != tag)
            {
                return;
            }
        }
        triggerEnter.Invoke();
    }
}