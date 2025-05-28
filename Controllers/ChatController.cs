using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers
{
    public class ChatController : Controller
    {
        private Model1 _dbContext = new Model1();

        public ActionResult GetChatHistory(string userId)
        {
            var messages = _dbContext.ChatMessages
                .Where(m => (m.SenderId == userId && m.ReceiverId == "Admin") || (m.SenderId == "Admin" && m.ReceiverId == userId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new { SenderId = m.SenderId, Message = m.Message, Timestamp = m.Timestamp })
                .ToList();

            return Json(new { success = true, messages = messages }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}