namespace Mvc.Framework.Attributes.Validation
{
    public class NumberRangeAttribute : PropertyValidationAttribute
    {
        private readonly double minValue;
        private readonly double maxValue;

        public NumberRangeAttribute(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            var valueAsDouble = value as double?;
            if (valueAsDouble is null)
            {
                return true;
            }

            return this.minValue <= valueAsDouble && valueAsDouble >= this.maxValue;
        }
    }
}
