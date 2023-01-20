using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGMS.Contract;
using SGMS.DTO;
using SGMS.Helper;
using SGMS.Models;

namespace SGMS.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IUnitOfWork<Tournament> _context;
        private readonly IUnitOfWork<Sponsor> _sponsorContext;
        private readonly IUnitOfWork<Coach> _coachContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public TournamentController(IUnitOfWork<Tournament> context,
                               IUnitOfWork<Sponsor> sponsorContext,
                               IWebHostEnvironment hostEnvironment,
                               IUnitOfWork<Coach> coachContext,

        INotyfService notify)
        {
            _context = context;

            _sponsorContext = sponsorContext;

            _hostEnvironment = hostEnvironment;

            _coachContext = coachContext;

            _notify = notify;

        }

        [Authorize(Roles = "Admin,Coach,Referee")]
        [HttpGet]
        public IActionResult OnRegisterTournament()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OnGetTournaments()
        {
            var tournaments = await _context.OnLoadItemsAsync();

            return View(tournaments);
        }

        [Authorize(Roles = "Admin,Coach,Referee")]
        public async Task<IActionResult> OnRemoveTournament(Guid TournamentId)
        {
            var tournament = await _context.OnLoadItemAsync(TournamentId);

            if(tournament is null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var delObject = await _context.OnRemoveItemAsync(TournamentId);

            if(delObject > 0)
            {
                _notify.Success("Tournament successfully removed", 5);
            }
            else
            {
                _notify.Error("Error: Unable to delete tournament!!!", 5);
            }

            return RedirectToAction(nameof(OnGetTournaments));
        }


        [Authorize(Roles = "Admin,Coach,Referee")]
        [HttpGet]
        public async Task<IActionResult> OnModifyTournament(Guid TournamentId)
        {
            if(TournamentId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var tournament = await _context.OnLoadItemAsync(TournamentId);

            if(tournament is null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            TournamentSponsor tournamentSponsor = new TournamentSponsor
            {
                AffiliationFee = tournament.AffiliationFee,

                CutOffDate = tournament.CutOffDate,

                Cycle = tournament.Cycle,

                From = tournament.From,

                HasAffiliationFee = tournament.HasAffiliationFee,

                HasPrice = tournament.HasPrice,

                HasSponsor = tournament.HasSponsor,

                Till = tournament.Till,

                TournamentName = tournament.TournamentName,

                TournamentStatus = tournament.TournamentStatus,

                Type = tournament.Type,

                Price1 = tournament.Price1,

                Price2 = tournament.Price2,
            };

            return View(tournamentSponsor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnModifyTournament(Tournament tournament)
        {
            tournament.ModifiedBy = Utility.loggedInUser;

            tournament.ModifiedOn = Utility.OnGetCurrentDateTime();

            tournament.IsActive = true;

            if (ModelState.IsValid)
            {
                var tournamentObj = await _context.OnModifyItemAsync(tournament);

                if(tournamentObj != null)
                {
                    int rc = await  _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Tournament successfully saved", 5);

                        return RedirectToAction(nameof(OnGetTournaments));
                    }
                }
                else
                {
                    _notify.Error("Error: Unable to save tournament!!!",5);
                }
            }
            else
            {
                _notify.Error("Error: Something went wrong!",5);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnRegisterTournament(TournamentSponsor tournamentSponsor)
        {
            Tournament tournament;

            if (ModelState.IsValid)
            {
                Guid globalId = Utility.GenerateGuid();

                tournament = new Tournament
                {
                    IsActive = true,

                    From = tournamentSponsor.From,

                    Till = tournamentSponsor.Till,

                    HasPrice = tournamentSponsor.HasPrice,

                    Price1 = tournamentSponsor.Price1,

                    Price2 = tournamentSponsor.Price2,

                    CutOffDate = tournamentSponsor.CutOffDate,

                    CreatedBy = Utility.loggedInUser,

                    CreatedOn = Utility.OnGetCurrentDateTime(),

                    Cycle = tournamentSponsor.Cycle,

                    HasSponsor = tournamentSponsor.HasSponsor,

                    TournamentId = globalId,

                    TournamentName = tournamentSponsor.TournamentName,

                    TournamentStatus = tournamentSponsor.TournamentStatus,

                    Type = tournamentSponsor.Type,


                    AffiliationFee = tournamentSponsor.AffiliationFee,

                    HasAffiliationFee = tournamentSponsor.HasAffiliationFee,

                    Sponsor = new Sponsor
                    {
                        Email = tournamentSponsor.Email,

                        LastName = tournamentSponsor.LastName,

                        Name = tournamentSponsor.Name,

                        SponsorId = Utility.GenerateGuid(),

                        Phone = tournamentSponsor.Phone,

                        Title = tournamentSponsor.Title,

                        TournamentId = globalId
                    }

                };

                if (tournament != null)
                {
                    var saveTournament = await _context.OnItemCreationAsync(tournament);

                    if (saveTournament != null)
                    {
                        int rc = await _context.ItemSaveAsync();

                        if(rc > 0)
                        {
                            _notify.Success("Tournament Successfully Created", 5);

                            _notify.Warning("...Waiting to save Tournament sponsor...", 6);

                            return RedirectToAction(nameof(OnGetTournaments));

                        }
                        else
                        {
                            _notify.Error("Error: Unable to save tournament", 5);
                        }
                    }
                    else
                    {
                        _notify.Error("Error: Something went wrong!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }


                //if (tournament.HasSponsor)
                //{
                //    var sponsor = await _sponsorContext.OnItemCreationAsync(tournament.Sponsor);

                //    if(sponsor != null)
                //    {
                //        int rc = await _sponsorContext.ItemSaveAsync();

                //        if(rc > 0)
                //        {
                //            _notify.Success("Sponsor Information added successfully", 5);
                //        }
                //        else
                //        {
                //            _notify.Error("Error: Attempt to register sponsor failed", 5);
                //        }
                //    }
                //}
                //else
                //{
                //    _notify.Information("Notice: Tournament registered without sponsors", 5);
                //}
            }
            else
            {
                _notify.Error("Error: Please enter all fields!", 5);
            }

            return RedirectToAction(nameof(OnGetTournaments));
        }

    }
}
