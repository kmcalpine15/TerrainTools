using Terrain.Data;

namespace Terrain.Test
{
    [TestClass]
    public class CoordinateTransformTest
    {
        [DataTestMethod]
        [DataRow(new int[] { 0, 0, 10, 10 }, new int[] { -5, -5, 5, 5 }, new int[] { 5, 5 }, new int[] {0, 0})]
        [DataRow(new int[] { 0, 0, 10, 10 }, new int[] { 0, 0, 100, 100 }, new int[] { 5, 5 }, new int[] { 50, 50 })]
        [DataRow(new int[] { 0, 0, 10, 10 }, new int[] { -100, -100, 0, 0 }, new int[] { 5, 5 }, new int[] { -50, -50 })]
        public void Transform_ShouldReturncorrectCoordinate(int[] fromSpace, int[] toSpace, int[] coordinate, int[] expected)
        {
            CoordinateSpace from = new CoordinateSpace(fromSpace[0], fromSpace[1], fromSpace[2], fromSpace[3]);
            CoordinateSpace to = new CoordinateSpace(toSpace[0], toSpace[1], toSpace[2], toSpace[3]);

            CoordinateTransform xf = new CoordinateTransform(from, to);

            Coordinate testCoord = new Coordinate(coordinate[0], coordinate[1]);
            Coordinate expectedCoord = new Coordinate(expected[0], expected[1]);

            Coordinate result = xf.Transform(testCoord);

            Assert.AreEqual(expectedCoord, result);
        }

    }
}