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
        public async Task<ActionResult> MatchModal()
        {
            try
            {
                var response = await HomeRepository.GetListOfPlayers();
                return PartialView(response);
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

        [HttpPost]
        public async Task<ActionResult> SetMatch(int playerOneId, int playerTwoId, int playerOneScore, int playerTwoScore)
        {
            try
            {
                var playerMatchInfo = new List<PlayerMatchInfo>();
                var one = new PlayerMatchInfo()
                {
                    PlayerId = playerOneId,
                    PlayerScore = playerOneScore
                };
                playerMatchInfo.Add(one);
                var two = new PlayerMatchInfo()
                {
                    PlayerId = playerTwoId,
                    PlayerScore = playerTwoScore
                };
                playerMatchInfo.Add(two);


                var response = await HomeRepository.SetMatch(playerMatchInfo);
                return Json(response, JsonRequestBehavior.AllowGet);
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
                return View(response);
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
