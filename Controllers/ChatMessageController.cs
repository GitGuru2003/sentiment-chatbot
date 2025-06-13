using AutoMapper;
using chatbot.Data;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ChatMessageController : ControllerBase
  {
    DataContextEF dataContextEF;
    IMapper mapper;
    public ChatMessageController(IConfiguration config)
    {
      dataContextEF = new DataContextEF(config);
      mapper = new Mapper(
        new MapperConfiguration(
          cfg =>
          {
            // Will add mapping configurations here if needed
          }
        )
      );
    }
    #region ChatMessage CRUD

    #endregion
  }
}