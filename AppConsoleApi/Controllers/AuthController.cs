using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseOperationLibrary;
using FileOperationLibrary;
using ModelLibrary;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace AppConsoleApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [ActionName("authenticate")]
        public ActionResult Authenticate(User user)
        {
            bool check;
            DatabaseOperation db = new DatabaseOperation();
            check = db.Authenticate(user.Username, user.Password);
            if (check)
            {
                return StatusCode(200);
            }
            return StatusCode(404);

        }
        [HttpPost]
        [ActionName("changepassword")]
        public ActionResult ChangePassword(ChangeUser user)
        {
            if (!userExist(user))
            {
                return StatusCode(404);
            }
            DatabaseOperation db = new DatabaseOperation();
            db.ChangePassword(user.Username, user.OldPassword, user.NewPassword);
            return StatusCode(200);
        }

        private bool userExist(ChangeUser user)
        {
            DatabaseOperation db = new DatabaseOperation();
            return db.Authenticate(user.Username, user.OldPassword);
        }

    }

}