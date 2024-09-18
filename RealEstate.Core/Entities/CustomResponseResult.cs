using Microsoft.AspNetCore.Mvc;
using RealEstate.Core.DTOs;

namespace RealEstate.Core.Entities
{
    public class CustomResponseResult : JsonResult
    {
        public CustomResponseResult(int status, string message, object description) : base(
            new Response 
            { 
                Status = status, 
                Message = message, 
                Description = description 
            })
        { }
    }
}
