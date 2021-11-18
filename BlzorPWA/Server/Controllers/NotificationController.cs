using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BlzorPWA.Server.Models;
using BlzorPWA.Shared.Models;
using Newtonsoft.Json;
using WebPush;

namespace BlzorPWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotificationController : Controller
    {
        private readonly UserInformationContext _userDb;

        public NotificationController(UserInformationContext userDb)
        {
            _userDb = userDb;
        }

        [HttpPut]
        public async Task<IActionResult> Subscribe(NotificationSubscription subscription)
        {
            // Fake data
            var userId = 1;

            var oldSubscription = _userDb.NotificationSubscriptions.SingleOrDefault(e => e.UserId == userId);
            if (oldSubscription != null) _userDb.NotificationSubscriptions.Remove(oldSubscription);

            subscription.UserId = userId;
            _userDb.NotificationSubscriptions.Add(subscription);

            await _userDb.SaveChangesAsync();
            return Ok(subscription);
        }

        /// <summary>
        /// Talking to that push servers of user's browser
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SendNotificationAsync()
        {
            var publicKey = "BI1kOwCq-466LEfvvDjdfNpjByjJyyXTdlIzjl-teZeBCy8_V788GWgBiZyvfilxYs1hd43eiZgpFKXTHfZ015U";
            var privateKey = "vtwjmVCM_OfkTStNquu8arGscuickmx6VsYcmzqpa4M";

            // Fake data
            var userId = 1;

            var subscription = _userDb.NotificationSubscriptions.SingleOrDefault(e => e.UserId == userId);
            if (subscription != null)
            {
                var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
                var vapidDetails = new VapidDetails("mailto:<andrew.lan@mojo-domo.com>", publicKey, privateKey);
                var webPushClient = new WebPushClient();
                try
                {
                    var payload = JsonConvert.SerializeObject(new
                    {
                        message = "Welcome to my Blazor project!!!!!",
                        url = "counter",
                    });
                    await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest("Error sending push notification: " + ex.Message);
                }

            }
            else
            {
                return BadRequest("Can not find the permission in this user");
            }
        }

        [HttpGet]
        public IActionResult GetVapidKeys()
        {
            var vapidDetails = VapidHelper.GenerateVapidKeys();
            return Ok($"public key: {vapidDetails.PublicKey}, private key: {vapidDetails.PrivateKey}");
        }
    }
}
