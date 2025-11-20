namespace SimGA.Tests.InnerProduct
{
    public sealed class InnerProductEuclidean3DAlgebraTests
    {
        public InnerProductEuclidean3DAlgebraTests() => Algebra.Set(3, 0, 0);

        [Fact]
        public void InnerProduct_Vectors_BehavesAsDotProduct()
        {
            // Arrange
            var v1 = new Multivector(0, 1, 2, 0, 3); // 1e1 + 2e2 + 3e3
            var v2 = new Multivector(0, 4, 5, 0, 6); // 4e1 + 5e2 + 6e3

            // Act
            var result = v1 | v2;

            // Assert
            // 1 * 4 + 2 * 5 + 3 * 6 = 4 + 10 + 18 = 32
            Assert.Equal(32, result[0], 10);
            Assert.True(result.IsScalar());
        }

        [Fact]
        public void InnerProduct_VectorAndOrthogonalBivector_ReturnsZero()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e23 = Multivector.CreateBaseBlade(6); // e23

            // Act
            var result = e1 | e23;

            // Assert
            Assert.True(result.IsZero());
        }

        [Fact]
        public void InnerProduct_VectorAndTrivector_ReturnsBivector()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e123 = Multivector.CreateBaseBlade(7);

            // Act
            var result = e1 | e123;

            // Assert
            // e1 | e123 = e23
            Assert.Equal(1, result[6], 10); // e23 component
            Assert.Equal(0, result[0], 10);
            Assert.Equal(0, result[1], 10);
        }

        [Fact]
        public void InnerProduct_BivectorAndTrivector_ReturnsVector()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3);
            var e123 = Multivector.CreateBaseBlade(7);

            // Act
            var result = e12 | e123;

            // Assert
            // e12 | e123 = -e3
            Assert.Equal(-1, result[4], 10); // -e3
            Assert.Equal(0, result[0], 10);
        }

        [Fact]
        public void InnerProduct_ParallelBivectors_ReturnsNegativeOne()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3);

            // Act
            var result = e12 | e12;

            // Assert
            Assert.Equal(-1, result[0], 10);
            Assert.True(result.IsScalar());
        }
    }
}
