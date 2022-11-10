using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.ChatUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using PusherServer;
using DoubleLStore.WebApp.Entities;
namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatUserController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public ChatUserController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpGet("get-all-message")]
      
        public async Task<IActionResult> GetAllMessage()
        {
              
            var  listmes = await _context.ChatUsers.ToListAsync();

            var user = await _context.Users.ToListAsync();
                    return Ok(listmes);
 
              
        }
        [HttpGet("get-count-message-unread")]

        public async Task<IActionResult> GetCountMessage()
        {
            


            var listmes = await _context.ChatUsers.Where(x=>x.isNewMessageAdmin==true).CountAsync();

            return Ok(listmes);
 
        }
        [HttpPut("seen-message")]

        public async Task<IActionResult> SeenMessage([FromBody] SeenMessageRequest request)
        {



            var listmes = await _context.ChatUsers.FindAsync(request.ChatId);
            if(request.isAdmin==true)
            listmes.isNewMessageAdmin = false;
            else listmes.isNewMessageUser = false ;
            await _context.SaveChangesAsync();

            return Ok(new Response { Status = 200, Message = "Tin nhắn đã được xem" });

        }



        [HttpPut("send-message")]
        public async Task<IActionResult> AdminSendMassage([FromBody] SendMessageRequest request)
        {

           
            var findmesage= await _context.ChatUsers.FindAsync(request.Id);
            var allMess = await _context.ChatUsers.ToListAsync();
            if (findmesage == null)
            {
                return NotFound(new Response { Status = 404, Message = "Tin nhắn không tồn tại" });
            }


            try
            {
                if(request.isAdmin==true)
                {
                    findmesage.isNewMessageUser = true;
                    findmesage.isNewMessageAdmin = false;
                    findmesage.Message = findmesage.Message + "|admin" + "," + DateTime.Now + "," + request.Message;

                }

                else
                {
                    if(findmesage.DisplayPriority == allMess.Count)
                    {
                        findmesage.DisplayPriority = 1;
                        for(int i= 0; i < allMess.Count; i++)
                        {   if(findmesage.ChatId != allMess[i].ChatId)
                            allMess[i].DisplayPriority +=1;
                        }    
                    }
                    
                    else
                    { 
                        if(findmesage.DisplayPriority!=1)
                        {
                            findmesage.DisplayPriority = 1;
                            for (int i = 0; i < allMess.Count; i++)
                            {
                                if (findmesage.ChatId != allMess[i].ChatId && i != allMess.Count - 1)
                                    allMess[i].DisplayPriority += 1;
                            }
                        }    
                       
                    } 
                        
                    findmesage.isNewMessageAdmin = true;
                    findmesage.isNewMessageUser = false;

                    findmesage.Message = findmesage.Message + "|user" + "," + DateTime.Now + "," + request.Message;

                }

                await _context.SaveChangesAsync();
                var options = new PusherOptions
                {
                    Cluster = "ap1",
                    Encrypted = true
                };

                var pusher = new Pusher(
                  "1504639",
                  "05ba42f251be5a21e7fa",
                  "f43ca2126b9cc915b1e4",
                  options);
                if (request.isAdmin == true)
                {
                    var result = await pusher.TriggerAsync(
                  "my-channel",
                  "my-event",
                  new { message = "admin" + "," + DateTime.Now + "," + request.Message ,
                  chatId=request.Id});
                }    
                else
                {
                  var result = await pusher.TriggerAsync(
                 "my-channel",
                 "my-event",
                 new { message = "user" + "," + DateTime.Now + "," + request.Message,
                     chatId = request.Id
                 });
                }    
                   


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Tin nhắn đã được gơi", Data = request });
        }
        [HttpGet("get-message-by-userid/{id}")]

        public async Task<IActionResult> GetMessageByUserId(string id)
        {

            var listmes = await _context.ChatUsers.Where(x=>x.UserId==id).ToListAsync();
            var allMess = await _context.ChatUsers.ToListAsync();

            if (listmes.Count==0)
            {
                ChatUser chatUser = new ChatUser();
                chatUser.UserId = id;
                chatUser.Message = "admin" + "," + DateTime.Now + "," +"Bạn đã bắt đầu đoạn chat với DoubleL Store. Chúng tôi dùng thông tin từ đoạn chat này để cải thiện trải nghiệm của bạn";

                chatUser.isNewMessageUser = false;
                chatUser.isNewMessageAdmin = false;
                if (allMess.Count > 0)
                {
                    for (int i = 0; i < allMess.Count; i++)
                        allMess[i].DisplayPriority++;
                }
                chatUser.DisplayPriority = 1;
           
                
                _context.ChatUsers.Add(chatUser);
                await _context.SaveChangesAsync();
                listmes = await _context.ChatUsers.Where(x => x.UserId == id).ToListAsync();
                var user = await _context.Users.ToListAsync();
                var options = new PusherOptions
                {
                    Cluster = "ap1",
                    Encrypted = true
                };

                var pusher = new Pusher(
                  "1504639",
                  "05ba42f251be5a21e7fa",
                  "f43ca2126b9cc915b1e4",
                  options);

                var result = await pusher.TriggerAsync(
              "my-channel",
              "my-event",
              new
              {
                  message = "admin" + "," + DateTime.Now + "," + chatUser.Message,
                  chatId = chatUser.ChatId
              });
                ;
                
                

            }    
            return Ok(listmes);


        }
    }
}
