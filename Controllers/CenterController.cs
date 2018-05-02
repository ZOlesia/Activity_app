using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using entity_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace entity_app.Controllers
{
    public class CenterController : Controller
    {
        private EntityContext _context;
 
        public CenterController(EntityContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("Home")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("current_userid") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = HttpContext.Session.GetInt32("current_userid");
            ViewBag.username = _context.users.SingleOrDefault(d => d.userid == HttpContext.Session.GetInt32("current_userid"));

            ViewBag.all_activities = _context.centers.Include(m => m.coordinator).Include(e => e.participants).ThenInclude(v => v.user).Where(r => r.date > DateTime.Now).OrderByDescending(v => v.created_at);
   
            return View("dashboard");
        }

        [HttpGet]
        [Route("New")]
        public IActionResult NewActivity()
        {
            if(HttpContext.Session.GetInt32("current_userid") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("newactivity");
        }


        [HttpPost]
        [Route("create/activity")]
        public IActionResult CreateActivity(FormActivityViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.date <= DateTime.Now)
                {
                    TempData["date_error"] = "Date must be in the future";
                    return View("newactivity");
                }
                Center newActivity = new Center{
                    title = model.title,
                    time = model.time,
                    date = model.date,
                    duration = model.duration,
                    duration_time = model.duration_time,
                    description = model.description,
                    coordinatorid = (int)HttpContext.Session.GetInt32("current_userid")
                };
                _context.centers.Add(newActivity);
                _context.SaveChanges();
                return RedirectToAction("Show", new{id = newActivity.centerid});
            };
            return View("newactivity");
        }

        [HttpPost]
        [Route("action")]
        public IActionResult Action(string action, int ActivityId, string duration_time)
        {
            if(HttpContext.Session.GetInt32("current_userid") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // var joined = _context.participants.Include(v => v.activity).Where(r => r.userid == HttpContext.Session.GetInt32("current_userid"));

            // var joined_duration = _context.participants.Include(v => v.activity).Where(r => r.userid == HttpContext.Session.GetInt32("current_userid")).SingleOrDefault();

            // var notjoined = _context.centers.Where(x => x.centerid == ActivityId).SingleOrDefault();

            // TimeSpan dur_time;

            // if(duration_time == "minutes")
            // {
            //     dur_time = TimeSpan.FromMinutes(joined_duration.activity.duration);
            // }
            // else if(duration_time == "hours")
            // {
            //     dur_time = TimeSpan.FromHours(joined_duration.activity.duration);
            // }
            // else if(duration_time == "days")
            // {
            //     dur_time = TimeSpan.FromDays(joined_duration.activity.duration);
            // }

            if(action == "Delete")
            {
                // var del = _context.weddings.SingleOrDefault(c => c.weddingid == WeddingId);
                _context.centers.Remove(_context.centers.SingleOrDefault(c => c.centerid == ActivityId));
            }   
            else if(action == "Join")
            {
                // foreach(var i in joined)
                // {
                //     if(i.activity.time <= notjoined.time && i.activity.time + dur_time >= notjoined.time)
                //     {
                //         // ViewBag.username = _context.users.SingleOrDefault(d => d.userid == HttpContext.Session.GetInt32("current_userid"));
                //         // ViewBag.all_activities = _context.centers.Include(m => m.coordinator).Include(e => e.participants).ThenInclude(v => v.user).Where(r => r.date > DateTime.Now).OrderByDescending(v => v.created_at);
                //         TempData["err"] = "You are joining other activity during this time";
                //         return RedirectToAction("dashboard");
                //     }
                // }

                Participant newParticipant = new Participant{
                userid = (int)HttpContext.Session.GetInt32("current_userid"),
                activityid = ActivityId
                };
                _context.participants.Add(newParticipant);
            }
            else if(action == "Leave")
            {
                _context.participants.Remove( _context.participants.Where(e => e.userid == (int)HttpContext.Session.GetInt32("current_userid")).Where(c => c.activityid == ActivityId).SingleOrDefault());
            }
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }



        [HttpGet]
        [Route("activity/{id}")]
        public IActionResult Show(int id)
        {
            ViewBag.user = HttpContext.Session.GetInt32("current_userid");
            if(HttpContext.Session.GetInt32("current_userid") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.all_info = _context.centers.Where(a=>a.centerid == id).Include(m => m.coordinator).Include(e => e.participants).ThenInclude(v => v.user).ToList();

            //  _context.centers.Include(m => m.coordinator).Include(e => e.participants).ThenInclude(v => v.user).Where(r => r.date > DateTime.Now).OrderByDescending(v => v.created_at)

            ViewBag.all_part = _context.participants.Include(c => c.activity).Where(t => t.activityid == id).Where(s => s.userid == HttpContext.Session.GetInt32("current_userid"));
            return View("activity");
        }

    }
}








    // @if(@ViewBag.all_info.coordinatorid == @ViewBag.user)
    // {
    //     <td><input type="submit" name="action" class="btn btn-danger" value="Delete"></td>
    // }
    // else if(@ViewBag.all_info.participants.userid == @ViewBag.user)
    // {
    //     <td><input type="submit" name="action" value="Leave" ></td>
    // }
    // else
    // {
    //     <td><input type="submit" name="action" value="Join"></td>
    // }


            //     if(p.userid != @ViewBag.user)
            // {
            //     <td><input type="submit" name="action" value="Join"></td>
            // }
            // else
            // {
            //     <td><input type="submit" name="action" value="Leave" ></td>
            // }












//     <form action="/action" method="POST">
//     <input type="hidden" value="@ViewBag.all_info.centerid" name="ActivityId">
//     @if(@ViewBag.all_info.coordinatorid == @ViewBag.user)
//     {
//         <td><input type="submit" name="action" class="btn btn-danger" value="Delete"></td>
//     }
//     else
//     {
//         if(@ViewBag.all_part == null)
//         {
//             <td><input type="submit" name="action" value="Join"></td>
//         }
//         else
//         {
//             <td><input type="submit" name="action" value="Leave" ></td>
//         }
//     }
// </form>