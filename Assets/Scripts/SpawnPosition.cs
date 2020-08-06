using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public bool active;
    public Transform cameraTarget;

    public GameEvent spawnUpdated;

    private bool found;

    public FloatVariable time;

    public FloatVariable increment;
    private List<Spark> sparks = new List<Spark>();
    public static SpawnPosition activeSpawn;
    void Start()
    {
        if (active) {
            activeSpawn = this;
        }
        foreach (Transform child in transform) {
            Spark s = child.GetComponent<Spark>();
            if (s != null) {
                sparks.Add(s);
                if (active) {
                    s.UpdateSprite();
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (!found) {
            this.found = true;
            time.SetValue(time.Value + (increment != null ? increment.Value : 0f));
        }
        if (this != activeSpawn) {
            spawnUpdated.Raise();
            Debug.Log("Moving spawn to " + this.transform.position);
            if(activeSpawn) {
                foreach (Spark spark in activeSpawn.sparks) {
                    spark.UpdateSprite();
                }
            }
            activeSpawn = this;
            active = true;
            foreach (Spark spark in sparks) {
                spark.UpdateSprite();
            }
        }
    }
}
