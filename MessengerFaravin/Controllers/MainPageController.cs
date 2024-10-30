using MessengerFaravin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MessengerFaravin.Controllers
{

    public class MainPageController : Controller
    {
        public faravinEntities1 db = new faravinEntities1();
        public int userId;


        public ActionResult Index()
        {
            return View("mainPage");
        }


        // MIX OF GROUP AND PV NAMES
        public ActionResult MessageList()
        {
            /* JUST GROUP ITEMS(try 1)
             * 
            public ActionResult MessageList()
            {
                userId = Convert.ToInt32(Session["userId"]);
                List<group> groupsUser = new List<group>();
                var groupsId = db.groupsUsers.Where(x => x.user_id == userId).Where(x => x.isActive == true).Select(c => c.group_id).ToList();
                foreach (var groupId in groupsId)
                {
                    var group = db.group.Where(x => x.id == groupId).Where(x => x.isActive == true).FirstOrDefault();
                    if (group != null)
                    {
                        groupsUser.Add(group);
                    }
                }
                return PartialView(groupsUser);
            }
          */



            /* try 2
             * 
            userId = Convert.ToInt32(Session["userId"]);
            List<(string chatType, string name, string key, DateTime time)> messageNamesList = new List<(string, string, string, DateTime)>();


            var groupsId = db.groupsUsers.Where(x => x.user_id == userId && x.isActive == true).Select(c => c.group_id).ToList();
            foreach (var groupId in groupsId)
            {
                var group = db.group.Where(x => x.id == groupId && x.isActive == true).FirstOrDefault();
                if (group != null)
                {
                    var lastMessageId = db.messageRecipient.Where(x2 => x2.groupReceiver_id == group.id).OrderByDescending(p => p.id).Select(x3 => x3.message_id).FirstOrDefault();
                    var time = db.message.Where(c => c.id == lastMessageId).Select(c => c.sendTime).FirstOrDefault();
                    messageNamesList.Add(("group", group.groupName, group.id.ToString(), time));
                }
            }


            int? receiverId = 0;
            List<int?> repeatController = new List<int?>();
            var sentId = db.message.Where(x => x.sender_id == userId).Select(x => x.id).ToList();
            foreach (var messageId in sentId)
            {
                if (!repeatController.Contains(receiverId))
                {
                    receiverId = db.messageRecipient.Where(x => x.message_id == messageId).Select(x => x.userReceiver_id).FirstOrDefault();
                    repeatController.Add(receiverId);


                    List<DateTime> newList = new List<DateTime>();
                    var messagesId = db.messageRecipient.Where(x => x.userReceiver_id == receiverId).Select(x => x.message_id).ToList();
                    foreach (var message in messagesId)
                    {
                        DateTime messageDate = db.message.Where(x => x.sender_id == userId && x.id == message).Select(x => x.sendDate).FirstOrDefault();
                        newList.Add(messageDate);
                    }
                    var date = newList.OrderByDescending(x => x).FirstOrDefault();


                    var contactUser = db.user.Where(x => x.id == receiverId).FirstOrDefault();
                    messageNamesList.Add(("pv", contactUser.firstName + " " + contactUser.lastName, contactUser.phoneNumber, date));
                }
            }


             //var receivedId = db.messageRecipient.Where(x => x.userReceiver_id == userId).Select(x => x.message_id).ToList();
            //foreach (var messageId in receivedId)
            //{
            //    var senderId = db.message.Where(x => x.id == messageId).Select(x => x.sender_id).FirstOrDefault();
            //    var userPhone = db.user.Where(x => x.id == senderId).Select(x => x.phoneNumber).FirstOrDefault();
            //    messageNamesList.Add(userPhone);
            //}

            return PartialView(messageNamesList.Distinct().OrderByDescending(x=>x.time));
            */


            userId = Convert.ToInt32(Session["userId"]);
            List<(string chatType, string name, string key, DateTime time)> groupNamesList = new List<(string, string, string, DateTime)>();
            List<(string chatType, string name, string key, DateTime time)> mergedList = new List<(string chatType, string name, string key, DateTime time)>();


            var groupsId = db.groupsUsers.Where(x => x.user_id == userId && x.isActive == true).Select(c => c.group_id).ToList();
            foreach (var groupId in groupsId)
            {
                var group = db.group.Where(x => x.id == groupId && x.isActive == true).FirstOrDefault();
                if (group != null)
                {
                    var lastMessageId = db.messageRecipient.Where(x2 => x2.groupReceiver_id == group.id).OrderByDescending(p => p.id).Select(x3 => x3.message_id).FirstOrDefault();
                    var time = db.message.Where(c => c.id == lastMessageId).Select(c => c.sendTime).FirstOrDefault();
                    groupNamesList.Add(("group", group.groupName, group.id.ToString(), time));
                }
            }


            var receivedMessages = db.messageRecipient.Where(i => i.userReceiver_id == userId).GroupBy(i => i.message.user)
                .Select(f => new { User = f.Key, LastMessage = f.OrderByDescending(c => c.message.sendTime).FirstOrDefault() })
                .Select(x => new { chatType = "pv", contactName = x.User.lastName + " " + x.User.firstName, key = x.User.phoneNumber, date = x.LastMessage.message.sendTime })
                .ToList();


            var sentMessages = db.messageRecipient.Where(d => d.message.sender_id == userId).GroupBy(f => f.user)
                .Select(f => new { User = f.Key, LastMessage = f.OrderByDescending(c => c.message.sendTime).FirstOrDefault() })
                .Select(x => new { chatType = "pv", contactName = x.User.lastName + " " + x.User.firstName, key = x.User.phoneNumber, date = x.LastMessage.message.sendTime })
                .ToList();


            var privateMessages = receivedMessages.Concat(sentMessages).GroupBy(i => i.key)
                .Select(i => i.OrderByDescending(f => f.date).First()).OrderByDescending(m => m.date)
                .ToList();


            List<(string chatType, string name, string key, DateTime time)> messageNamesList = privateMessages
                .Select(x => (chatType: x.chatType, name: x.contactName, key: x.key, time: x.date)).ToList();


            mergedList.AddRange(groupNamesList);
            mergedList.AddRange(privateMessages.Select(x => (chatType: x.chatType, name: x.contactName, key: x.key, time: x.date)));

           
            return PartialView(mergedList.OrderByDescending(i=>i.time));
        }


        public ActionResult UserContactsList()
        {
            userId = Convert.ToInt32(Session["userId"]);
            var contacts = db.contacts.Where(a => a.listOwner_id == userId).ToList();
            return PartialView("UserContactsList", contacts);
        }


        public JsonResult AddContact(string firstName, string lastName, string phoneNumber)
        {
            byte A = 0;
            userId = Convert.ToInt32(Session["userId"]);
            var contactUser = db.user.Where(x => x.phoneNumber == phoneNumber).FirstOrDefault();
            var rContact = db.user.Where(x => x.id == userId).FirstOrDefault().contacts.Where(c => c.contactUser_id == contactUser.id).FirstOrDefault();
            //var selfUser = db.user.Where(x => x.id == userId).FirstOrDefault();
            //var rContact=selfUser.contacts.Where(x=>x.contactUser_id == contactUser.id).FirstOrDefault();


            if (contactUser == null)
            {
                A = 1;
            }
            else if (contactUser != null && rContact == null)
            {
                contacts newContact = new contacts();
                newContact.listOwner_id = Convert.ToInt32(Session["userId"]);
                newContact.contactUser_id = contactUser.id;
                newContact.name = firstName;
                newContact.family = lastName;
                newContact.status = true;
                db.contacts.Add(newContact);
                db.SaveChanges();
                A = 2;
            }
            else if (rContact != null)
            {
                A = 3;
            }


            return Json(new { status = A, JsonRequestBehavior.AllowGet });
        }


        public ActionResult OpenContact(string phoneNumber)
        {
            var contactId = db.user.Where(x => x.phoneNumber == phoneNumber).Select(x => x.id).FirstOrDefault();
            userId = Convert.ToInt32(Session["userId"]);
            List<message> messages = new List<message>();
            if (contactId <= 0) contactId = 1;
            Session["contactId"] = contactId;
            ViewBag.userId = Session["userId"];
            ViewBag.contactId = contactId;


            var sentId = db.messageRecipient.Where(x => x.userReceiver_id == contactId).Select(x => x.message_id).ToList();
            foreach (var messageId in sentId)
            {
                var message = db.message.Where(x => x.id == messageId && x.sender_id == userId).FirstOrDefault();
                if (message != null) messages.Add(message);
            }


            var receivedId = db.messageRecipient.Where(x => x.userReceiver_id == userId).Select(x => x.message_id).ToList();
            foreach (var messageId in receivedId)
            {
                var message = db.message.Where(x => x.id == messageId && x.sender_id == contactId).FirstOrDefault();
                if (message != null) messages.Add(message);
            }


            //Session["messagesText"] = messages;
            return PartialView("ChatMessages", messages.OrderByDescending(x => x.sendTime));
        }


        /* some try
         * 
         public ActionResult MessagesPage()
        {
            if (Session["messagesText"] != null)
            {
                var messages = Session["messagesText"] as List<message>;
                ViewBag.userId = Session["userId"];
                return PartialView(messages);
            }
            else
            {
                int a = 0;
                ViewBag.userId = a;
                return PartialView (db.message.Where(x => x.id == 2).ToList());
            }
        }
        */


        public ActionResult SendMessage(string text)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var contactId = Convert.ToInt32(Session["contactId"]);
            List<message> messages = new List<message>();
            ViewBag.userId = Session["userId"];
            ViewBag.contactId = contactId;

            
            message newMessage = new message
            {
                sender_id = userId,
                messageText = text,
                sendDate = DateTime.Today,
                sendTime = DateTime.Now,
                isActive = true
            };
            db.message.Add(newMessage);
            db.SaveChanges();


            messageRecipient newMessageRecipient = new messageRecipient
            {
                message_id = newMessage.id,
                userReceiver_id = contactId
            };
            db.messageRecipient.Add(newMessageRecipient);
            db.SaveChanges();


            var sentId = db.messageRecipient.Where(x => x.userReceiver_id == contactId).Select(x => x.message_id).ToList();
            foreach (var messageId in sentId)
            {
                var message = db.message.Include(x => x.user).Where(x => x.id == messageId && x.sender_id == userId).FirstOrDefault();
                if (message != null) messages.Add(message);
            }


            var receivedId = db.messageRecipient.Where(x => x.userReceiver_id == userId).Select(x => x.message_id).ToList();
            foreach (var messageId in receivedId)
            {
                var message = db.message.Include(x => x.user).Where(x => x.id == messageId && x.sender_id == contactId).FirstOrDefault();
                if (message != null) messages.Add(message);
            }


            //if (messages==null) return PartialView("ChatMessages", new List<message>());
            return PartialView("ChatMessages", messages.OrderByDescending(c => c.sendTime));
        }
    }
}