using Microsoft.OpenApi.Models;
using HusRoizBackendTest_MidLevel.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. 註冊服務
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<DefaultValueSchemaFilter>();
});

var app = builder.Build();

// 2. 配置 HTTP 請求管道
// 讓 Swagger 在所有環境都可執行
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // 讓首頁直接就是 Swagger
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();