using Net9ApiOdev.DTOs;
using System.ComponentModel.DataAnnotations; // Tam adını kullanıyoruz

namespace Net9ApiOdev.Utilities
{
    public class DataAnnotationsValidator<T> : IValidator<T>
    {
        
        public DTOs.ValidationResult Validate(T model) 
        {
            if (model == null)
            {
               
                return new DTOs.ValidationResult(false, new[] { "Giriş modeli null olamaz." });
            }
            
            
            var context = new ValidationContext(model);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            
           
            bool isValid = Validator.TryValidateObject(model, context, results, true);
            
            
            return new DTOs.ValidationResult(isValid, results.Select(r => r.ErrorMessage ?? "").ToList());
        }
    }
}