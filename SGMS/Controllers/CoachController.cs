
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGMS.Contract;
using SGMS.Data;
using SGMS.DTO;
using SGMS.Helper;
using SGMS.Models;

namespace SGMS.Controllers
{
    [Authorize(Roles = "Admin,Coach,Referee")]
    public class CoachController : Controller
    {
        private readonly IUnitOfWork<Coach> _context;
        public INotyfService _notify { get; }

        private IWebHostEnvironment _hostEnvironment;
        public CoachController(IUnitOfWork<Coach> context,
                      IWebHostEnvironment hostEnvironment,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _notify = notify;
        }


        public async Task<IActionResult> RegisteredCoaches()
        {
              return View(await _context.OnLoadItemsAsync());
        }

        public IActionResult AddTeamCoach()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeamCoach(CoachContact coachContact)
        {
            Coach coach = new Coach
            {
                CoachId = Utility.GenerateGuid(),

                IDNumber = coachContact.IDNumber,

                IsActive = true,

                RSAIDCopyFile = coachContact.RSAIDCopyFile,

                RSAIDCopy = coachContact.RSAIDCopyFile.FileName,

                CreatedBy = Utility.loggedInUser,

                CreatedOn = Utility.OnGetCurrentDateTime(),

                LastName = coachContact.LastName,

                Name = coachContact.Name,

                Title = coachContact.Title,

                Contact = new Contact
                {
                    Alternative = coachContact.Alternative,

                    Email = coachContact.Email,

                    Phone = coachContact.Phone
                }
            };


            if (ModelState.IsValid)
            {
                if(coach != null)
                {
                    //coach.RSAIDCopy = coachContact.RSAIDCopyFile.FileName;

                    CoachIDUploader(coach);

                    var coachObj = await _context.OnItemCreationAsync(coach);

                    if(coachObj != null)
                    {
                        int rc = await _context.ItemSaveAsync();

                        if(rc > 0)
                        {
                            _notify.Success("Coach successfully saved", 5);
                            _notify.Warning("...waiting to add coach contact info", 6);
                            _notify.Success("Coach contact details saved successfully", 7);

                        }
                        else
                        {
                            _notify.Error("Error: Unable to save coach!", 5);
                        }
                    }
                    else
                    {
                        _notify.Error("Error: Something went wrong!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: Unable something went wrong!", 5);
                }
            }
            else
            {
                _notify.Error("Error: Please enter all fields", 5);
            }

            return RedirectToAction(nameof(RegisteredCoaches));
        }

        [HttpGet]
        public async Task<IActionResult> OnAddTeamCoach(Guid CoachId)
        {
            if(CoachId == Guid.Empty)
            {
                return NotFound();
            }

            var coach = await _context.OnLoadItemAsync(CoachId);

            CoachContact cContact = new CoachContact();

            cContact.CoachId = coach.CoachId;

            cContact.IDNumber = coach.IDNumber;

            cContact.LastName = coach.LastName;

            cContact.Name = coach.Name;

            cContact.Title = coach.Title;

            //cContact.ContactId = coach.Contact.ContactId;

            return View(cContact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnAddTeamCoach(CoachContact coachContact)
        {
            Coach coach = new Coach
            {
                IsActive = true,

                ModifiedBy = Utility.loggedInUser,

                ModifiedOn = Utility.OnGetCurrentDateTime(),

                Name = coachContact.Name,

                LastName = coachContact.LastName,

                Title = coachContact.Title,

                IDNumber = coachContact.IDNumber,

                RSAIDCopy = coachContact.RSAIDCopy
            };

            if (ModelState.IsValid)
            {
                var coachObj = await _context.OnModifyItemAsync(coach);

                if(coachObj != null) 
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Coach successfully saved",5);
                    }
                    else
                    {
                        _notify.Error("Error: Coach NOT saved !!!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error:Something went wrong !!!", 5);
                }
            }
            else
            {
                _notify.Error("Error:All input fields required !!!", 5);
            }

            return View(nameof(RegisteredCoaches));
        }

        public async Task<IActionResult> AttachmentDownload(string filename)
        {
            if (filename == null)

                return Content("Sorry NO Attachment found!!!");


            var path1 = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot");

            string folder = path1 + @"\CoachID\" + filename;

            var memory = new MemoryStream();

            using (var stream = new FileStream(folder, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, Utility.GetContentType(folder), Path.GetFileName(folder));
        }
        public async void CoachIDUploader(Coach coach)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(coach.RSAIDCopyFile.FileName);

            string extension = Path.GetExtension(coach.RSAIDCopyFile.FileName);

            coach.RSAIDCopy = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/CoachID/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await coach.RSAIDCopyFile.CopyToAsync(fileStream);
            }
        }

    }
}
