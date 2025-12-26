namespace Net9ApiOdev.DTOs
{
    // Kendi doğrulama sonucumuzu tutan sınıf (IsValid kontrolü için)
    public class ValidationResult 
    {
        public bool IsValid { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        // System.ComponentModel.DataAnnotations'ın yerleşik sonuç listesinden
        // bizim sınıfımıza dönüştürme yapıyoruz.
        public ValidationResult(bool isValid, IEnumerable<string> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }
    }
}