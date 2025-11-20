namespace SimGA.Tests.InnerProduct
{
    public sealed class InnerProductRealAlgebraTests
    {
        public InnerProductRealAlgebraTests() => Algebra.Set(0, 0, 0);

        [Fact]
        public void InnerProduct_Scalars_ReturnsProduct()
        {
            // Arrange
            var a = new Multivector(3);
            var b = new Multivector(4);

            // Act
            var result = a | b;

            // Assert
            // 3 * 4 = 12
            Assert.Equal(12, result[0], 10);
            Assert.True(result.IsScalar());
        }

        [Fact]
        public void InnerProduct_Scalars_IsCommutative()
        {
            // Arrange
            var a = new Multivector(3);
            var b = new Multivector(4);

            // Act
            var ab = a | b;
            var ba = b | a;

            // Assert
            Assert.Equal(ab, ba);
        }

        [Fact]
        public void InnerProduct_Scalars_IsAssociative()
        {
            // Arrange
            var a = new Multivector(2);
            var b = new Multivector(3);
            var c = new Multivector(4);

            // Act
            var left = (a | b) | c;
            var right = a | (b | c);

            // Assert
            // (2 * 3) * 4 = 24
            // 2 * (3 * 4) = 24
            Assert.Equal(left, right);
            Assert.Equal(24, left[0], 10);
        }

        [Fact]
        public void InnerProduct_Scalars_IsDistributiveOverAddition()
        {
            // Arrange
            var a = new Multivector(2);
            var b = new Multivector(3);
            var c = new Multivector(4);

            // Act
            var left = a | (b + c);
            var right = (a | b) + (a | c);

            // Assert
            // 2 * (3 + 4) = 14
            // (2 * 3) + (2 * 4) = 6 + 8 = 14
            Assert.Equal(left, right);
            Assert.Equal(14, left[0], 10);
        }

        [Fact]
        public void InnerProduct_WithZero_ReturnsZero()
        {
            // Arrange
            var a = new Multivector(5);
            var zero = new Multivector();

            // Act
            var result = a | zero;

            // Assert
            Assert.True(result.IsZero());
        }
    }
}