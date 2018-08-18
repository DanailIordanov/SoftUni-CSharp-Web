namespace Mvc.Framework.Attributes.Validation
{
    using System.Text.RegularExpressions;

    public class RegexAttribute : PropertyValidationAttribute
    {
        private readonly string pattern;

        public RegexAttribute(string pattern)
        {
            this.pattern = $"^{pattern}$";
        }

        public override bool IsValid(object value)
        {
            if (!(value is string valueAsString))
            {
                return true;
            }

            return Regex.IsMatch(valueAsString, this.pattern);
        }
    }
}
