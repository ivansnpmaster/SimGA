namespace SimGA.Tests.InnerProduct
{
    public sealed class InnerProductEuclidean2DAlgebraTests
    {
        public InnerProductEuclidean2DAlgebraTests() => Algebra.Set(2, 0, 0);

        [Fact]
        public void InnerProduct_BasisVectors_SquareToOne()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);

            // Act
            var e1dote1 = e1 | e1;
            var e2dote2 = e2 | e2;

            // Assert
            // e1 | e1 = 1, e2 | e2 = 1
            Assert.Equal(1, e1dote1[0], 10);
            Assert.Equal(1, e2dote2[0], 10);
            Assert.True(e1dote1.IsScalar());
            Assert.True(e2dote2.IsScalar());
        }

        [Fact]
        public void InnerProduct_OrthogonalVectors_ReturnsZero()
        {
            // Arrange
            var v1 = new Multivector(0, 1, 1); // e1 + e2
            var v2 = new Multivector(0, 1, -1); // e1 - e2

            // Act
            var result = v1 | v2;

            // Assert
            Assert.True(result.IsZero());
        }

        [Fact]
        public void InnerProduct_VectorAndBivector_ReturnsContraction()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e12 = Multivector.CreateBaseBlade(3); // e12

            // Act
            var result = e1 | e12;

            // Assert
            // e1 | e12 = e2
            Assert.Equal(1, result[2], 10); // e2
            Assert.Equal(0, result[0], 10); // scalar
            Assert.Equal(0, result[1], 10); // e1
            Assert.Equal(0, result[3], 10); // e12
        }

        [Fact]
        public void InnerProduct_Vectors_IsSymmetric()
        {
            // Arrange
            var v1 = new Multivector(0, 1, 2); // e1 + 2e2
            var v2 = new Multivector(0, 3, 4); // 3e1 + 4e2

            // Act
            var v1Dotv2 = v1 | v2;
            var v2Dotv1 = v2 | v1;

            // Assert
            // 1*3 + 2*4 = 11
            Assert.Equal(11, v1Dotv2[0], 10);
            Assert.Equal(v1Dotv2, v2Dotv1);
            Assert.True(v1Dotv2.IsScalar());
        }

        [Fact]
        public void InnerProduct_VectorAndBivector_IsAntiSymmetric()
        {
            // Arrange
            var v = Multivector.CreateBaseBlade(1); // e1
            var B = Multivector.CreateBaseBlade(3); // e12

            // Act
            var left = v | B;
            var right = B | v;

            // Assert
            // v | B = - (B | v)
            Assert.Equal(left, -right);
        }

        [Fact]
        public void InnerProduct_Bivectors_ReturnsNegativeOne()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3);

            // Act
            var result = e12 | e12;

            // Assert
            // e12 | e12 = -1
            Assert.Equal(-1, result[0], 10);
            Assert.True(result.IsScalar());
        }
    }
}