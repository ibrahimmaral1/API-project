// EN ÜSTTE OLMASI GEREKEN TÜM using'ler
using Net9ApiOdev.Data;
using Net9ApiOdev.Data.Repositories;
using Net9ApiOdev.DTOs;
using Net9ApiOdev.Service;
using Net9ApiOdev.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
// ----------------------------------------------------
// 1. BUILDER (SERVİS TANIMLAMA) AŞAMASI
// ----------------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// --- 1.1. Servislerin Tanımlanması ---


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), 
        b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    );
});


builder.Services.AddEndpointsApiExplorer();
// Swagger JWT Ayarları (Kilit Butonu İçin)
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Net9ApiOdev", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Lütfen token'ı 'Bearer [TOKEN]' formatında giriniz.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// DI için Service ve Repository servisleri
builder.Services.AddScoped<IUserRepository, UserRepository>(); 
builder.Services.AddScoped<IUserService, UserService>(); 
builder.Services.AddScoped<IAuthService, AuthService>();
// DTO doğrulama için gerekli (Data Annotations)
builder.Services.AddScoped(typeof(IValidator<>), typeof(DataAnnotationsValidator<>));
// Program.cs içinde, DI kayıtlarından sonra:

// JWT Konfigürasyonunu okuma
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

// Authentication (Kimlik Doğrulama) servisini ekleme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        // Appsettings.json'dan okunan değerler:
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        
        // KRİTİK NOKTA BURASI: Anahtarın okunması
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "Varsayilan_Uzun_Bir_Anahtar_123!") // Eğer null gelirse yedek anahtar
        )
    };
});

// Authorization (Yetkilendirme) servisini ekleme
builder.Services.AddAuthorization();

// ----------------------------------------------------
// 2. APP (MIDDLEWARE VE ENDPOINT) AŞAMASI
void MigrateDatabase(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Veritabanı yoksa oluşturur ve migration'ları uygular
        dbContext.Database.Migrate(); 
    }
}
var app = builder.Build();

// --- 2.1. Middleware'lar ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
       context.Response.StatusCode = StatusCodes.Status500InternalServerError; 
        context.Response.ContentType = "application/json";

        var response = ApiResponse<object>.ErrorResponse("Sunucuda beklenmeyen bir hata oluştu."); 

        // Hata loglaması (Logging gereksinimi için)
        app.Logger.LogError(context.Features.Get<IExceptionHandlerPathFeature>()?.Error, 
                            "Global hata yakalandı.");

        await context.Response.WriteAsJsonAsync(response);
    });
});

app.UseHttpsRedirection();
app.UseAuthentication(); // Kimsin? (Token'ı doğrula)
app.UseAuthorization();  // Ne yapabilirsin? (Rolleri kontrol et)
// ----------------------------------------------------
// 3. MINIMAL API ENDPOINT'LERİ (CONTROLLER KATMANI)
// ----------------------------------------------------

// Kaynak odaklı URL yapısı: /users
var usersApi = app.MapGroup("/users")
                  .WithTags("Users")
                .RequireAuthorization();
// 1. GET: Tüm Kullanıcıları Listele
usersApi.MapGet("/", async (IUserService userService) =>
{
    var users = await userService.GetAllUsersAsync();
    return Results.Ok(ApiResponse<IEnumerable<UserResponseDTO>>.SuccessResponse(users));
})
.Produces<ApiResponse<IEnumerable<UserResponseDTO>>>(StatusCodes.Status200OK); 
// 2. GET: ID'ye Göre Kullanıcı Getir
usersApi.MapGet("/{id}", async (int id, IUserService userService) =>
{
    var user = await userService.GetUserByIdAsync(id);

    if (user == null)
    {
        return Results.NotFound(ApiResponse<object>.ErrorResponse($"ID {id} olan kullanıcı bulunamadı.")); 
    }
    return Results.Ok(ApiResponse<UserResponseDTO>.SuccessResponse(user)); 
});

// 3. POST: Yeni Kullanıcı Oluştur
usersApi.MapPost("/", async (
    UserCreateDTO userDto, 
    IUserService userService,
    IValidator<UserCreateDTO> validator // <-- YENİ: Validator'ı parametre olarak ekledik
) =>
{
    // Artık 'app.Services' kullanmaya gerek yok, doğrudan 'validator' parametresini kullanıyoruz
    var validationResult = validator.Validate(userDto);
    
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(ApiResponse<object>.ErrorResponse("Giriş verileri geçerli değil.")); 
    }
    
    var createdUser = await userService.CreateUserAsync(userDto);

    return Results.Created($"/users/{createdUser.Id}", 
                           ApiResponse<UserResponseDTO>.SuccessResponse(createdUser, "Kullanıcı başarıyla oluşturuldu.")); 
})
.Produces<ApiResponse<UserResponseDTO>>(StatusCodes.Status201Created);

// 4. PUT: Kullanıcı Güncelle
usersApi.MapPut("/{id}", async (int id, UserUpdateDTO userDto, IUserService userService) =>
{
    var success = await userService.UpdateUserAsync(id, userDto);

    if (!success)
    {
        return Results.NotFound(ApiResponse<object>.ErrorResponse($"ID {id} olan kullanıcı bulunamadı."));
    }

   return Results.NoContent(); 
});

// 5. DELETE: Kullanıcı Sil
usersApi.MapDelete("/{id}", async (int id, IUserService userService) =>
{
    var success = await userService.DeleteUserAsync(id);

    if (!success)
    {
        return Results.NotFound(ApiResponse<object>.ErrorResponse($"ID {id} olan kullanıcı zaten mevcut değil."));
    }
    
   return Results.NoContent(); 
});


var authApi = app.MapGroup("/auth")
                 .WithTags("Auth");

// POST: /auth/login (Kullanıcı Girişi)
authApi.MapPost("/login", async (LoginDTO loginDto, IAuthService authService, IUserRepository userRepository) =>
{
    var isValid = await authService.ValidateUserCredentials(loginDto.Email, loginDto.Password);

    if (!isValid)
    {
        
       return Results.Json(ApiResponse<object>.ErrorResponse("E-posta veya şifre hatalı."), statusCode: StatusCodes.Status401Unauthorized);
    }
    
    // Kullanıcıyı tekrar çekiyoruz (Rol ve Id almak için)
    var user = await userRepository.GetUserByEmailAsync(loginDto.Email);
    
    // Token üretme
    var token = authService.GenerateJwtToken(user!.Username, user.Role, user.Id);
    
    var responseData = new { Token = token };

    // 200 OK ile Token'ı döndür
    return Results.Ok(ApiResponse<object>.SuccessResponse(responseData, "Giriş başarılı."));
})
.Produces<ApiResponse<object>>(StatusCodes.Status200OK)
.Produces<ApiResponse<object>>(StatusCodes.Status401Unauthorized);


// Veritabanını otomatik uygula
MigrateDatabase(app);
// ----------------------------------------------------
// 4. UYGULAMAYI ÇALIŞTIRMA
// ----------------------------------------------------
app.Run();
