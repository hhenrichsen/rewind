using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public FloatVariable spawnTime;
    public FloatReference initialSpawnTime;

    public FloatVariable effectIntensity;

    public FloatVariable timeRemaining;
    public SpawnPosition defaultSpawn;
    public GameObject player;
    public GameObject camera;


    private float nextSpawn;

    public float NextSpawn { get { return nextSpawn; } }


    void Awake()
    {
        spawnTime.SetValue(initialSpawnTime.Value);
        BackToSpawn();
        Invoke("BackToSpawn", spawnTime.Value);
    }

    void ResetBack() {
        CancelInvoke();
        BackToSpawn();
    }

    void Update() {
        timeRemaining.SetValue(TimeRemaining());
        float intensity = IntensityFromTimeRemaining(timeRemaining.Value);
        effectIntensity.SetValue(intensity);
    }

    private float IntensityFromTimeRemaining(float time) {
        return 1f / (Mathf.Pow(time, 4) + 1);
    }

    public float TimeRemaining() {
        return nextSpawn - Time.time;
    }

    void BackToSpawn() {
        if (SpawnPosition.activeSpawn == null) {
            SpawnPosition.activeSpawn = defaultSpawn;
            defaultSpawn.OnTriggerEnter(null);
        }
        player.transform.position = SpawnPosition.activeSpawn.transform.position;
        player.transform.rotation = SpawnPosition.activeSpawn.transform.rotation;
        camera.transform.position = SpawnPosition.activeSpawn.cameraTarget.position;
        camera.transform.rotation = SpawnPosition.activeSpawn.cameraTarget.rotation;
        nextSpawn = Time.time + spawnTime.Value;
        Invoke("BackToSpawn", spawnTime.Value);
    }
}
