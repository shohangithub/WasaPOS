using Data;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class FileController : Controller
    {

      
        private ApplicationUserManager _userManager;

        public FileController()
        {
        }

        public FileController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
          
        }

     

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: File
        public async Task<ActionResult> Index(string id)
        {

            var fileToRetrieve = await UserManager.FindByIdAsync(id);
          
            return File(fileToRetrieve.Image, fileToRetrieve.ContentType);
        }
    }
}