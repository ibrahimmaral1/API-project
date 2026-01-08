using Net9ApiOdev.DTOs;

namespace Net9ApiOdev.Utilities
{
    // Genel Doğrulama Arayüzü
    public interface IValidator<T> 
    { 
        ValidationResult Validate(T model); 
    }
}