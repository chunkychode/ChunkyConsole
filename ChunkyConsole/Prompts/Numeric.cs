using System;


namespace ChunkyConsole.Prompts
{
    public class Numeric<T> : PromptItem where T : struct, IConvertible, IComparable<T>
    {
        public Numeric()
        {
            this.Validator = new Validators.Numeric<T>();
        }
    }

    public class Int : Numeric<int> { }
    public class Double : Numeric<double> { }
    public class Long : Numeric<long> { }
    public class Decimal : Numeric<decimal> { }
    public class Float : Numeric<float> { }
}
