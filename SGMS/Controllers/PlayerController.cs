using AspNetCore.Reporting;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGMS.Contract;
using SGMS.DTO;
using SGMS.Helper;
using SGMS.Models;

namespace SGMS.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IUnitOfWork<Player> _context;

        private readonly IUnitOfWork<Team> _teamContext;

        private readonly IUnitOfWork<Coach> _coachContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public PlayerController(IUnitOfWork<Player> context,
                               IWebHostEnvironment hostEnvironment,
                               IUnitOfWork<Team> teamContext,
                               IUnitOfWork<Coach> coachContext,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _teamContext = teamContext;

            _coachContext = coachContext;

            _notify = notify;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> OnGetAllPlayers()
        {
            return View(await _context.OnLoadItemsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> OnViewPlayerProfile(Guid PlayerId)
        {
            if(PlayerId == Guid.Empty)
            {
                return RedirectToAction("ResultsNotFound", "Global");
            }

            var player = await _context.OnLoadItemAsync(PlayerId);

            var team = await _teamContext.OnLoadItemAsync(player.TeamId);

            if(player is null)
            {
                return RedirectToAction("ResultsNotFound", "Global");

            }

            PlayerProfile playerProfile = new PlayerProfile
            {
                Player = new Player
                {
                    AlternativePhone = player.AlternativePhone,

                    CardNumber = player.CardNumber,

                    City = player.City,

                    CreatedBy = player.CreatedBy,

                    DOB = player.DOB,

                    Email = player.Email,

                    CreatedOn = player.CreatedOn,

                    IDNumber = player.IDNumber,

                    RSAIDCopy = player.RSAIDCopy,

                    Gender = player.Gender,

                    IsActive = player.IsActive,

                    JerseyNumber = player.JerseyNumber,

                    Photo = player.Photo,

                    Phone = player.Phone,

                    StreetName = player.StreetName,

                    PlayerId = PlayerId,

                    PlayerLastName = player.PlayerLastName,

                    PlayerName = player.PlayerName,

                    Position = player.Position,

                    PostalCode = player.PostalCode,

                    Province = player.Province,

                    TeamId = player.TeamId
                },

                Team = new Team
                {
                    TeamId = player.TeamId,

                    TeamName = team.TeamName,

                    TeamLogo = team.TeamLogo,
                    
                    Description = team.Description

                }
            };

            ViewData["PlayerName"] = $"{playerProfile.Player.PlayerName} {playerProfile.Player.PlayerLastName}" ;

            ViewData["IDNumber"] = playerProfile.Player.IDNumber;

            ViewData["Gender"] = playerProfile.Player.Gender;

            ViewData["Phone"] = playerProfile.Player.Phone;

            ViewData["AlternativePhone"] = playerProfile.Player.AlternativePhone;

            ViewData["Email"] = playerProfile.Player.Email;

            ViewData["Position"] = playerProfile.Player.Position;

            ViewData["JerseyNumber"] = playerProfile.Player.JerseyNumber;

            ViewData["StreetName"] = playerProfile.Player.StreetName;

            ViewData["City"] = playerProfile.Player.City;

            ViewData["Province"] = playerProfile.Player.Province;

            ViewData["PostCode"] = playerProfile.Player.PostalCode;

            ViewData["TeamName"] = playerProfile.Team.TeamName;

            ViewData["Description"] = playerProfile.Team.Description;



            return View(playerProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnViewPlayerProfile(PlayerProfile playerProfile)
        {
            if (ModelState.IsValid) 
            {
                

            }

            return View();
        }

        [Authorize(Roles = "Admin,Coach,Referee")]
        [HttpGet]
        public async Task<IActionResult> OnModifyPlayer(Guid PlayerId)
        {
            if(PlayerId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var teams = await _teamContext.OnLoadItemsAsync();

            IEnumerable<SelectListItem> getTeams = from s in teams

                                                   select new SelectListItem
                                                   {
                                                       Value = s.TeamId.ToString(),

                                                       Text = $"Team: {s.TeamName}"
                                                   };

            ViewBag.TeamId = new SelectList(getTeams, "Value", "Text");

            var player = await _context.OnLoadItemAsync(PlayerId);

            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnModifyPlayer(Player player)
        {
            player.IsActive = true;

            player.ModifiedBy = Utility.loggedInUser;

            player.ModifiedOn = Utility.OnGetCurrentDateTime();

            if (ModelState.IsValid)
            {
                var playerObj = await _context.OnModifyItemAsync(player);

                if(playerObj != null)
                {
                    _notify.Success("Player modified successfully", 5);
                }
                else
                {
                    _notify.Error("Error: Unable to save player!", 5);
                }
            }
            else
            {
                _notify.Error("Error: All fields are required!", 5);
            }

            return RedirectToAction(nameof(OnGetAllPlayers));
        }

        [Authorize]
        public async Task<IActionResult> AddPlayer()
        {

            _notify.Information("Player registrations are closed!", 5);

            var teams = await _teamContext.OnLoadItemsAsync();

            IEnumerable<SelectListItem> getTeams = from s in teams

                                                   select new SelectListItem
                                                   {
                                                       Value = s.TeamId.ToString(),

                                                       Text = $"Team: {s.TeamName}"
                                                   };

            ViewBag.TeamId = new SelectList(getTeams, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlayer(Player player)
        {
            string teamName = string.Empty;

            player.PlayerId = Utility.GenerateGuid();

            player.IsActive = true;

            player.CreatedBy = Utility.loggedInUser;

            player.CreatedOn = Utility.OnGetCurrentDateTime();

            player.Photo = player.PhotoFile.FileName;

            player.RSAIDCopy = player.RSAIDCopyFile.FileName;

            var playerTeam = await _teamContext.OnLoadItemAsync(player.TeamId);

            if(playerTeam is null)
            {
                teamName = "N/A";
            }

            if (!_context.DoesEntityExist<Player>(x => x.IDNumber == player.IDNumber))
            {
                if (ModelState.IsValid)
                {
                    player.PlayerId = Utility.GenerateGuid();

                    player.IsActive = true;

                    player.CardNumber = "000 000 0000";

                    player.CreatedOn = Utility.OnGetCurrentDateTime();

                    player.CreatedBy = Utility.loggedInUser;

                    PlayerPhotoUploader(player);

                    PlayerIDUploader(player);

                    var playerObj = _context.OnItemCreationAsync(player);

                    if (playerObj != null)
                    {
                        int rc = await _context.ItemSaveAsync();

                        if (rc > 0)
                        {
                            _notify.Success("Player Successfully Added", 5);

                            //Utility.SendSMS($"Dear Mr D Samkethe this is to notify you that {player.PlayerLastName} {player.PlayerName} has been registered on the system as a soccer player. He is registered under the team: {playerTeam.TeamName}"
                            //                ,Utility.contact);

                            var team = await _teamContext.OnLoadItemAsync(player.TeamId);

                            Utility.OnSendMailNotification(player.Email, "Player Registration",
                                                           Utility.OnConfirmationMessage($"{player.PlayerLastName} {player.PlayerName}",team.TeamName,player.IDNumber,player.Position.ToString()),"Forek Kasi Games 2022");

                            return RedirectToAction(nameof(PlayerConfirmation), new { PlayerId = player.PlayerId });
                        }
                        else
                        {
                            _notify.Error("Error: Unable to Add Player", 5);
                        }
                    }
                    else
                    {
                        _notify.Error("Error:Something went wrong!", 5);
                    }
                }
                else

                    _notify.Error("Error:All fields required!", 5);
                {
                }
            }
            else
            {
                _notify.Error("Error: Player already exists!", 5);
            }
    
            return View(player);
        }
        public async Task<IActionResult> PlayerConfirmation(Guid PlayerId)
        {
            var player = await _context.OnLoadItemAsync(PlayerId);
 
            if(player is null)
            {
                return NotFound();
            }

            var team = await _teamContext.OnLoadItemAsync(player.TeamId);

            if(team is null)
            {
                return NotFound();
            }


            ViewData["pName"] = $"{player.PlayerName} {player.PlayerLastName}" ;
            ViewData["pDOB"] = player.DOB;
            ViewData["pID"] = player.IDNumber;
            ViewData["pGender"] = player.Gender;
            ViewData["pPosition"] = player.Position;
            ViewData["JerseyNumber"] = player.JerseyNumber;

            ViewData["Phone"] = player.Phone;
            ViewData["AlternativePhone"] = player.AlternativePhone;
            ViewData["Email"] = player.Email;

            ViewData["StreetName"] = player.StreetName;
            ViewData["City"] = player.City;
            ViewData["Province"] = player.Province;
            ViewData["PostalCode"] = player.PostalCode;

            ViewData["Team"] = team.TeamName;

            return View();
        }
        public async void PlayerPhotoUploader(Player player)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(player.PhotoFile.FileName);

            string extension = Path.GetExtension(player.PhotoFile.FileName);

            player.Photo = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/PlayerPhoto/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await player.PhotoFile.CopyToAsync(fileStream);
            }
        }
        public async void PlayerIDUploader(Player player)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(player.RSAIDCopyFile.FileName);

            string extension = Path.GetExtension(player.RSAIDCopyFile.FileName);

            player.RSAIDCopy = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/PlayerID/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await player.RSAIDCopyFile.CopyToAsync(fileStream);
            }
        }
        public async Task<IActionResult> AttachmentDownload(string filename)
        {
            if (filename == null)

                return Content("Sorry NO Attachment found!!!");


            var path1 = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot");

            string folder = path1 + @"\PlayerID\" + filename;

            var memory = new MemoryStream();

            using (var stream = new FileStream(folder, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, Utility.GetContentType(folder), Path.GetFileName(folder));
        }
        public async Task<IActionResult> OnPlayerRemoval(Guid PlayerId)
        {
            if (PlayerId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var player = await _context.OnLoadItemAsync(PlayerId);

            if (player== null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var record = await _context.OnRemoveItemAsync(PlayerId);

            if (record > 0)
            {
                _notify.Success("Player successfully removed", 5);
            }
            else
            {
                _notify.Error("Error: Unable to delete record", 5);
            }
            return RedirectToAction("OnGetAllPlayers", "Player");
        }
        public async Task<IActionResult> DownloadReport()
        {
            string mimeType = "";

            string path  = $"{_hostEnvironment.WebRootPath}/Report/SGMS.rdlc";

            int extension = 1;

            List<Player> players = await _context.OnLoadItemsAsync();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("param1", "Soccer Games Report");

            parameters.Add("param2", Utility.OnGetCurrentDateTime());

            parameters.Add("param3", "All Teams");

            LocalReport localReport = new LocalReport(path);

            localReport.AddDataSource("ForekDS", players);

            var res = localReport.Execute(RenderType.Pdf, extension, parameters,mimeType);

            return File(res.MainStream, "application/pdf");

        }

    }

}
