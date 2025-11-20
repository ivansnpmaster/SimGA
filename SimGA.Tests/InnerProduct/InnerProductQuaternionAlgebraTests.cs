namespace SimGA.Tests.InnerProduct
{
    public sealed class InnerProductQuaternionAlgebraTests
    {
        // Quaternions correspond to the even subalgebra of Cl(0, 3, 0): scalars + bivectors
        public InnerProductQuaternionAlgebraTests() => Algebra.Set(0, 3, 0);

        [Fact]
        public void InnerProduct_BasisUnits_SquareToMinusOne()
        {
            // Arrange
            var i = Multivector.CreateBaseBlade(6); // e23
            var j = -Multivector.CreateBaseBlade(5); // -e13
            var k = Multivector.CreateBaseBlade(3); // e12

            // Act
            var ii = i | i;
            var jj = j | j;
            var kk = k | k;

            // Assert
            Assert.Equal(-1, ii[0], 10);
            Assert.Equal(-1, jj[0], 10);
            Assert.Equal(-1, kk[0], 10);

            Assert.True(ii.IsScalar());
            Assert.True(jj.IsScalar());
            Assert.True(kk.IsScalar());
        }

        [Fact]
        public void InnerProduct_OrthogonalUnits_ReturnsZero()
        {
            // Arrange
            var i = Multivector.CreateBaseBlade(6); // e23
            var j = -Multivector.CreateBaseBlade(5); // -e13

            // Act
            var result = i | j;

            // Assert
            Assert.True(result.IsZero());
        }

        [Fact]
        public void InnerProduct_GeneralQuaternions_ReturnsCorrectScalar()
        {
            // Arrange
            var q1 = new Multivector(1, 0, 0, 0, 0, 0, 1); // 1 + e23
            var q2 = new Multivector(1, 0, 0, 0, 0, 0, -1); // 1 - e23

            // Act
            var result = q1 | q2;

            // Assert
            // (1+i) | (1-i) = 2
            Assert.Equal(2, result[0], 10);
            Assert.True(result.IsScalar());
        }

        [Fact]
        public void InnerProduct_ScalarAndQuaternion_ReturnsScaledQuaternion()
        {
            // Arrange
            var scalar = new Multivector(2);
            var q = new Multivector(1, 0, 0, 1, 0, 0, 1); // 1 + k + i

            // Act
            var result = scalar | q;

            // Assert
            // 2 | (1 + k + i) = 2 + 2k + 2i
            Assert.Equal(2, result[0], 10); // scalar
            Assert.Equal(2, result[3], 10); // k (e12)
            Assert.Equal(2, result[6], 10); // i (e23)
        }

        [Fact]
        public void InnerProduct_MixedGrades_IsNotAssociative()
        {
            // Arrange
            var i = Multivector.CreateBaseBlade(6); // e23
            var j = -Multivector.CreateBaseBlade(5); // -e13
            
            // Act
            // (i | i) | j = (-1) | j = -j
            var left = (i | i) | j;
            
            // i | (i | j) = i | 0 = 0
            var right = i | (i | j);

            // Assert
            Assert.False(left == right);
            Assert.NotEqual(left[5], right[5]); // Check specific component difference
        }
    }
}