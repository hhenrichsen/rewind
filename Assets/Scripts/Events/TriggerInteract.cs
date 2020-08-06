using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerInteract : MonoBehaviour {

    private Collider collider;
    [SerializeField]
    public UnityEvent triggerEnter;

    public void Start() {
        this.collider = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other) {
        triggerEnter.Invoke();
    }
}