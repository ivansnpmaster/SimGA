namespace SimGA.Tests.InnerProduct
{
    public sealed class InnerProductComplexAlgebraTests
    {
        public InnerProductComplexAlgebraTests() => Algebra.Set(0, 2, 0);

        [Fact]
        public void InnerProduct_ImaginaryUnit_SquaresToMinusOne()
        {
            // Arrange
            var i = new Multivector(0, 0, 0, 1); // i = e12

            // Act
            var result = i | i;

            // Assert
            // i | i = -1
            Assert.Equal(-1, result[0], 10);
            Assert.True(result.IsScalar());
        }

        [Fact]
        public void InnerProduct_ComplexNumbers_IsCommutative()
        {
            // Arrange
            var c1 = new Multivector(2, 0, 0, 3); // 2 + 3i
            var c2 = new Multivector(1, 0, 0, 4); // 1 + 4i

            // Act
            var dot12 = c1 | c2;
            var dot21 = c2 | c1;

            // Assert
            Assert.Equal(dot12, dot21);
        }

        [Fact]
        public void InnerProduct_ScalarAndImaginaryUnit_ReturnsScaledImaginaryUnit()
        {
            // Arrange
            var scalar = new Multivector(2);
            var i = new Multivector(0, 0, 0, 1); // i = e12

            // Act
            var result = scalar | i;

            // Assert
            // 2 | i = 2i
            Assert.Equal(2, result[3], 10);
            Assert.Equal(0, result[0], 10);
        }

        [Fact]
        public void InnerProduct_General_IsLinear()
        {
            // Arrange
            var a = new Multivector(2);
            var x = new Multivector(3, 0, 0, 1); // 3 + i
            var y = new Multivector(1, 0, 0, 2); // 1 + 2i

            // Act
            var left = a | (x + y);
            var right = (a | x) + (a | y);

            // Assert
            // a | (x + y) == (a | x) + (a | y)
            Assert.Equal(left, right);
        }

        [Fact]
        public void InnerProduct_ConjugatePairs_ReturnsModulusSquared()
        {
            // Arrange
            var z = new Multivector(3, 0, 0, 4); // 3 + 4i
            var zConj = new Multivector(3, 0, 0, -4); // 3 - 4i

            // Act
            var result = z | zConj;

            // Assert
            // (3 + 4i) | (3 - 4i) = 3 * 3 + 4 * 4 = 9 + 16 = 25
            Assert.Equal(25, result[0], 10);
            Assert.True(result.IsScalar());
        }
    }
}
