using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Helper;
using SGMS.Models;

namespace SGMS.Controllers
{
    public class MatchController : Controller
    {
        private readonly IUnitOfWork<Match> _context;
        private readonly IUnitOfWork<Referee> _refContext;
        private readonly IUnitOfWork<Player> _playerContext;
        private readonly IUnitOfWork<Tournament> _tournContext;
        private readonly IUnitOfWork<Team> _teamContext;


        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public MatchController(IUnitOfWork<Match> context,

                                IUnitOfWork<Referee> refContext,

                                IUnitOfWork<Tournament> tournContext,

                                IUnitOfWork<Player> playerContext,

                                IUnitOfWork<Team> teamContext,

                                IWebHostEnvironment hostEnvironment,

                                INotyfService notify)
        {
            _context = context;

            _refContext = refContext;

            _playerContext = playerContext;

            _hostEnvironment = hostEnvironment;

            _tournContext = tournContext;

            _teamContext = teamContext;

            _notify = notify;
        }

        [Authorize]
        public async Task<IActionResult>MatchList()
        {
              return View(await _context.OnLoadItemsAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            //if (id == null || _context.Matches == null)
            //{
            //    return NotFound();
            //}

            //var match = await _context.Matches
            //    .FirstOrDefaultAsync(m => m.MatchId == id);
            //if (match == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        [Authorize(Roles = "Admin, Referee")]
        public async Task<IActionResult> ScheduleMatch()
        {

            var refereeList = await _refContext.OnLoadItemsAsync();

            var players = await _playerContext.OnLoadItemsAsync();

            var tournaments = await _tournContext.OnLoadItemsAsync();

            var teams = await _teamContext.OnLoadItemsAsync();


            var teamFilter = from n in teams

                            where n.IsActive == true

                            select n;

            var onGetActiveTeams = teamFilter.ToList();


            var refFilter = from n in refereeList

                            where n.IsActive == true

                            select n;

            var onGetActiveRefs = refFilter.ToList();

            var PlayerFilter = from n in players

                            where n.IsActive == true

                            select n;

            var onGetActivePlayers = PlayerFilter.ToList();


            IEnumerable<SelectListItem> referees = from s in refereeList

                                                   select new SelectListItem
                                                      {
                                                          Value = s.RefereeId.ToString(),

                                                          Text = $"Ref: {s.Title} {s.LastName} {s.Name}"
                                                      };


            IEnumerable<SelectListItem> getPlayers = from s in onGetActivePlayers

                                                   select new SelectListItem
                                                   {
                                                       Value = s.PlayerId.ToString(),

                                                       Text = $" {s.PlayerName} {s.PlayerLastName} [{s.Position}]"
                                                   };

            IEnumerable<SelectListItem> getTournaments = from s in tournaments

                                                         select new SelectListItem
                                                     {
                                                         Value = s.TournamentId.ToString(),

                                                         Text = $"Tournament: {s.TournamentName} [{s.Type}]"
                                                     };

            IEnumerable<SelectListItem> getTeams = from s in onGetActiveTeams

                                                         select new SelectListItem
                                                         {
                                                             Value = s.TeamId.ToString(),

                                                             Text = $" {s.TeamName}"
                                                         };


            ViewBag.RefereeId = new SelectList(referees, "Value", "Text");

            ViewBag.TeamId = new SelectList(getTeams, "Value", "Text");

            ViewBag.PlayerId = new SelectList(getPlayers, "Value", "Text");

            ViewBag.TournamentId = new SelectList(getTournaments, "Value", "Text");


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleMatch(Match match)
        {
            match.MatchId = Utility.GenerateGuid();

            match.CreatedBy = Utility.loggedInUser;

            match.CreatedOn = Utility.OnGetCurrentDateTime();

            match.IsActive = true;

            if (ModelState.IsValid)
            {
                var matchObj = await _context.OnItemCreationAsync(match);

                if(matchObj != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Match successfully scheduled", 5);

                        return RedirectToAction("PlayerSelection", "PlayerSelection",

                            new { 

                                MatchId = match.MatchId,

                                FirstTeamId = match.FirstTeamId,

                                SecondTeamId = match.SecondTeamId,

                               });
                    }
                    else
                    {
                        _notify.Error("Error: Unable to schedule match!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }

            }
            else
            {
                _notify.Error("Error: Please enter all field(s)", 5);
            }
            return View(match);
        }

        [Authorize(Roles = "Admin, Referee")]
        public async Task<IActionResult> OnScheduleMatch(Guid MatchId)
        {
            if (MatchId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var match = await _context.OnLoadItemAsync(MatchId);

            if (match == null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }
            return View(match);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnScheduleMatch(Guid id, Match match)
        {
            if (id != match.MatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                //    _context.Update(match);
                //    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!MatchExists(match.MatchId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

       
    }
}
