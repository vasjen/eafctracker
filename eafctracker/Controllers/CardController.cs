
using Eafctracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eafctracker.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    
    private readonly ILogger<CardController> _logger;

    public CardController(ILogger<CardController> logger)
    {
        _logger = logger;
    }

    // [HttpGet]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [Route("{id:int}")]
    // public async Task<IActionResult> Get(int id)
    // {
        // var card = await _cardService.GetCard(id);
        // if (card == null)
        // {
            // return NotFound($"Card with id: '{id}' not found");
        // }
        // return Ok(card);
    // }
    // [HttpGet(Name = "GetMaxId")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<int> GetMaxId()
    // {
        // var MaxId = await _cardService.GetMaxId();
        // return MaxId;
    // }
    // [HttpPut]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [Route("{id:int}")]
    // public async Task<IActionResult> Update(int id, CardUpdateRequest card)
    // {
        // if (id != card.Id)
        // {
            // return BadRequest("Id does not match");
        // }
        // await _cardService.UpdateCard(card);
        // return Ok(card);
    // }

    // [HttpDelete]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [Route("{id:int}")]
    // public async Task<IActionResult> Delete(int id)
    // {
        // var card = await _cardService.GetCard(id);
        // if (card == null)
        // {
            // return NotFound($"Card with id: '{id}' not found");
        // }
        // await _cardService.DeleteCard(id);
        // return Ok();
    // }

    // [HttpPost]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> Create(Card card)
    // {
        // var createdCard = await _cardService.CreateCard(card);
        // return CreatedAtAction(nameof(Get), new { id = createdCard.Id }, createdCard);
    // }
}

