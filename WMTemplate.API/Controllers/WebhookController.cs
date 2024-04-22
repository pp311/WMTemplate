using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WMTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
	private readonly ILogger<WebhookController> _logger;

	public WebhookController(ILogger<WebhookController> logger)
	{
		_logger = logger;
	}


	[HttpGet]
	public string Get([FromQuery(Name = "hub.mode")] string hub_mode,
	                  [FromQuery(Name = "hub.challenge")] string hub_challenge,
	                  [FromQuery(Name = "hub.verify_token")] string hub_verify_token)
	{
		if ("qwerty" == hub_verify_token)
		{
			return hub_challenge;
		}
		else
		{
			return "error. no match";
		}
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] FacebookWrapper request)
	{
		_logger.LogInformation($"Received message from {request.Entry.First().Messaging.First().Sender.Id}: {request.Entry.First().Messaging.First().Message.Text}");

		return Ok();
	}
	
	[HttpGet("send")]
	public async Task<IActionResult> Post([FromQuery] string text,
	                                      [FromQuery] string accessToken,
	                                      [FromQuery] string receiverId)
	{
		HttpClient client = new HttpClient();
		var body = new
		{
			messaging_type = "RESPONSE",
			recipient = new
			{
				id = receiverId
			},
			message = new
			{
				text = text
			}
		};


		var url = $"https://graph.facebook.com/v12.0/me/messages?access_token={accessToken}";

		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

		using StringContent jsonContent = new(
			JsonSerializer.Serialize(body),
			Encoding.UTF8,
			"application/json");

		await client.PostAsync(url, jsonContent);

		return Ok();
	}
}

public class FacebookMessageRequest
{
	[JsonProperty("sender")]
	public FacebookSender Sender { get; set; }
	
	[JsonProperty("recipient")]
	public FacebookRecipient Recipient { get; set; }
	
	[JsonProperty("timestamp")]
	public long Timestamp { get; set; }
	
	[JsonProperty("message")]
	public FacebookMessage Message { get; set; }
}

public class FacebookSender
{
	[JsonProperty("id")]
	public string Id { get; set; }
}

public class FacebookRecipient
{
	[JsonProperty("id")]
	public string Id { get; set; }
}

public class FacebookMessage
{
	[JsonProperty("mid")]
	public string Mid { get; set; }
	[JsonProperty("text")]
	public string Text { get; set; }
}

public class FacebookWrapper
{
	[JsonProperty("object")]
	public string Object { get; set; }
	
	[JsonProperty("entry")]
	public List<FacebookMessageWrapper> Entry { get; set; }
}

public class FacebookMessageWrapper
{
	[JsonProperty("id")] 
	public string Id { get; set; }
	
	[JsonProperty("time")]
	public long Time { get; set; }
	
	[JsonProperty("messaging")]
	public List<FacebookMessageRequest> Messaging { get; set; }
}
