
using AspNetCore.Reporting;
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
    public class TeamController : Controller
    {
        private readonly IUnitOfWork<Team> _teamContext;
        private readonly IUnitOfWork<Player> _playerContext;
        private readonly IUnitOfWork<Coach> _coachContext;
        private readonly IUnitOfWork<District> _districtContext;
        private readonly IUnitOfWork<Municipality> _muniContext;


        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public TeamController(IUnitOfWork<Team> teamContext, IWebHostEnvironment hostEnvironment,
                              IUnitOfWork<Coach> coachContext,
                              IUnitOfWork<Player> playerContext,
                              IUnitOfWork<District> districtContext,
                              IUnitOfWork<Municipality> muniContext,
                              INotyfService notify)
        {
            _teamContext = teamContext;

            _hostEnvironment = hostEnvironment;

            _coachContext = coachContext;

            _districtContext = districtContext;

            _muniContext = muniContext;

            _playerContext = playerContext;

            _notify = notify;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _teamContext.OnLoadItemsAsync());
        }

        public async Task<IActionResult> OnAddTeam()
        {

            _notify.Information("Team registrations are closed!", 5);

            var districtList = await _districtContext.OnLoadItemsAsync();

            var muniList = await _muniContext.OnLoadItemsAsync();

            IEnumerable<SelectListItem> municipalities = from m in muniList

                                                   select new SelectListItem
                                                   {
                                                       Value = m.MunicipalityId.ToString(),

                                                       Text = $"{m.MunicipalityName} Municipality"
                                                   };

            IEnumerable<SelectListItem> districts = from m in districtList

                                                         select new SelectListItem
                                                         {
                                                             Value = m.DistrictId.ToString(),

                                                             Text = $"District: {m.DistrictName}"
                                                         };

            ViewBag.MunicipalityId = new SelectList(municipalities, "Value", "Text");

            ViewBag.DistrictId = new SelectList(districts, "Value", "Text");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnAddTeam(Team team)
        {
            if (ModelState.IsValid)
            {
                team.TeamId = Utility.GenerateGuid();

                team.CreatedOn = Utility.OnGetCurrentDateTime();

                team.CreatedBy = Utility.loggedInUser;

                team.IsActive = true;

                team.TeamLogo = team.TeamLogoFile.FileName;

                var teamObj = await _teamContext.OnItemCreationAsync(team);

                if(teamObj != null)
                {

                    TeamLogoUploader(team);

                    int rc = await _teamContext.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Team successfully added", 5);

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notify.Error("Error: Unable to save team", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: something went wrong!", 5);
                }

                return View();
            }
            else
            {
                _notify.Error("Error: Please enter all fields", 5);
            }

            return View(nameof(OnTeamAddition),new {TeamId = team.TeamId});
        }

        [HttpGet]
        public async Task<IActionResult> OnTeamAddition(Guid TeamId)
        {
            if (TeamId == Guid.Empty)
            {
                return NotFound();
            }

            var team = await _teamContext.OnLoadItemAsync(TeamId);

            var districtList = await _districtContext.OnLoadItemsAsync();

            var muniList = await _muniContext.OnLoadItemsAsync();

            IEnumerable<SelectListItem> municipalities = from m in muniList

                                                         select new SelectListItem
                                                         {
                                                             Value = m.MunicipalityId.ToString(),

                                                             Text = $"{m.MunicipalityName} Municipality"
                                                         };

            IEnumerable<SelectListItem> districts = from m in districtList

                                                    select new SelectListItem
                                                    {
                                                        Value = m.DistrictId.ToString(),

                                                        Text = $"District: {m.DistrictName}"
                                                    };

            ViewBag.MunicipalityId = new SelectList(municipalities, "Value", "Text");

            ViewBag.DistrictId = new SelectList(districts, "Value", "Text");

            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnTeamAddition(Guid TeamId, Team team)
        {
            if (TeamId != team.TeamId)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            team.IsActive = true;

            team.ModifiedBy = Utility.loggedInUser;

            team.ModifiedOn = Utility.OnGetCurrentDateTime();

            var getTeam = await _teamContext.OnLoadItemAsync(TeamId);

            if (getTeam is null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var teamObj = await _teamContext.OnModifyItemAsync(team);

                    if(teamObj != null)
                    {
                        _notify.Success("Team successfully saved", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to save team!", 5);

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_teamContext.DoesEntityExist<Team>(t => t.TeamId == TeamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }
        public async void TeamLogoUploader(Team team)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(team.TeamLogoFile.FileName);

            string extension = Path.GetExtension(team.TeamLogoFile.FileName);

            team.TeamLogo = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/Logo/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await team.TeamLogoFile.CopyToAsync(fileStream);
            }
        }
        public async Task<IActionResult> OnViewTeamPlayers(Guid TeamId)
        {

            if (TeamId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            Team team = await _teamContext.OnLoadItemAsync(TeamId);

            List<Player> playerList = await _playerContext.OnLoadItemsAsync();

            var filterPlayer = playerList.Where(m => m.TeamId == team.TeamId).ToList();

            if(team is null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            ViewData["Team"] = $"Team: {team.TeamName}";

            ViewData["List"] = filterPlayer;

            return View(team);
        }
        public async Task<IActionResult> DownloadReport(Guid TeamId)
        {
            string mimeType = "";

            string path = $"{_hostEnvironment.WebRootPath}/Report/SGMS.rdlc";

            int extension = 1;

            List<Player> players = await _playerContext.OnLoadItemsAsync();

            var filterPlayers = players.Where(x => x.TeamId == TeamId).ToList();

            var team = await _teamContext.OnLoadItemAsync(TeamId);

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("param1", "Soccer Games Report");

            parameters.Add("param2", Utility.OnGetCurrentDateTime());

            parameters.Add("param3", team.TeamName);

            parameters.Add("param4", team.CreatedBy);

            LocalReport localReport = new LocalReport(path);

            localReport.AddDataSource("ForekDS", filterPlayers);

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);

            return File(res.MainStream, "application/pdf");

        }



    }
}
