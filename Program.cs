using cronjob;
using cronjob.Authentication;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using static Quartz.Logging.OperationName;


var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<Appdbcontext>(option =>
option.UseSqlServer(connectionString)
);



//builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("SendEmailJob");
    q.AddJob<MyJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("SendEmailJob-trigger")
        //This Cron interval can be described as "run every minute" (when second is zero)
        .WithCronSchedule("0 * * ? * *")
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


//namespace QuartzSampleApp
//{
//    public class Program
//    {
//        private static async Task Main(string[] args)
//        {
           
//            StdSchedulerFactory factory = new StdSchedulerFactory();
//            IScheduler scheduler = await factory.GetScheduler();

        
//            await scheduler.Start();

//            await Task.Delay(TimeSpan.FromSeconds(10));

          
//            await scheduler.Shutdown();
//        }
//    }
//}
//public class QuartzService
//{
//    public async Task StartAsync(CancellationToken cancellationToken)
//    {
      
//        var schedulerFactory = new StdSchedulerFactory();

//        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

 
//        await scheduler.Start(cancellationToken);
//    }
//}








