using Eafctracker.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eafctracker.Controllers;

[ApiController]
[Route("[controller]")]

public class ProfitController : ControllerBase
{
    
    
    [HttpGet]
    [ProducesResponseType(typeof(Card), 200)]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(id);
    }

}