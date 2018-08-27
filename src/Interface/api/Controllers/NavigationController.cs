using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	[Route("api/[controller]")]
	public class NavigationController : Controller
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok();
		}
	}
}
