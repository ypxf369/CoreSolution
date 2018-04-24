using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CoreSolution.AutoMapper.Configuration;
using CoreSolution.AutoMapper.Startup;
using CoreSolution.IService.Convention;
using CoreSolution.WebApi.Interceptor;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreSolution.WebApi
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Logger = LogManager.CreateRepository("CoreSolutionRepository");
            XmlConfigurator.Configure(Logger, new FileInfo("log4net.config"));
        }

        public static ILoggerRepository Logger { get; set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddAutoMapper();
            services.AddMvc(options =>
            {
                options.Filters.Add<SysExceptionFilter>();//添加异常过滤器
                options.Filters.Add<AuditingFilter>();//审计日志过滤器
            });

            //配置跨域处理
            //string[] urls = Configuration.GetSection("AllowCors:AllowOrigins").Value.Split(",");
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", i =>
                {
                    //i.WithOrigins(urls)//允许指定主机列表访问
                    i.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();//允许处理cookie
                });
                //options.AddPolicy("AllowOrigins", i =>
                //{
                //    i.WithOrigins(urls)
                //    .AllowAnyHeader()
                //    .AllowAnyMethod()
                //    .AllowCredentials();
                //});
            });

            //配置Swagger
            services.AddSwaggerGen(i =>
            {
                i.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "CoreSolution WebApi接口文档",
                    Description = "CoreSolution Test",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "yepeng", Email = "ypxf369@163.com", Url = "http://www.baidu.com" }
                });
                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "CoreSolution.WebApi.xml");
                i.IncludeXmlComments(xmlPath);
            });

            var builder = new ContainerBuilder();//实例化Autofac容器
            builder.Populate(services);

            var assemblys = Assembly.Load("CoreSolution.Service");
            var baseType = typeof(IServiceSupport);

            builder.RegisterAssemblyTypes(assemblys)
                .Where(i => baseType.IsAssignableFrom(i) && i != baseType)
                .AsImplementedInterfaces();
            var container = builder.Build();
            return new AutofacServiceProvider(container);//让Autofac接管core内置DI容器
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            AutoMapperStartup.Register();//加载AutoMapper配置项
            app.UseMvc();

            //使用跨域
            app.UseCors("AllowAllOrigin");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(i =>
            {
                i.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreSolution API V1");
                i.ShowExtensions();
            });
        }
    }
}
