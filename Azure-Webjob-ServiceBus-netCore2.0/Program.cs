 class Program
    {     
      static void Main(string[] args)
      {
        IServiceCollection serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var sbConfig = new ServiceBusConfiguration();
        sbConfig.MessageOptions.MaxConcurrentCalls = 1;

        var config = new JobHostConfiguration();
        config.JobActivator = new CustomJobActivator(serviceCollection.BuildServiceProvider()); // custom DI implementation
        config.UseServiceBus(sbConfig);           

        var host = new JobHost(config);
        host.RunAndBlock();

      }
      private static void ConfigureServices(IServiceCollection serviceCollection)
      {        
        // SQLAZURECONNSTR_DefaultConnection - is a Azure notification
        var defaultConnection = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_DefaultConnection", EnvironmentVariableTarget.Process);
        serviceCollection.AddDbContext<FDSContext>(sharedOptions => sharedOptions.UseSqlServer(defaultConnection));
        
        serviceCollection.AddScoped<IVistaOrderService, VistaOrderSerivce>();       
        serviceCollection.AddScoped<WebJobsMethods, WebJobsMethods>();

        // Set from configfile or read from Azure Application Settings
        // Environment.SetEnvironmentVariable("AzureWebJobsDashboard", configuration.GetConnectionString("WebJobsDashboard"));
        // Environment.SetEnvironmentVariable("AzureWebJobsStorage", configuration.GetConnectionString("WebJobsStorage"));
      }
