using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SGMS.Contract;
using SGMS.Helper;
using SGMS.Models;
using System.Dynamic;

namespace SGMS.Controllers
{
    public class SystemUserController : Controller
    {
        private readonly IUnitOfWork<SysUsers> _context;
        public INotyfService _notify { get; }
        public SystemUserController(IUnitOfWork<SysUsers> context,
                                    INotyfService notify)
        {
            _context = context;

            _notify = notify;
        }

        [HttpGet]
        public async Task<IActionResult> LogSystemUser()
        {
            var user = await _context.OnLoadItemsAsync();

            dynamic userObject = new ExpandoObject();

            userObject.UserList = user.ToList();

            return View(userObject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogSystemUser(SysUsers users)
        {
            users.IsActive = true;

            users.CreatedBy = Utility.loggedInUser;

            users.CreatedOn = Utility.OnGetCurrentDateTime();

            if (ModelState.IsValid)
            {
                users.Id = Utility.GenerateGuid();

                var user = await _context.OnItemCreationAsync(users);

                if(user != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("User added succesfully", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to add user!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }
            }
            else
            {
                _notify.Error("Error: All input fields are required!", 5);
            }

            return RedirectToAction(nameof(LogSystemUser));
        }


    }
}
