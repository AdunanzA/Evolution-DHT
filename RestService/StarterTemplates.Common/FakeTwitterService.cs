using System.Collections.Generic;
using System.Linq;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

//Entire implementation for the backend REST service of the TODO demo app
namespace PiattolerService.Backend
{
    // http://localhost:82/json/metadata?op=FakeTwit?profileImageUrl=http
    // http://localhost:82/FakeTwit?profileImageUrl=http&userid=me&text=ciaomondo
    // http://localhost:82/FakeTwit?profileImageUrl=http2&userid=me2&text=ciaomondo2
	
    //Register REST Paths
	[RestService("/faketwit")]
    [RestService("/faketwit/{userId}")]
	public class FakeTwit //REST Resource DTO
	{
        public string userId { get; set; }
        public string profileImageUrl { get; set; }
        public string text { get; set; }
        
        //public long Id { get; set; }
        //public string Content { get; set; }
        //public int Order { get; set; }
        //public bool Done { get; set; }
	}

	//Todo REST Service implementation
	public class FakeTwitService : RestServiceBase<FakeTwit>
	{
        public FakeTwitRepository Repository { get; set; }  //Injected by IOC

        public override object OnGet(FakeTwit request)
		{
            if (request.userId == default(string))
				return Repository.GetAll();

            return Repository.GetByuserId(request.userId);
		}

        //Called for new and update
        public override object OnPost(FakeTwit faketwit)
        {
            
            //char suca = Repository.Store(faketwit).userId.FirstOrDefault();
            object f = Repository.Store(faketwit);
            //return "suca";
            return Repository.Store(faketwit);
        }

        public override object OnDelete(FakeTwit request)
        {
            Repository.DeleteByuserId(request.userId);
            return null;
        }
	}


	/// <summary>
	/// In-memory repository, so we can run the TODO app without any dependencies
	/// Registered in Funq as a singleton, injected on every request
	/// </summary>
    public class FakeTwitRepository
	{
        private readonly List<FakeTwit> faketwits = new List<FakeTwit>();

        public FakeTwitRepository()
        {
            FakeTwit ft1 = new FakeTwit { userId = "Hammon", profileImageUrl = @"http://forum.adunanza.net/image.php?u=1&dateline=1294398092", text = "Il mondo e' mio!!" };
            FakeTwit ft2 = new FakeTwit { userId = "", profileImageUrl = @"http://forum.adunanza.net/image.php?u=39429&dateline=1216736785", text = "Aaah Ahhh..." };
            FakeTwit ft3 = new FakeTwit { userId = "3", profileImageUrl = @"http://forum.adunanza.net/image.php?u=38944&dateline=1297260296", text = "Chi va con lo zoppo va piano." };
            faketwits.Add(ft1);
            faketwits.Add(ft2);
            faketwits.Add(ft3);
        }

        public List<FakeTwit> GetAll()
		{
			return faketwits;
		}

        public FakeTwit GetByuserId(string userId)
		{
            return faketwits.FirstOrDefault(x => x.userId == userId);
		}

        public FakeTwit Store(FakeTwit faketwit)
        {
            if (faketwit.userId == default(string))
            {
                faketwit.userId = faketwit.userId;
            }
            else
            {
                for (var i = 0; i < faketwits.Count; i++)
                {
                    if (faketwits[i].userId != faketwit.userId) continue;

                    faketwits[i] = faketwit;
                    return faketwit;
                }
            }

            faketwits.Add(faketwit);
            return faketwit;
        }

		public void DeleteByuserId(string userid)
		{
            faketwits.RemoveAll(x => x.userId == userid);
		}
	}
}