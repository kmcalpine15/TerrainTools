using System;
using Terrain.Data;

namespace Terrain.Test
{
    [TestClass]
    public class PointTest
	{
		[TestMethod]
		public void ShouldBeEqualIfXandYMatch()
		{
			Point a = new Point(0.5, 0.6);
			Point b = new Point(0.5, 0.6);

			Assert.IsTrue(a == b);
		}

	}
}

