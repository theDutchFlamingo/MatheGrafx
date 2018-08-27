using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	[Route("api/[controller]")]
	public class GraphController : Controller
	{
		// GET api/graph
		[HttpGet]
		public IActionResult Get()
		{
			Point[] pointArray = new Point[]
			{
				new Point(-3, 9),
				new Point(-2, 4),
				new Point(-1, 1),
				new Point(0, 0),
				new Point(1, 1),
				new Point(2, 4),
				new Point(3, 9),
			};

			dynamic points = new dynamic[20]; // Amount of points

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
				Precision = 0.1,

				Points = new []
				{
					new { X = 1, Y = 5 },
					new { X = 2, Y = 9 }
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
