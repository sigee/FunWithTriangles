using System;
using Xunit;

namespace FunWithTriangles.UnitTests
{
    public class TriangleTests
    {
        [Theory]
        [InlineData(0, 0, 0, false)]
        [InlineData(0, 0, 1, false)]
        [InlineData(0, 1, 0, false)]
        [InlineData(0, 1, 1, false)]
        [InlineData(1, 0, 0, false)]
        [InlineData(1, 0, 1, false)]
        [InlineData(1, 1, 0, false)]
        public void ISConstructable_MissingEdge_ReturnsFalse(Int64 a, Int64 b, Int64 c, bool expectedResult)
        {
            var triangle = new Triangle() {EdgeA = a, EdgeB = b, EdgeC = c};
            var result = triangle.IsConstructable();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsConstructable_EdgesHasTheSameLong_ReturnsTrue()
        {
            var triangle = new Triangle() {EdgeA = 10, EdgeB = 10, EdgeC = 10};
            var result = triangle.IsConstructable();
            Assert.True(result);
        }

        [Theory]
        [InlineData(3, 3, 10, false)]
        [InlineData(3, 10, 3, false)]
        [InlineData(10, 3, 3, false)]
        public void IsConstructable_TwoEdgesAreIncredibleShort_ReturnsFalse(Int64 a, Int64 b, Int64 c,
            bool expectedResult)
        {
            var triangle = new Triangle() {EdgeA = a, EdgeB = b, EdgeC = c};
            var result = triangle.IsConstructable();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetArea()
        {
            Assert.True(true);
        }

        [Fact]
        public void GetAlpha()
        {
            var triangle = new Triangle() {EdgeA = 480, EdgeB = 350, EdgeC = 620};
            Assert.Equal(50.4243, triangle.GetAlpha(), 4);
        }

        [Fact]
        public void GetBeta()
        {
            var triangle = new Triangle() {EdgeA = 480, EdgeB = 350, EdgeC = 620};
            Assert.Equal(34.1963, triangle.GetBeta(), 4);
        }

        [Fact]
        public void GetGamma()
        {
            var triangle = new Triangle() {EdgeA = 480, EdgeB = 350, EdgeC = 620};
            Assert.Equal(95.3794, triangle.GetGamma(), 4);
        }

        [Fact]
        public void InnerAnglesShouldBe180Deg()
        {
            var triangle = new Triangle() {EdgeA = 480, EdgeB = 350, EdgeC = 620};

            Assert.Equal(180, triangle.GetAlpha() + triangle.GetBeta() + triangle.GetGamma(), 4);
        }

        [Fact]
        public void GetLargestAngle__ReturnsGamma()
        {
            var triangle = new Triangle() {EdgeA = 480, EdgeB = 350, EdgeC = 620};

            Assert.Equal(Angle.Gamma, triangle.GetLargestAngle());
        }
    }
}