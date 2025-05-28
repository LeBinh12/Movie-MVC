using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WbeMovieUser.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinGroup(string role)
        {
            if (role == "Admin")
            {
                await Groups.Add(Context.ConnectionId, "Admins");
            }
            else
            {
                await Groups.Add(Context.ConnectionId, "Users");
            }
        }

        public void SendToAdmin(string user, string message)
        {
            Clients.Group("Admins").receiveMessage(user, message);
        }

        public void SendToUser(string admin, string message)
        {
            Clients.Group("Users").receiveMessage(admin, message);
        }

        public override async Task OnConnected()
        {
            // Không cần logic dựa trên Referer nữa, sẽ gọi JoinGroup từ client
            await base.OnConnected();
        }
    }
}