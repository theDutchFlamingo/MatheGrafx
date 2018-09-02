using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	[Route("api/[controller]")]
	public class GraphController : Controller
	{
		public static readonly List<string> Spacings = new List<string>
		{
			"2", "5", "10" 
		};

		// GET api/graph
		[HttpGet]
		public IActionResult Get()
		{
			var points1 = new []
			{
				new {X = -3, Y = 9},
				new {X = -2, Y = 4},
				new {X = -1, Y = 1},
				new {X = 0, Y = 0},
				new {X = 1, Y = 1},
				new {X = 2, Y = 4},
				new {X = 300, Y = 900}
			};

			//dynamic points = new dynamic[20]; // Amount of points

			//for (int i = 0; i < pointArray.Length; i++)
			//{
			//	points[i] = new
			//	{
			//		x = pointArray[i].X,
			//		y = pointArray[i].Y
			//	};
			//}

			var result = new
			{
				StepX = 0.1,
				StepY = 0.1,

				MinX = -10,
				MaxX = 10,
				MinY = -10,
				MaxY = 10,

				GridSpacing = "2",

				Functions = new[]
				{
					new
					{
						Name = "f1",
						Points = points1,
						IsVisible = true,
						IsFocused = true,
						Color = "blue"
					}, // Example function
				}
			};

			return Ok(result);
		}

		[HttpPost]
		public void Post()
		{

		}
	}
}
