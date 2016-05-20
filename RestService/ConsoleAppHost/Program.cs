using System;
using ServiceStack.Configuration;
using ServiceStack.WebHost.Endpoints;
using PiattolerService.Backend;
#if MONO_REL
using Mono.Unix;
using Mono.Unix.Native;
#endif

namespace PiattolerConsoleServer
{
	class Program
	{
		private static readonly string ListeningOn = ConfigUtils.GetAppSetting("ListeningOn");

		//HttpListener Hosts
		public class AppHost
			: AppHostHttpListenerBase
		{
			public AppHost()
                : base("PiattolerService HttpListener", typeof(FakeTwit).Assembly) { }

			public override void Configure(Funq.Container container)
			{
				container.Register(new FakeTwitRepository());
			}
		}

		static void Main(string[] args)
		{
			var appHost = new AppHost();
			appHost.Init();
			appHost.Start(ListeningOn);

#if MONO_REL
            // For Linux
            UnixSignal[] signals = new UnixSignal[] { 
				new UnixSignal(Signum.SIGINT), 
				new UnixSignal(Signum.SIGTERM), 
			};

            // Wait for a unix signal
            for (bool exit = false; !exit; )
            {
                int id = UnixSignal.WaitAny(signals);

                if (id >= 0 && id < signals.Length)
                {
                    if (signals[id].IsSet) exit = true;
                }
            }
#else
			Console.WriteLine("Started listening on: " + ListeningOn);

			Console.WriteLine("Service Server Created at {0}, listening on {1}",
				DateTime.Now, ListeningOn);


			Console.WriteLine("ReadKey()");
			Console.ReadKey();
#endif
		}
	}
}
