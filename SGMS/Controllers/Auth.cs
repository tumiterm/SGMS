using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SGMS.Contract;
using SGMS.DTO;
using SGMS.Helper;
using SGMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace SGMS.Controllers
{
    public class AuthController : Controller
    {

        private readonly IUnitOfWork<User> _context;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public AuthController(IUnitOfWork<User> context,
                               IWebHostEnvironment hostEnvironment,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RetrieveUsers()
        {
            return View(await _context.OnLoadItemsAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> OnViewUserInfo(Guid Id)
        {
            if(Id == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            User user = await _context.OnLoadItemAsync(Id);

            if(user is null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            ViewData["Password"] = user.Password;

            ViewData["CPassword"] = user.ConfirmPassword;

            ViewData["user"] = $"{user.Name} {user.LastName}";

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnViewUserInfo(User user)
        {
            user.IsActive = true;

            user.LastLoginDate = Utility.OnGetCurrentDateTime();

            user.ModifiedBy = Utility.loggedInUser;

            user.ModifiedOn = Utility.OnGetCurrentDateTime();

            if (ModelState.IsValid)
            {
                var userObj = await _context.OnModifyItemAsync(user);

                if(userObj != null)
                {
                    _notify.Success("User details successfully saved", 5);
                }
                else
                {
                    _notify.Error("Error: Unable to add user!", 5); ;
                }  
            }
            else
            {
                _notify.Error("Error: Please fill in all the fields!",5);
            }

            return RedirectToAction(nameof(RetrieveUsers));
        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        public async Task<IActionResult> SignUp(User user)
        {
            bool Status = false;

            string message = "";

            #region Generate Activation Code 

            string time = DateTime.Now.ToString("hh:mm tt");

            string date = DateTime.Now.ToString("dddd, dd MMMM yyyy") + " " + time;

            user.ActivationCode = Utility.GenerateGuid();

            user.LastLoginDate = date;

            user.ResetPasswordCode = "";

            user.Id = Utility.GenerateGuid();

            #endregion

            #region  Password Hashing 

            user.Password = Utility.ValueEncryption(user.Password);

            user.ConfirmPassword = Utility.ValueEncryption(user.ConfirmPassword);

            #endregion

            user.IsEmailVerified = false;

            user.IsActive = true;

            user.CreatedOn = Utility.OnGetCurrentDateTime();

            user.ResetPasswordCode = "";

            user.Role = Enums.eSysRole.None;

            
            if (ModelState.IsValid)
            {
                #region Save to Database

                try
                {
                    if (!_context.DoesEntityExist<User>(m => m.Username == user.Username))
                    {
                        var userAddition = _context.OnItemCreationAsync(user);

                        if (userAddition != null)
                        {
                            int rc = await _context.ItemSaveAsync();

                            if (rc > 0)
                            {
                                ViewData["successMessage"] = $"User Successfully Registered";

                            }
                        }

                        //SendVerificationLinkEmail(user.Username, user.ActivationCode.ToString(), "", "VerifyAccount");

                        //message = " Registration successful. Account activation link " +

                        //    " has been sent to your email: " + user.Username + "\nKindly click on the link to activate your account.\n" +

                        //    " Regards";

                        Status = true;

                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                #endregion
            }
            else
            {
                message = "Registration Successful";
            }

            ViewBag.Message = message;

            ViewBag.Status = Status;

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserViewModel user, string? ReturnUrl = "")
        {
            string message = "";

            if (!String.IsNullOrEmpty(user.EmailID))
            {
                if (!String.IsNullOrEmpty(user.Password))
                {
                    if (ModelState.IsValid)
                    {

                        var getUsers = await _context.OnLoadItemsAsync();

                        var filterUsers = from n in getUsers

                                          where n.Username == user.EmailID &&

                                          n.Password == Utility.ValueEncryption(user.Password) &&

                                          n.IsActive == true

                                          select n;

                        var userInfo = filterUsers.FirstOrDefault();

                        if (userInfo != null)
                        {
                            var claims = new List<Claim>()
                            {
                               new Claim(ClaimTypes.NameIdentifier,userInfo.ToString()),
                               new Claim(ClaimTypes.Name, userInfo.Name),
                               new Claim(ClaimTypes.Surname, userInfo.LastName),
                               new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                               new Claim(ClaimTypes.Role, userInfo.Role.ToString()),
                               new Claim("SGMSAppCookie","Code")
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var principal = new ClaimsPrincipal(identity);

                            var currentUserName = identity.FindFirst(ClaimTypes.Name);

                            var currentUserSurname = identity.FindFirst(ClaimTypes.Surname);

                            var currentUserId = identity.FindFirst(ClaimTypes.NameIdentifier);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                            {
                                IsPersistent = user.RememberMe

                            });

                            //if (!userInfo.IsEmailVerified)
                            //{
                            //    ViewBag.Message = "Please verify your email first";

                            //    _notify.Error("email not verified");

                            //    return View();

                            //}


                            if (string.Compare(Utility.ValueEncryption(user.Password), userInfo.Password) == 0)
                            {
                                int timeout = user.RememberMe ? 525600 : 20;

                                if (Url.IsLocalUrl(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }
                                else
                                {

                                    Utility.loggedInUser = $"{currentUserName.Value} {currentUserSurname.Value}";

                                    userInfo.LastLoginDate = DateTime.Now.ToString();

                                    if (userInfo.Role == Enums.eSysRole.None)
                                    {
                                        //Utility.SendSMS($"Dear Mr Baloyi, User {Utility.loggedInUser} tried to login to the our soccer system BUT was denied! Please allocate them a role",
                                        //                "0683965711");

                                        return RedirectToAction("AccessDenied", "Global");

                                    }
                                    else if (userInfo.Role == Enums.eSysRole.Coach)
                                    {
                                        return RedirectToAction("Index", "Home");

                                    }
                                    else if (userInfo.Role == Enums.eSysRole.Player)
                                    {
                                        return RedirectToAction("Index", "Home");

                                    }
                                    else if (userInfo.Role == Enums.eSysRole.Admin)
                                    {
                                        return RedirectToAction("Index", "Home");

                                    }
                                    else if (userInfo.Role == Enums.eSysRole.Referee)
                                    {
                                        return RedirectToAction("Index", "Home");

                                    }
                                }

                            }
                            else
                            {
                                message = "Invalid credentials provided";

                                _notify.Error("Invalid credentials provided");
                            }
                        }
                        else
                        {
                            message = "Error: An error occured!";

                            _notify.Error("Error: An error occured!");

                        }
                    }
                }
                else
                {
                    message = "Error: Invalid or Empty Password!";

                    _notify.Error("Error: Invalid or Empty Password!");
                }
            }
            else
            {
                message = "Error: Invalid or Empty Email Provided!";

                _notify.Error("Error: Invalid or Empty Email Provided!");
            }

            ViewBag.Message = message;

            return View();
        }
        public async Task<ActionResult> Logout()
        {
            Utility.loggedInUser = String.Empty;

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(SignIn));
        }
    }
}
