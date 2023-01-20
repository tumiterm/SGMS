using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGMS.Contract;
using SGMS.DTO;
using SGMS.Helper;
using SGMS.Models;

namespace SGMS.Controllers
{
    [Authorize(Roles = "Admin,Referee")]
    public class RefereeController : Controller
    {
        private readonly IUnitOfWork<Referee> _context;
        private readonly IUnitOfWork<Contact> _contactContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public RefereeController(IUnitOfWork<Referee> context,
                               IWebHostEnvironment hostEnvironment,
                               INotyfService notify)
        {

            _context = context;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

        }

        [HttpGet]
        public async Task<IActionResult> AddReferee()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>AddReferee(RefereeContact refereeContact)
        {

            Guid globalGuid = Utility.GenerateGuid();

            Referee referee = new Referee
            {
                RefereeId = globalGuid,

                CreatedBy = Utility.loggedInUser,

                CreatedOn = Utility.OnGetCurrentDateTime(),

                IDNumber = refereeContact.IDNumber,

                LastName = refereeContact.LastName,

                Name = refereeContact.Name,

                RefereeLevel = refereeContact.RefereeLevel,

                RefereeType = refereeContact.RefereeType,

                Gender = refereeContact.Gender,

                Title = refereeContact.Title,

                HasJoinedLFA = refereeContact.HasJoinedLFA,

                Contact = new Contact
                {
                    Alternative = refereeContact.Alternative,

                    ContactId = refereeContact.ContactId,

                    Email = refereeContact.Email,

                    Phone = refereeContact.Phone,

                }

            };

            refereeContact.RefereeId = globalGuid;

            if (referee != null)
            {
                if(!_context.DoesEntityExist<Referee>(m => m.IDNumber == refereeContact.IDNumber))
                {
                    if (ModelState.IsValid)
                    {
                        var refereeObj = await _context.OnItemCreationAsync(referee);

                        if (refereeObj != null)
                        {
                            int rc = await _context.ItemSaveAsync();

                            if (rc > 0)
                            {
                                _notify.Success("Referee successfully Added", 5);

                                _notify.Warning("...Please proceed to adding your attachments", 5);

                                return RedirectToAction("OnAddUserAttachment", "Attachment", new { AssociativeKey = refereeObj.RefereeId });
                            }
                            else
                            {
                                _notify.Error("Error: Referee cannot be Added");
                            }
                        }
                        else
                        {
                            _notify.Error("Error: Unable to save referee!");
                        }
                    }
                    else
                    {
                        _notify.Error("Error: All fields required!");
                    }
                }
                else
                {
                    _notify.Error($"Error: Referee with ID: {refereeContact.IDNumber}  already registered!", 5);
                }

                
            }

            return View();
        }

        public async Task<IActionResult> RegisteredRefs()
        {
            return View(await _context.OnLoadItemsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> OnViewRef(Guid RefereeId)
        {
            if(RefereeId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var refObj = await _context.OnLoadItemAsync(RefereeId);

            if (refObj is null)
            {
                return RedirectToAction("PageNotFound", "Global");

            }

            RefereeContact refereeContact = new RefereeContact
            {
                IDNumber = refObj.IDNumber,

                Experience = refObj.Experience,

                Gender = refObj.Gender,

                RefereeLevel = refObj.RefereeLevel,

                RefereeType = refObj.RefereeType,

                Name = refObj.Name,

                LastName = refObj.LastName,

                Title = refObj.Title,

                Photo = refObj.Photo,

            };

            if(refereeContact is null)
            {
                return NotFound();
            }

            return View(refereeContact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnViewRef(Referee referee)
        {
            referee.IsActive = true;

            referee.ModifiedBy = Utility.loggedInUser;

            referee.ModifiedOn = Utility.OnGetCurrentDateTime();

            if (ModelState.IsValid)
            {
                var refObj = await _context.OnModifyItemAsync(referee);

                if (refObj != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if (rc > 0)
                    {
                        _notify.Success("Referee saved successfully", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to save referee!!!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }
            }
            else
            {
                _notify.Error("Error: Please fill in all the fields!!!", 5);
            }

            return View(nameof(OnViewRef));
        }

    }


}
