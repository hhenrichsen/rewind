using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Dialog Contents")]
public class Dialog : ScriptableObject
{

    public string Title;
#if UNITY_EDITOR
    [Multiline]
#endif
    public string Description = "";
    public Sprite icon;
}
