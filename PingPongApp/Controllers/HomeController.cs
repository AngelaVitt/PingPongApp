/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using PingPongApp.Processors;
using PingPongApp.Repository;

namespace PingPongApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";
            var response = await AppManagerProcessor.ProcessContact();
            return View();
        }

        public async Task<ActionResult> Players()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";
            var response = await AppManagerProcessor.ProcessContact();
            return View();
        }

        public async Task<ActionResult> PlayerDetails()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";
            var response = await AppManagerProcessor.ProcessContact();
            return View();
        }

        public async Task<ActionResult> Edit()
        {
            try
            {
                //var dfsd = await ContentRepository.AddContactToDB();
                return View();
            }
            catch
            {
                return View();
            }
        }


    }

}
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using PingPongApp.Repository;
using PingPongApp.Models;

namespace PingPongApp.Controllers
{
    public class HomeController : Controller
    {
        /*private ISAHome _saHome;
        public HomeController() { }
        public HomeController(ISAHome saHome)
        {
            _saHome = saHome;
        }*/
        public ActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> Players()
        {
            try
            {
                var response = await HomeRepository.GetListOfPlayers();
                return View(response);
            }
            catch
            {
                //return View();
            }
            return View();
        }
        public async Task<ActionResult> Matches()
        {
            try
            {
                var response = await HomeRepository.GetListOfMatches();
                return View(response);
            }
            catch
            {
                //return View();
            }
            return View();
        }
        public async Task<ActionResult> SetMatch(/*List<PlayerMatchInfo> playerMatchInfo*/)
        {
            try
            {
                var playerMatchInfo = new List<PlayerMatchInfo>();
                var one = new PlayerMatchInfo()
                {
                    PlayerId = 1,
                    PlayerScore = 99
                };
                playerMatchInfo.Add(one);
                var two = new PlayerMatchInfo()
                {
                    PlayerId = 2,
                    PlayerScore = 100
                };
                playerMatchInfo.Add(two);


                var response = await HomeRepository.SetMatch(playerMatchInfo);
                //return View();
            }
            catch
            {
                //return View();
            }
            return View();
        }
        public async Task<ActionResult> GetPlayerMatchesInfo()
        {
            try
            {
                var response = await HomeRepository.GetPlayerMatchesInfo(1);
                //return View();
            }
            catch
            {
                //return View();
            }
            return View();
        }
        public async Task<ActionResult> PlayerInfo(int PlayerID)
        {
            try
            {
                var response = await HomeRepository.GetPlayerMatchesInfo(PlayerID);
                return View(response);
            }
            catch
            {
                //return View();
            }
            return View();
        }

    }

}
