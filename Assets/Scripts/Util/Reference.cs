public class Reference<T> {
    public bool UseConstant = true;
    public T ConstantValue;
    public Variable<T> Variable;

    public Reference() { }

    public Reference(T value) 
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public T Value 
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator T(Reference<T> reference) 
    {
        return reference.Value;
    }
}