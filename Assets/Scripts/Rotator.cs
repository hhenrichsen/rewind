using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    public Vector3 rotationAmount;

    // Update is called once per frame
    void Update()
    {
        Vector3 oldRotation = this.transform.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(oldRotation + rotationAmount);
    }
}
