using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoProjector : MonoBehaviour
{
    public string Title;
#if UNITY_EDITOR
    [Multiline]
#endif
    public string Description = "";
}
