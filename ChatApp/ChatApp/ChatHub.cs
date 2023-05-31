﻿using Microsoft.AspNetCore.SignalR;

namespace ChatApp
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string user,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",user,message);
        }
    }
}
