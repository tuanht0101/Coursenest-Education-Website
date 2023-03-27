using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.API.DTOs;
using System.Globalization;


namespace Payment.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
	private readonly IRequestClient<ExtendRole> _extendRoleClient;

	public PaymentController(IRequestClient<ExtendRole> extendRoleClient)
	{
		_extendRoleClient = extendRoleClient;
	}

	// POST: /payment
	[HttpPost]
	[Authorize]
	public async Task<ActionResult> Pay(
		[FromBody] Pay body)
	{
		if (body.Role == RoleType.Admin)
		{
			return BadRequest();
		}

		var userId = ClaimUtilities.GetUserId(User.Claims);

		var cardExpiry = DateTime.ParseExact(
			$"{body.ExpiryDateMonth}/{body.ExpiryDateYear}",
			"MM/yy",
			CultureInfo.InvariantCulture);
		if (cardExpiry < DateTime.Now)
			return BadRequest("Card expired.");

		var request = new ExtendRole()
		{
			UserId = userId,
			Type = body.Role,
			ExtendedDays = body.Months * 30
		};
		var response = await _extendRoleClient
			.GetResponse<Succeeded, NotFound>(request);

		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}

		return Ok();
	}
}
