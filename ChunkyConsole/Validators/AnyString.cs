using ChunkyConsole.Utils;
using System;

namespace ChunkyConsole.Validators
{
    public class RegEx : NonBlankString
    {
        public RegEx(string pattern) { this.Pattern = pattern; }
        public RegEx() { }
        public string Pattern { get; set; }

        public const string EMAIL = @"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$";
        public const string PHONENUMBER = @"^(\(?(\d{3})\)?\s?-?\s?(\d{3})\s?-?\s?(\d{4}))$";
        public const string STRONGPWD = @"^(?=[\x21-\x7E]*[0-9])(?=[\x21-\x7E]*[A-Z])(?=[\x21-\x7E]*[a-z])(?=[\x21-\x7E]*[\x21-\x2F|\x3A-\x40|\x5B-\x60|\x7B-\x7E])[\x21-\x7E]{6,}$";

        public override bool Validate(string val)
        {
            return base.Validate(val) && System.Text.RegularExpressions.Regex.IsMatch(Value, Pattern);

        }
    }
    public class NumericRange<K> : Numeric<K> where K : struct, IConvertible, IComparable<K>, IComparable, IFormattable
    {
        public K Min { get; set; }
        public K Max { get; set; }
        public NumericRange(K min, K max)
        {
            this.Min = min;
            this.Max = max;
            this.ErrorMessage = string.Format("Value must be between [{0}] and [{1}]", Min, Max);
        }
        public override bool Validate(string val)
        {
            return base.Validate(val) && Convert.ToUInt64(Value) >= Convert.ToUInt64(Min) && Convert.ToUInt64(Value) <= Convert.ToUInt64(Max);
        }
    }

    public class IntRange : NumericRange<int> { public IntRange(int min, int max) : base(min, max) { } }
    public class DoubleRange : NumericRange<double> { public DoubleRange(double min, double max) : base(min, max) { } }
    public class LongRange : NumericRange<long> { public LongRange(long min, long max) : base(min, max) { } }
    public class DecimalRange : NumericRange<decimal> { public DecimalRange(decimal min, decimal max) : base(min, max) { } }
    public class FloatRange : NumericRange<float> { public FloatRange(float min, float max) : base(min, max) { } }
    public class NonBlankString : BaseValidator<string>
    {
        public class Numeric<K> : BaseValidator<K> where K : struct, IConvertible, IComparable<K>
        {
            public Numeric() : this("Input must be numeric") { }
            public Numeric(string errorMessage) : base(errorMessage) { }
            public override bool Validate(string val)
            {
                K k = default(K);
                if (k is Int64 && new Int64().ParseNullable(val).HasValue)
                {
                    Assignment((K)Convert.ChangeType(new Int64().ParseNullable(val).Value, typeof(K)));
                }
                else if (k is Int32 && new Int32().ParseNullable(val).HasValue)
                {
                    Assignment((K)Convert.ChangeType(new Int32().ParseNullable(val).Value, typeof(K)));
                }
                else if (k is double && new double().ParseNullable(val).HasValue)
                {
                    Assignment((K)Convert.ChangeType(new double().ParseNullable(val).Value, typeof(K)));
                }
                else if (k is decimal && new decimal().ParseNullable(val).HasValue)
                {
                    Assignment((K)Convert.ChangeType(new decimal().ParseNullable(val).Value, typeof(K)));
                }
                else if (k is double && new double().ParseNullable(val).HasValue)
                {
                    Assignment((K)Convert.ChangeType(new double().ParseNullable(val).Value, typeof(K)));
                }
                else
                    return false;

                return true;
            }
        }

        public class Int : Numeric<int> { }
        public class Double : Numeric<int> { }
        public class Long : Numeric<long> { }
        public class Decimal : Numeric<decimal> { }
        public class Float : Numeric<float> { }
        public NonBlankString() : this("value may not be blank") { }
        public NonBlankString(string errorMessage) : base(errorMessage) { }
        public override bool Validate(string val)
        {
            if (!String.IsNullOrEmpty(val))
            {
                Assignment(val);
                return true;
            }
            //Value = val;
            return false;
        }
    }
    public interface IValue<T>
    {
        T Value { get; }
    }
    public interface IValidator
    {
        bool Validate(string val);

        string ErrorMessage { get; }

        Action<object> Assignment { get; set; }
    }
    public class IP : BaseValidator<System.Net.IPAddress>
    {
        public IP() : base("Value must be a valid IPAddress") { }
        public IP(string errorMessage) : base(errorMessage) { }
        public override bool Validate(string val)
        {
            System.Net.IPAddress ip;
            if (System.Net.IPAddress.TryParse(val, out ip))
            {
                //Value = ip;
                Assignment(ip);
                return true;
            }
            return false;

        }
    }
    public class File : NonBlankString
    {
        public File() : this(false) { }
        public File(bool mustExist) { this.MustExist = mustExist; }
        public bool MustExist { get; set; }
        public override bool Validate(string val)
        {
            return base.Validate(val) && MustExist ? System.IO.File.Exists(Value) : true;
        }
    }
    public class Enum<T> : BaseValidator<T> where T : struct, IConvertible
    {

        public Enum() : this("value must be of type " + typeof(T).Name) { }
        public Enum(string errorMessage) : base(errorMessage) { }
        public bool UseValue { get; set; }

        public override bool Validate(string val)
        {
            int i = 0;
            if (UseValue && int.TryParse(val, out i))
            {
                foreach (var x in Enum.GetValues(typeof(T)))
                {
                    if ((int)x == i)
                    {
                        Assignment((T)x);
                        return true;
                    }
                }
            }
            else
            {
                try
                {
                    var oret = Enum.Parse(typeof(T), val);
                    Assignment((T)oret);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }
    }
    public class Directory : NonBlankString
    {
        public Directory() : this(false, false)
        {
            this.ErrorMessage = MustExist ? "Directory must exist" : "value may not be blank";
        }
        public Directory(bool autoCreate) : this(autoCreate, false) { }
        public Directory(bool autoCreate, bool mustExist) { this.AutoCreate = autoCreate; this.MustExist = mustExist; }
        public bool AutoCreate { get; set; }

        public bool MustExist { get; set; }

        public override bool Validate(string val)
        {
            return base.Validate(val) && AutoCreate ? System.IO.Directory.CreateDirectory(Value) != null : MustExist ? System.IO.Directory.Exists(Value) : true;
        }
    }
    public class DateTime : BaseValidator<System.DateTime>
    {
        public DateTime() : base("Value must be a valid Date Time format") { }

        public DateTime(string errorMessage) : base(errorMessage) { }

        public override bool Validate(string val)
        {
            System.DateTime dt;
            if (System.DateTime.TryParse(val, out dt))
            {
                Assignment(dt);
                //Value = dt;
                return true;
            }
            return false;
        }
    }
    public class Bool : BaseValidator<bool>
    {
        public Bool() : base("Value must be [true | t | false | f]") { }

        public Bool(string errorMessage) : base(errorMessage) { }

        public override bool Validate(string val)
        {
            val = (String.Compare(val, "true", StringComparison.OrdinalIgnoreCase) == 0) || (String.Compare(val, "t", StringComparison.OrdinalIgnoreCase) == 0) ? bool.TrueString : val;
            val = (String.Compare(val, "false", StringComparison.OrdinalIgnoreCase) == 0) || (String.Compare(val, "f", StringComparison.OrdinalIgnoreCase) == 0) ? bool.FalseString : val;

            System.Boolean dt;
            if (System.Boolean.TryParse(val, out dt))
            {
                Assignment(dt);
                //Value = dt;
                return true;
            }
            return false;
        }
    }
    public abstract class BaseValidator<T> : IValidator, IValue<T>
    {
        public Action<object> Assignment { get; set; }


        public BaseValidator(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.Assignment = (t => Value = (T)t);
        }

        public T Value { get; set; }
        public abstract bool Validate(string val);
        public string ErrorMessage { get; set; }

        public static IValidator InstanceByType(object source, string propertyName)
        {
            var prop = source.GetType().GetProperty(propertyName);
            if (prop != null)
            {
                return InstanceByType(source.GetType().GetProperty(propertyName).PropertyType);
            }
            else
                return new Validators.AnyString();

        }
        public static IValidator InstanceByType(Type ptype)
        {
            if (ptype.Equals(typeof(int)))
                return new Validators.Int();
            else if (ptype.Equals(typeof(decimal)))
                return new Validators.Decimal();
            else if (ptype.Equals(typeof(double)))
                return new Validators.Double();
            else if (ptype.Equals(typeof(float)))
                return new Validators.Float();
            else if (ptype.Equals(typeof(long)))
                return new Validators.Long();
            else if (ptype.Equals(typeof(string)))
                return new Validators.NonBlankString();
            else if (ptype.Equals(typeof(System.Boolean)))
                return new Validators.Bool();
            else if (ptype.Equals(typeof(System.DateTime)))
                return new Validators.DateTime();

            else
                return new Validators.AnyString();
        }
    }
    public class Numeric<K> : BaseValidator<K> where K : struct, IConvertible, IComparable<K>
    {
        public Numeric() : this("Input must be numeric") { }
        public Numeric(string errorMessage) : base(errorMessage) { }
        public override bool Validate(string val)
        {
            K k = default(K);
            if (k is Int64 && new Int64().ParseNullable(val).HasValue)
            {
                Assignment((K)Convert.ChangeType(new Int64().ParseNullable(val).Value, typeof(K)));
            }
            else if (k is Int32 && new Int32().ParseNullable(val).HasValue)
            {
                Assignment((K)Convert.ChangeType(new Int32().ParseNullable(val).Value, typeof(K)));
            }
            else if (k is double && new double().ParseNullable(val).HasValue)
            {
                Assignment((K)Convert.ChangeType(new double().ParseNullable(val).Value, typeof(K)));
            }
            else if (k is decimal && new decimal().ParseNullable(val).HasValue)
            {
                Assignment((K)Convert.ChangeType(new decimal().ParseNullable(val).Value, typeof(K)));
            }
            else if (k is double && new double().ParseNullable(val).HasValue)
            {
                Assignment((K)Convert.ChangeType(new double().ParseNullable(val).Value, typeof(K)));
            }
            else
                return false;

            return true;
        }
    }
    public class Int : Numeric<int> { }
    public class Double : Numeric<int> { }
    public class Long : Numeric<long> { }
    public class Decimal : Numeric<decimal> { }
    public class Float : Numeric<float> { }
    class AnyString : BaseValidator<string>
    {
        public AnyString() : this("value may be anything") { }
        public AnyString(string errorMessage) : base(errorMessage) { }
        public override bool Validate(string val)
        {
            Assignment(val);
            return true;
        }
    }
}
