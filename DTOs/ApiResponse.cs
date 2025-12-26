namespace Net9ApiOdev.DTOs
{
    public class ApiResponse<T>
    {
        // Mutlaka get ve set erişimcileri olmalı
        public bool Success { get; set; } 
        public string Message { get; set; } = string.Empty; 
        public T? Data { get; set; } 

        // Başarılı Cevap için Statik Metot
        public static ApiResponse<T> SuccessResponse(T data, string message = "İşlem başarılı.")
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        // Hata Cevabı için Statik Metot
        public static ApiResponse<T> ErrorResponse(string message, T? data = default)
        {
            return new ApiResponse<T> { Success = false, Message = message, Data = data };
        }
    }
}