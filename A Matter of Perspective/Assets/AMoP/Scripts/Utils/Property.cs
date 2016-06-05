
public class Property<T>
{
    private T value;
    public event System.Action<T> Changed;

    public T Value
    {
        get
        {
            return value;
        }

        set
        {
            if (!value.Equals(this.value) && Changed != null)
            {
                this.value = value;
                Changed(this.value);
            }
        }
    }

    public Property(T value)
    {
        this.value = value;
        Changed = null;
    }

    public override bool Equals(object obj)
    {
        return value.Equals(obj);
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public override string ToString()
    {
        return value.ToString();
    }

    public static implicit operator T(Property<T> prop)
    {
        return prop.value;
    }

    public static implicit operator Property<T>(T value)
    {
        return new Property<T>(value);
    }
}
