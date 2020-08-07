using UnityEngine;

public class MoveResponse : MonoBehaviour {
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float time;

    private Vector3 goal;
    private bool move;
    private float movementCompleted = 0f;

    void Start() {
        goal = this.transform.position + offset;
    }

    public void Respond() {
        move = true;
    }

    void Update() {
        if (move) {
            if(movementCompleted < time) {
                movementCompleted += Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.goal, movementCompleted);
            }
            else if(this.transform.position != this.goal) {
                this.transform.position = this.goal;
            }
        }
    }
}