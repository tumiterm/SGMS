using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGMS.Contract;
using SGMS.Helper;
using SGMS.Models;
using System.Dynamic;

namespace SGMS.Controllers
{
    [Authorize]
    public class AttachmentController : Controller
    {
        private readonly IUnitOfWork<Attachment> _context;

        private readonly IUnitOfWork<Referee> _refContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public AttachmentController(IUnitOfWork<Attachment> context,

                               IWebHostEnvironment hostEnvironment,

                               IUnitOfWork<Referee> refContext,

                               INotyfService notify)
        {
            _context = context;
             
            _refContext = refContext;

            _hostEnvironment = hostEnvironment;

            _notify = notify;
        }

        [HttpGet]
        public async Task<IActionResult> OnAddUserAttachment(Guid AssociativeKey)
        {

            if(AssociativeKey == Guid.Empty)
            {
                return NotFound();
            }

            var referee = await _refContext.OnLoadItemAsync(AssociativeKey);

            var attachments = await _context.OnLoadItemsAsync();

            var filterAtt = from n in attachments

                            where n.AssociativeKey == referee.RefereeId

                            select n;

            if(filterAtt is null)
            {
                return NotFound();
            }

            var filteredAttachments = filterAtt.ToList();

            dynamic refObj = new ExpandoObject();

            refObj.RefereeModel = filteredAttachments.ToList();

            ViewData["AssociativeKey"] = AssociativeKey;

            return View(refObj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnAddUserAttachment(Attachment attachment)
        {
            attachment.AttachmentId = Utility.GenerateGuid();

            attachment.IsActive = true;

            attachment.CreatedOn = Utility.OnGetCurrentDateTime();

            attachment.CreatedBy = Utility.loggedInUser;

            attachment.File = attachment.AttachmentFile.FileName;

            if (ModelState.IsValid)
            {
                AttachmentUploader(attachment);

                var attachmentobj = await _context.OnItemCreationAsync(attachment);

                if(attachmentobj != null)
                {

                    var rc = await _context.ItemSaveAsync();

                    if (rc > 0)
                    {
                        _notify.Success("Attachment successfully saved ");
                    }
                    else
                    {
                        _notify.Error("Error: Unable to attach file! ");
                    }
                }
                else
                {
                    _notify.Error("Error: couldn't load attachment");
                }
            }

            return RedirectToAction("OnAddUserAttachment", "Attachment", new { AssociativeKey = attachment.AssociativeKey });
        }
        public async void AttachmentUploader(Attachment attachment)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(attachment.AttachmentFile.FileName);

            string extension = Path.GetExtension(attachment.AttachmentFile.FileName);

            attachment.File = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/Attachments/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await attachment.AttachmentFile.CopyToAsync(fileStream);
            }
        }
        public async Task<IActionResult> AttachmentDownload(string filename)
        {
            if (filename == null)

                return Content("Sorry NO Attachment found!!!");


            var path1 = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot");

            string folder = path1 + @"\Attachments\" + filename;

            var memory = new MemoryStream();

            using (var stream = new FileStream(folder, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, Utility.GetContentType(folder), Path.GetFileName(folder));
        }
        public async Task<IActionResult> RemoveAttachment(Guid AttachmentId)
        {
            if (AttachmentId == Guid.Empty)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var attachmentObj = await _context.OnLoadItemAsync(AttachmentId);

            if (attachmentObj == null)
            {
                return RedirectToAction("PageNotFound", "Global");
            }

            var record = await _context.OnRemoveItemAsync(AttachmentId);

            if (record > 0)
            {
                _notify.Success("attachment successfully removed", 5);
            }
            else
            {
                _notify.Error("Error: Unable to delete attachment", 5);
            }
            return RedirectToAction("RemoveDialog", "PlayerSelection");
        }

    }

}
