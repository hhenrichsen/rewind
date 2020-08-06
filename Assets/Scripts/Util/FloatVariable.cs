using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject 
{
    #if UNITY_EDITOR
    [Multiline]
    public string Description = "";
    #endif
    public float Value;

    public void SetValue(float value) 
    {
        Value = value;
    }

    public void SetValue(FloatVariable value) {
        Value = value.Value;
    }

    public void ApplyChange(float value) 
    {
        Value += value;
    }

    public void ApplyChange(FloatVariable value) {
        Value += value.Value;
    }

    public string toString() {
        return Value.ToString();
    }
}