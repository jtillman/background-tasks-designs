﻿using DefinedCaller.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueueT;
using QueueT.Tasks;

namespace DefinedCaller
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<OrderDbContext>(options => {
                options.UseInMemoryDatabase("OrderDb", memOptions => { });
            });

            services.AddScoped<IOrderService, OrderService>();

            var defaultQueueName = "default";
            services.AddQueueT(options =>
            {
                options.AddQueues(defaultQueueName, OrderService.QueueName);
                options.DefaultQueueName = defaultQueueName;
                options.Broker = new QueueT.Brokers.InMemoryBroker(defaultQueueName, OrderService.QueueName);
            })
            .UseTasks(options =>
            {
                options.RegisterTaskAttibutes(System.Reflection.Assembly.GetEntryAssembly());
            });

            services.AddQueueTWorker();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
