using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.ChatUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;

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
        public async Task<IActionResult> SendMassage([FromBody] SendMessageRequest request)
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


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Tin nhắn đã được gơi", Data = request });
        }
    }
}
