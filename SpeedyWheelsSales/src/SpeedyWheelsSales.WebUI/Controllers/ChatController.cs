using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class ChatController : Controller
{
    [Authorize]
    public IActionResult GetChat(string recipientUsername, string currUserUsername)
    {
        ViewBag.currUserUsername = currUserUsername;
        ViewBag.recipientUsername = recipientUsername;
        return View();
    }
}