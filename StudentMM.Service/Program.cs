using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using StudentMM.Common.AutoMapper;
using StudentMM.NHibernate.Mappings;
using StudentMM.Service.Services;

using StudentMM.NHibernate.UnitOfWork;
using ProtoBuf.Grpc.Server;
using StudentMM.Common.IServices;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISessionFactory>(provider =>
{
    return Fluently.Configure()
        .Database(MsSqlConfiguration.MsSql2012
            .ConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
            .ShowSql()
        )
        .Mappings(m =>
        {
            m.FluentMappings.AddFromAssemblyOf<SinhVienMap>();
            m.FluentMappings.AddFromAssemblyOf<LopHocMap>();
            m.FluentMappings.AddFromAssemblyOf<GiaoVienMap>();
        })
        .BuildSessionFactory();
});

// Inject ISession
builder.Services.AddScoped(factory =>
{
    var sessionFactory = factory.GetRequiredService<ISessionFactory>();
    return sessionFactory.OpenSession();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddScoped<IGiaoVienGrpcService, GiaoVienGrpcService>();
builder.Services.AddScoped<ILopHocGrpcService, LopHocGrpcService>();
builder.Services.AddScoped<ISinhVienGrpcService, SinhVienGrpcService>();

// Add services to the container.

builder.Services.AddGrpc();
builder.Services.AddCodeFirstGrpc();
builder.Services.AddGrpcReflection();
 
var app = builder.Build();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.MapGrpcService<SinhVienGrpcService>().EnableGrpcWeb();
app.MapGrpcService<GiaoVienGrpcService>().EnableGrpcWeb();
app.MapGrpcService<LopHocGrpcService>().EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
