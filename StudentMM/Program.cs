using AntDesign;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ProtoBuf.Grpc.Client;
using StudentMM.Common.IServices;
using StudentMM.Data;

var builder = WebApplication.CreateBuilder(args);

string grpcUrl = builder.Configuration.GetSection("GrpcServer")["Url"] ?? throw new InvalidOperationException("Cannot find gRPC URL!");
builder.Services.AddAntDesign();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddSingleton(serviceProvider =>
{
    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
    return GrpcChannel.ForAddress(grpcUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
});

builder.Services.AddSingleton<ISinhVienGrpcService>(serviceProvider =>
{
    var channel = serviceProvider.GetRequiredService<GrpcChannel>();
    return channel.CreateGrpcService<ISinhVienGrpcService>();
});

builder.Services.AddSingleton<ILopHocGrpcService>(serviceProvider =>
{
    var channel = serviceProvider.GetRequiredService<GrpcChannel>();
    return channel.CreateGrpcService<ILopHocGrpcService>();
});

builder.Services.AddSingleton<IGiaoVienGrpcService>(serviceProvider =>
{
    var channel = serviceProvider.GetRequiredService<GrpcChannel>();
    return channel.CreateGrpcService<IGiaoVienGrpcService>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
