using ServiceStack.Configuration;
using ServiceStack.WebHost.Endpoints;

namespace PiattolerService.Backend
{
	//HttpListener Hosts
	public class PiattolerServiceAppListenerHost
		: AppHostHttpListenerBase
	{
		static readonly ConfigurationResourceManager AppSettings = new ConfigurationResourceManager();

        //public PiattolerServiceAppListenerHost()
        //    : base(AppSettings.GetString("ServiceName") ?? "PiattolerService HttpListener", typeof(TodoService).Assembly) { }
        public PiattolerServiceAppListenerHost()
            : base(AppSettings.GetString("ServiceName") ?? "PiattolerService HttpListener", typeof(FakeTwitService).Assembly) { }

		public override void Configure(Funq.Container container)
		{
			//container.Register(new TodoRepository());
            container.Register(new FakeTwitRepository());
		}
	}
}
