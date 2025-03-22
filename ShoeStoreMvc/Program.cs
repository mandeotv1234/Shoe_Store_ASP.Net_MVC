using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using ShoeStoreMvc.Services;
using ShoeStoreMvc.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using ShoeStoreMvc.Hubs; // Đảm bảo import đúng

var builder = WebApplication.CreateBuilder(args);

// ✅ Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

// ✅ Cấu hình MongoDB kết nối
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = MongoClientSettings.FromConnectionString(
        builder.Configuration.GetConnectionString("MongoDbConnection")
    );
    return new MongoClient(settings);
});

builder.Services.AddScoped(s =>
{
    var client = s.GetRequiredService<IMongoClient>();
    return client.GetDatabase("dimillav");
});

// ✅ Cấu hình Identity với MongoDB
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, string>(
        builder.Configuration.GetConnectionString("MongoDbConnection"), "dimillav")
    .AddDefaultTokenProviders();

// ✅ Đăng ký các dịch vụ cần thiết
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CartService>(); // Đăng ký CartService
builder.Services.AddScoped<ChatService>(); // ✅ Thêm dòng này

builder.Services.AddSession();

// ✅ Cấu hình Authentication (Google & Facebook)
builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
        options.Fields.Add("email");
        options.Fields.Add("picture.width(100).height(100)");
        options.Scope.Add("email");
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.Scope.Add("email");
        options.Scope.Add("profile");
        options.ClaimActions.MapJsonKey("picture", "picture", "url");
    });

// ✅ Thêm SignalR
builder.Services.AddSignalR();


// ✅ Thêm CORS nếu client chạy trên domain khác
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .WithOrigins("http://localhost:5001", "http://localhost:5002") // Thay bằng domain User & Admin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


var app = builder.Build();

// ✅ Cấu hình pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAllOrigins"); // 🔹 Bật CORS
app.UseRouting();  // 🔹 Bật hệ thống định tuyến


app.UseAuthentication();  // 🔹 Kích hoạt xác thực
app.UseAuthorization();  // 🔹 Kích hoạt phân quyền

// ✅ Định tuyến MVC & SignalR
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // 🔹 Kích hoạt API/MVC Controller
    endpoints.MapHub<ChatHub>("/chatHub");  // 🔹 Kích hoạt SignalR tại "/chatHub"
});

// ✅ Định tuyến mặc định cho ứng dụng MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
