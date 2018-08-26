using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	[Route("api/[controller]")]
	public class ContactsController : Controller
	{
		// GET api/contacts
		[HttpGet]
		public IActionResult Get()
		{
			var result = new[] {
				new { FirstName = "John", LastName = "Doe" },
				new { FirstName = "Mike", LastName = "Smith" }
			};

			return Ok(result);
		}
	}
}
