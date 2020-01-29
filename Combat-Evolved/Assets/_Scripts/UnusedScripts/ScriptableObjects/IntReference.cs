using System;

public class IntReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public IntReference Variable;

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
