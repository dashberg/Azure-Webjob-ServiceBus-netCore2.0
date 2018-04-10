// shout out to https://matt-roberts.me/azure-webjobs-in-net-core-2-with-di-and-configuration/

public class CustomJobActivator : IJobActivator
  {
    private readonly IServiceProvider _service;
    public CustomJobActivator(IServiceProvider service)
    {
      _service = service;
    }


    public T CreateInstance<T>()
    {
      var service = _service.GetService<T>();
      return service;
    }
