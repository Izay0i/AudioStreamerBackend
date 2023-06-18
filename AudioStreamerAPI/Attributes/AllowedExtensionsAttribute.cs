using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        public string GetErrorMessage()
        {
            return $"This extension is not allowed!";
        }
    }
}
