namespace MonoBulletHell.AnimatedValues;

public class ConstantAnimatedFloat : IAnimatedFloat
{
    private readonly float _value;

    public ConstantAnimatedFloat(float value)
    {
        _value = value;
    }

    public float Evaluate(float time) => _value;
}