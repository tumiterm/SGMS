using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.DTO;
using SGMS.Helper;
using SGMS.Models;

namespace SGMS.Controllers
{
    [Authorize(Roles = "Admin,Coach,Referee")]
    public class PlayerSelectionController : Controller
    {
        private readonly IUnitOfWork<PlayerSelection> _context;
        private readonly IUnitOfWork<Team> _teamContext;
        private readonly IUnitOfWork<Player> _playerContext;
        private readonly IUnitOfWork<Match> _matchContext;
        private readonly IUnitOfWork<Tournament> _tournamentContext;



        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }

        dynamic dynamicObj = new ExpandoObject();
        public PlayerSelectionController(IUnitOfWork<PlayerSelection> context,

                               IWebHostEnvironment hostEnvironment,

                               IUnitOfWork<Team> teamContext,

                               IUnitOfWork<Player> playerContext,

                               IUnitOfWork<Match> matchContext,

                               IUnitOfWork<Tournament> tournamentContext,

                               INotyfService notify)
        {
            _context = context;

            _teamContext = teamContext;

            _playerContext = playerContext;

            _hostEnvironment = hostEnvironment;

            _matchContext = matchContext;

            _tournamentContext = tournamentContext;

            _notify = notify;
        }

        [HttpGet]
        public async Task<IActionResult> Fixtures()
        {
            List<Match> list = await _matchContext.OnLoadItemsAsync();

            SelectionDTO? selection = null;

            List<SelectionDTO> selectList = new List<SelectionDTO>();

            foreach (var item in list)
            {
                selection = new SelectionDTO
                {
                    Match = $"{await ConvertTeamId(item.FirstTeamId)} v/s {await ConvertTeamId(item.SecondTeamId)}",

                    GameDay = item.GameDay,

                    Venue = item.Venue
                };

                selectList.Add(selection);
            }

            return View(selectList);
        }

        [HttpGet]
        public async Task<IActionResult> PlayerSelection(Guid MatchId, Guid FirstTeamId, Guid SecondTeamId)
        {
            if (MatchId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var players = await _playerContext.OnLoadItemsAsync();

            var teamsList = await _teamContext.OnLoadItemsAsync();

            var match = await _matchContext.OnLoadItemAsync(MatchId);

            //get playing teams name

            var team1 = await _teamContext.OnLoadItemAsync(FirstTeamId);
            var team2 = await _teamContext.OnLoadItemAsync(SecondTeamId);

            IEnumerable<SelectListItem> getTeams = from s in teamsList

                                                   select new SelectListItem
                                                   {
                                                       Value = s.TeamId.ToString(),

                                                       Text = $"{s.TeamName}"
                                                   };



            IEnumerable<SelectListItem> getPlayers = from s in players

                                                     select new SelectListItem
                                                     {
                                                         Value = s.PlayerId.ToString(),

                                                         Text = $"{s.PlayerName} {s.PlayerLastName} | ({s.Position})"
                                                     };




            ViewBag.TeamId = new SelectList(getTeams, "Value", "Text");

            ViewData["Team"] = teamsList;

            ViewData["MatchId"] = MatchId;

            ViewData["FirstTeamId"] = FirstTeamId;

            ViewData["SecondTeamId"] = SecondTeamId;

            ViewBag.PLayerId = new SelectList(getPlayers, "Value", "Text");

            ViewData["MatchGames"] = $"{team1.TeamName} v/s {team2.TeamName}";

            var pSelection = await _context.OnLoadItemsAsync();

            var selectionList = from n in pSelection

                                where n.IsActive == true

                                select n;

            var playerCred = from c in players

                             where c.TeamId == FirstTeamId ||

                             c.TeamId == SecondTeamId

                             select c;

            var fPlayerList = playerCred.ToList();

            var pName = fPlayerList.Where(x => x.TeamId == FirstTeamId || x.TeamId == SecondTeamId).FirstOrDefault();

            var finSelection = selectionList.ToList();

            dynamicObj.Fixtures = finSelection;

            dynamicObj.TestName = "Test";

            return View(dynamicObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayerSelection(PlayerSelection playerSelection)
        {
            if (ModelState.IsValid)
            {
                playerSelection.Id = Utility.GenerateGuid();

                playerSelection.IsActive = true;

                playerSelection.CreatedBy = Utility.loggedInUser;

                playerSelection.CreatedOn = Utility.OnGetCurrentDateTime();

                var select = _context.OnItemCreationAsync(playerSelection);

                if(select != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Player succefully selected", 5);

                        return RedirectToAction("PlayerSelection", "PlayerSelection",

                            new

                            {
                                MatchId = playerSelection.MatchId,

                                FirstTeamId = playerSelection.FirstTeamId,

                                SecondTeamId = playerSelection.SecondTeamId

                            });
                    }
                    else
                    {
                        _notify.Error("Error: Unable to select player!",5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }
            }
            else
            {
                _notify.Error("Error: Please fill in all the fields", 5);
            }

            return View(playerSelection);
        }

        public async Task<IActionResult> RemoveSelection(Guid SelectionId)
        {

            if(SelectionId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var playerSelection = await _context.OnLoadItemAsync(SelectionId);
            
            if (playerSelection == null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var record = await _context.OnRemoveItemAsync(SelectionId);

            if(record > 0)
            {
                _notify.Success("Selection successfully removed", 5);
            }
            else
            {
                _notify.Error("Error: Unable to delete record", 5);
            }
            return View(nameof(RemoveDialog));
        }
        public IActionResult RemoveDialog()
        {
            return View();
        }

        private async Task<string> ConvertTeamId(Guid TeamId)
        {
            string rs = string.Empty;

            Team team = new Team();

            if(TeamId != Guid.Empty)
            {
                team = await _teamContext.OnLoadItemAsync(TeamId);

                rs = team.TeamName;
            }

            return rs;
        }
        private async Task<string> ConvertTournamentId(Guid TournamentId)
        {
            string rs = string.Empty;

            Tournament tournament = new Tournament();

            if (TournamentId != Guid.Empty)
            {
                tournament = await _tournamentContext.OnLoadItemAsync(TournamentId);

                rs = tournament.TournamentName;
            }

            return rs;
        }


    }


}
