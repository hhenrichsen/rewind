using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerInteract : MonoBehaviour {

    [SerializeField]
    public string requiredTag;
    [SerializeField]
    public UnityEvent triggerEnter;

    private Collider collider;

    public void Start() {
        this.collider = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other) {
        if (requiredTag != null && requiredTag != "")
        {
            if (other.gameObject.tag != requiredTag)
            {
                return;
            }
        }
        triggerEnter.Invoke();
    }
}