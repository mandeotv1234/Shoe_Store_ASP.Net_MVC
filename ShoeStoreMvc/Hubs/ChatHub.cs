using Microsoft.AspNetCore.SignalR;
using ShoeStore.Models;
using ShoeStoreMvc.Models;
using ShoeStoreMvc.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ShoeStoreMvc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        // ✅ User tham gia phòng chat của riêng mình
        public async Task JoinRoom(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        // ✅ Gửi tin nhắn riêng giữa User & Admin
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            var chatMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                SentAt = DateTime.UtcNow
            };

            // ✅ Lưu tin nhắn vào database
            await _chatService.SaveMessageAsync(chatMessage);

            // ✅ Gửi tin nhắn đến user hoặc admin trong phòng chat
            await Clients.Group(receiverId).SendAsync("ReceiveMessage", senderId, message);
           // await Clients.Group(senderId).SendAsync("ReceiveMessage", receiverId, message);

            //await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
            //await Clients.Caller.SendAsync("ReceiveMessage", senderId, message);
        }
        public async Task GetUserList()
        {
            var users = await _chatService.GetChatUsers();
            await Clients.Caller.SendAsync("UpdateUserList", users);
        }
        // ✅ Lấy lịch sử tin nhắn giữa user và admin
        public async Task GetChatHistory(string userId)
        {
            var messages = await _chatService.GetMessagesAsync(userId, "admin");

            await Clients.Caller.SendAsync("LoadChatHistory", messages);
        }
    }

    //public class ChatHub : Hub
    //{
    //    private readonly ChatService _chatService;

    //    public ChatHub(ChatService chatService)
    //    {
    //        _chatService = chatService;
    //    }

    //    // ✅ Khi User hoặc Admin tham gia, tự động vào phòng riêng của họ
    //    public async Task JoinRoom(string userId)
    //    {
    //        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    //    }

    //    // ✅ Gửi tin nhắn giữa User & Admin (cả hai bên đều nhận được)
    //    public async Task SendMessage(string senderId, string receiverId, string message)
    //    {
    //        if (string.IsNullOrEmpty(message)) return;

    //        var chatMessage = new Message
    //        {
    //            SenderId = senderId,
    //            ReceiverId = receiverId,
    //            Content = message,
    //            SentAt = DateTime.UtcNow
    //        };

    //        // ✅ Lưu tin nhắn vào database
    //        await _chatService.SaveMessageAsync(chatMessage);

    //        // ✅ Gửi tin nhắn đến cả User & Admin
    //        await Clients.Group(receiverId).SendAsync("ReceiveMessage", senderId, message);
    //        await Clients.Group(senderId).SendAsync("ReceiveMessage", senderId, message);
    //    }

    //    // ✅ Lấy danh sách User đã chat với Admin
    //    public async Task GetUserList()
    //    {
    //        var users = await _chatService.GetChatUsers();
    //        await Clients.Caller.SendAsync("UpdateUserList", users);
    //    }

    //    // ✅ Lấy lịch sử tin nhắn giữa User và Admin
    //    public async Task GetChatHistory(string userId)
    //    {
    //        var messages = await _chatService.GetMessagesAsync(userId, "admin");
    //        await Clients.Caller.SendAsync("LoadChatHistory", messages);
    //    }
    //}

}
