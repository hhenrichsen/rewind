using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationResponse : MonoBehaviour {
    [SerializeField]
    private string trigger;
    
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Respond() {
        animator.SetTrigger(trigger);
    }
}