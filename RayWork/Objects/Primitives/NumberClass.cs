namespace RayWork.Objects;

// double = 1.79e308 or 1.79 * 10 ^ 308
// this = 1.79ee308 or 1.79 * 10 ^ (1.79 * 10 ^ 308)
public record NumberClass
{
    public static readonly NumberClass Zero = new();
    public static readonly NumberClass One = new(1);
    public static readonly NumberClass Two = new(2);
    public static readonly NumberClass E = new(Math.E);
    public static readonly NumberClass MaxValue = new(9.999999999999999, double.MaxValue);

    public double Mantissa;
    public double Exponent;

    public NumberClass(double mantissa = 0, double exponent = 0)
    {
        Update(mantissa, exponent);
    }

    public NumberClass(string number)
    {
        var splitByE = number
            .ToLower()
            .Split('e')
            .Select(betweenNumber =>
                betweenNumber.Any() ? double.Parse(betweenNumber) : 1)
            .ToArray();

        switch (splitByE.Length)
        {
            case 1:
                Update(splitByE[0]);
                break;
            case 2:
                Update(splitByE[0], splitByE[1]);
                break;
            case 3:
                Update(splitByE[0], splitByE[1] * Math.Pow(10, splitByE[2]));
                break;
            default:
                throw new ArgumentException($"{number} is too big to interpret, please try to decrease the amount");
        }
    }

    private void Update(double mantissa = 0, double exponent = 0)
    {
        if (mantissa == 0)
        {
            Mantissa = 0;
            Exponent = 0;
            return;
        }

        var log = (long) Math.Floor(Math.Log10(Math.Abs(mantissa)));
        Exponent = exponent + log;
        Mantissa = mantissa / Math.Pow(10, log);
    }

    # region Operator Overloading

    public static NumberClass operator +(NumberClass n1, NumberClass n2)
    {
        var delta = n1.Exponent - n2.Exponent;
        if (Math.Abs(delta) >= 15) return n1.Max(n2);

        return delta switch
        {
            0 => new(n1.Mantissa + n2.Mantissa, n1.Exponent),
            < 0 => new NumberClass(n1.Mantissa / Math.Pow(10, Math.Abs(delta)) + n2.Mantissa, n2.Exponent),
            _ => new NumberClass(n2.Mantissa / Math.Pow(10, delta) + n1.Mantissa, n1.Exponent)
        };
    }

    public static NumberClass operator -(NumberClass n1, NumberClass n2)
    {
        var delta = n1.Exponent - n2.Exponent;
        if (Math.Abs(delta) >= 15) return n1.Max(n2);

        return delta switch
        {
            0 => new(n1.Mantissa - n2.Mantissa, n1.Exponent),
            < 0 => new NumberClass(n1.Mantissa / Math.Pow(10, Math.Abs(delta)) - n2.Mantissa, n2.Exponent),
            _ => new NumberClass(n1.Mantissa - n2.Mantissa / Math.Pow(10, delta), n1.Exponent)
        };
    }

    public static NumberClass operator *(NumberClass n1, NumberClass n2)
    {
        if (n1 == Zero || n2 == Zero) return Zero;
        if (n1 == One) return n2;
        if (n2 == One) return n1;
        return new NumberClass(n1.Mantissa * n2.Mantissa, n1.Exponent + n2.Exponent);
    }

    public static NumberClass operator /(NumberClass n1, NumberClass n2)
    {
        if (n1 == Zero) return Zero;
        if (n2 == Zero) throw new DivideByZeroException();
        if (n2 == One) return n1;
        if (n1 == n2) return One;
        return new NumberClass(n1.Mantissa / n2.Mantissa, n1.Exponent - n2.Exponent);
    }

    public static bool operator <(NumberClass n1, NumberClass n2)
    {
        if (n1.Exponent < n2.Exponent) return true;
        return n1.Mantissa < n2.Mantissa;
    }

    public static bool operator <=(NumberClass n1, NumberClass n2)
    {
        return n1 < n2 || n1 == n2;
    }

    public static bool operator >(NumberClass n1, NumberClass n2)
    {
        return !(n1 <= n2);
    }

    public static bool operator >=(NumberClass n1, NumberClass n2)
    {
        return !(n1 < n2);
    }

    #endregion

    #region Casting Overloading

    public static implicit operator NumberClass(double number)
    {
        return new NumberClass(number);
    }
    
    public static implicit operator NumberClass((double mantissa, double exponent) number)
    {
        return new NumberClass(number.mantissa, number.exponent);
    }
    
    public static implicit operator NumberClass(string number)
    {
        return new NumberClass(number);
    }

    #endregion

    public NumberClass Max(NumberClass number)
    {
        return this > number ? this : number;
    }

    public NumberClass Power(double powerBy)
    {
        return new NumberClass(Math.Pow(powerBy, Mantissa), Exponent);
    }

    public NumberClass Power(NumberClass powerBy)
    {
        return new NumberClass(Math.Pow(powerBy.Mantissa, Mantissa), Exponent + powerBy.Exponent);
    }

    public NumberClass Log()
    {
        return Log(E);
    }

    public NumberClass Log(NumberClass logBase)
    {
        if (this == Zero) return Zero;
        return Log10() / logBase.Log10();
    }

    public NumberClass Log2()
    {
        return Log(Two);
    }

    public double Log10()
    {
        return Exponent + Math.Log10(Mantissa);
    }

    public override string ToString()
    {
        return ToString(true);
    }

    public string ToString(bool cutOffTrail)
    {
        if (!cutOffTrail && Math.Abs(Exponent) < 15) return $"{Mantissa * Math.Pow(10, Exponent)}";
        if (Exponent is < 7 and > -4) return $"{Mantissa * Math.Pow(10, Exponent):###,##0.###}";
        if (Math.Abs(Exponent) >= 1e8) return $"e{new NumberClass(Exponent)}";

        return $"{Mantissa:0.00}e{Exponent:###,##0.###}";
    }

    public string ToLongString()
    {
        return $"{this} | {Mantissa} | {Exponent}";
    }
}