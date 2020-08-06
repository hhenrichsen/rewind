using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject 
{
    #if UNITY_EDITOR
    [Multiline]
    public string Description = "";
    #endif
    public int Value;

    public void SetValue(int value) 
    {
        Value = value;
    }

    public void SetValue(IntVariable value) {
        Value = value.Value;
    }

    public void ApplyChange(int value) 
    {
        Value += value;
    }

    public void ApplyChange(IntVariable value) {
        Value += value.Value;
    }

    public string toString() {
        return Value.ToString();
    }
}