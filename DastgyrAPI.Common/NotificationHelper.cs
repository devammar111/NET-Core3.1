using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DastgyrAPI.Common
{
    public class NotificationHelper
    {
        public static async Task<string> SendNotification(string clientToken, string title, string body)
        {
            var registrationTokens = clientToken;
            var message = new Message()
            {
                Token = registrationTokens,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                },
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message).ConfigureAwait(true);
            return "";
        }
    }
}
