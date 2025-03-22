//using Microsoft.AspNetCore.Mvc;
//using ShoeStore.Services;
//using ShoeStore.Models;
//using System.Threading.Tasks;

//namespace ShoeStore.Controllers
//{
//    [Route("api/chat")]
//    [ApiController]
//    public class ChatApiController : ControllerBase
//    {
//        private readonly ChatService _chatService;

//        public ChatApiController(ChatService chatService)
//        {
//            _chatService = chatService;
//        }

//        //[HttpPost]
//        //public async Task<IActionResult> SendMessage([FromBody] Message message)
//        //{
//        //    if (message == null) return BadRequest("Invalid data");

//        //    await _chatService.SaveMessage(message);
//        //    return Ok(new { success = true });
//        //}
//    }
//}
