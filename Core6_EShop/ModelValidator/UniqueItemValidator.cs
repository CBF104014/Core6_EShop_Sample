using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.ModelValidator
{
    public class UniqueItemValidator : ValidationAttribute
    {
        public string FieldName { get; set; }
        public UniqueItemValidator(string fieldName) : base("值不能重複")
        {
            this.FieldName = fieldName;
        }
        public UniqueItemValidator(string errorMessage, string fieldName) : base(errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.FieldName = fieldName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IEnumerable<object>;
            if (String.IsNullOrEmpty(FieldName) || list == null || !list.Any())
            {
                return ValidationResult.Success;
            }
            var duplicateNums = list
                .GroupBy(x => x.GetType().GetProperty(FieldName)?.GetValue(x))
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicateNums.Any())
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
