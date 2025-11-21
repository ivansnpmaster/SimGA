namespace SimGA
{
    public partial class Multivector
    {
        /// <summary>
        /// Computes the geometric product of two multivectors.
        /// </summary>
        /// <param name="a">The left operand.</param>
        /// <param name="b">The right operand.</param>
        /// <returns>The result of the geometric product A * B.</returns>
        public static Multivector operator *(Multivector a, Multivector b)
        {
            var resultCoefficients = new double[Algebra.Dimension];

            /*
                The geometric product between two multivectors is calculated as the sum of the geometric products of all combinations of their components. 
                    C = A * B = Σ_i Σ_j (a_i * b_j) * (blade_i * blade_j)
                Where:
                    - a_i, b_j are scalar coefficients
                    - blade_i * blade_j is the geometric product between the blades
                The result is accumulated in the appropriate blade with the correct sign.
            */

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                if (a[i] == 0.0) continue;

                for (int j = 0; j < Algebra.Dimension; j++)
                {
                    if (b[j] == 0.0) continue;

                    /*
                        The resulting blade of the geometric product is given by the XOR of the masks.
                        The XOR (^) operation between the blade indices tells us which blade results:
                            - Bits that are in BOTH operands are zeroed (contraction)
                            - Bits that are in ONLY ONE operand are kept

                        Example:
                            i = 3 (011 = e1e2), j = 5 (101 = e1e3)
                            011 ^ 101 = 110 (e2e3)
                    */
                    int resultBlade = Algebra.GetGeometricProductMask(i, j);

                    /*
                        The sign of the geometric product has already been precomputed during initialization.
                        The sign considers:
                            1. The metric of the vectors (P, Q, R)
                            2. The order of the vectors (non-commutativity)
                    */
                    double sign = Algebra.GetGeometricProductSign(i, j);

                    // The contribution of this specific pair (blade i of A * blade j of B) to
                    // the final result is: sign * coefficientA * coefficientB
                    resultCoefficients[resultBlade] += sign * a[i] * b[j];
                }
            }

            return new Multivector(resultCoefficients);
        }

        /// <summary>
        /// Computes the wedge product (exterior product) of two multivectors.
        /// </summary>
        /// <param name="a">The left operand.</param>
        /// <param name="b">The right operand.</param>
        /// <returns>The result of the wedge product A ^ B.</returns>
        public static Multivector operator ^(Multivector a, Multivector b)
        {
            var resultCoefficients = new double[Algebra.Dimension];

            /*
                The wedge product (exterior product) between two blades is zero if they share any basis vector. Otherwise, it is equal to the geometric product.

                Properties:
                - Anti-commutative: A ^ B = -B ^ A
                - Nilpotent: A ^ A = 0
                - Associative: (A ^ B) ^ C = A ^ (B ^ C)
            */

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                if (a[i] == 0.0) continue;

                for (int j = 0; j < Algebra.Dimension; j++)
                {
                    if (b[j] == 0.0) continue;

                    /*
                        Checks if the blades share basis vectors
                        If (i & j) != 0, they share at least one basis vector
                        In this case, the wedge product is zero
                    */
                    if ((i & j) != 0)
                    {
                        continue;
                    }

                    // For blades without common vectors, the wedge product is equal to the geometric product
                    int resultBlade = Algebra.GetGeometricProductMask(i, j);
                    double sign = Algebra.GetGeometricProductSign(i, j);
                    resultCoefficients[resultBlade] += sign * a[i] * b[j];
                }
            }

            return new Multivector(resultCoefficients);
        }

        /// <summary>
        /// Computes the inner product (contraction) of two multivectors.
        /// </summary>
        /// <param name="a">The left operand.</param>
        /// <param name="b">The right operand.</param>
        /// <returns>The result of the inner product A | B.</returns>
        public static Multivector operator |(Multivector a, Multivector b)
        {
            var resultCoefficients = new double[Algebra.Dimension];

            /*
                The inner product is the lowest grade part of the geometric product. It represents contraction.
                A | B = <AB>_|r-s|  where r = grade(A), s = grade(B)
            */

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                if (a[i] == 0.0) continue;

                for (int j = 0; j < Algebra.Dimension; j++)
                {
                    if (b[j] == 0.0) continue;

                    int gradeA = BladeGrade(i);
                    int gradeB = BladeGrade(j);
                    int targetGrade = Math.Abs(gradeA - gradeB);

                    // Calculate the resulting blade
                    int resultBlade = Algebra.GetGeometricProductMask(i, j);
                    
                    // Check if the resulting blade has the expected grade
                    if (BladeGrade(resultBlade) == targetGrade)
                    {
                        double sign = Algebra.GetGeometricProductSign(i, j);
                        resultCoefficients[resultBlade] += sign * a[i] * b[j];
                    }
                }
            }

            return new Multivector(resultCoefficients);
        }

        /// <summary>
        /// Adds two multivectors.
        /// </summary>
        /// <param name="a">The first multivector.</param>
        /// <param name="b">The second multivector.</param>
        /// <returns>The sum of the two multivectors.</returns>
        public static Multivector operator +(Multivector a, Multivector b)
        {
            var result = new double[Algebra.Dimension];

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                result[i] = a[i] + b[i];
            }

            return new Multivector(result);
        }

        /// <summary>
        /// Negates a multivector.
        /// </summary>
        /// <param name="a">The multivector to negate.</param>
        /// <returns>A new multivector with all coefficients negated.</returns>
        public static Multivector operator -(Multivector a)
        {
            var result = new double[Algebra.Dimension];

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                result[i] = -a[i];
            }

            return new Multivector(result);
        }

        /// <summary>
        /// Subtracts one multivector from another.
        /// </summary>
        /// <param name="a">The multivector to subtract from (minuend).</param>
        /// <param name="b">The multivector to subtract (subtrahend).</param>
        /// <returns>The difference A - B.</returns>
        public static Multivector operator -(Multivector a, Multivector b)
        {
            var result = new double[Algebra.Dimension];

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                result[i] = a[i] - b[i];
            }

            return new Multivector(result);
        }

        /// <summary>
        /// Multiplies a scalar by a multivector.
        /// </summary>
        /// <param name="scalar">The scalar value.</param>
        /// <param name="other">The multivector.</param>
        /// <returns>The result of the scalar multiplication.</returns>
        public static Multivector operator *(double scalar, Multivector other)
        {
            var result = new double[Algebra.Dimension];

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                result[i] = scalar * other[i];
            }

            return new Multivector(result);
        }

        /// <summary>
        /// Multiplies a multivector by a scalar.
        /// </summary>
        /// <param name="mv">The multivector.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>The result of the scalar multiplication.</returns>
        public static Multivector operator *(Multivector mv, double scalar) => scalar * mv;

        /// <summary>
        /// Determines whether two multivectors are equal.
        /// </summary>
        /// <param name="left">The first multivector.</param>
        /// <param name="right">The second multivector.</param>
        /// <returns><c>true</c> if the multivectors are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Multivector? left, Multivector? right) => Equals(left, right);

        /// <summary>
        /// Determines whether two multivectors are not equal.
        /// </summary>
        /// <param name="left">The first multivector.</param>
        /// <param name="right">The second multivector.</param>
        /// <returns><c>true</c> if the multivectors are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Multivector? left, Multivector? right) => !Equals(left, right);

        /// <summary>
        /// Determines whether the specified object is equal to the current multivector.
        /// </summary>
        /// <param name="obj">The object to compare with the current multivector.</param>
        /// <returns><c>true</c> if the specified object is equal to the current multivector; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not Multivector other)
            {
                return false;
            }

            const double tolerance = 1e-10;

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                if (Math.Abs(_coefficients[i] - other._coefficients[i]) > tolerance)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current multivector.</returns>
        public override int GetHashCode()
        {
            // Combines all coefficients into a deterministic sequence
            var hash = new HashCode();

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                hash.Add(_coefficients[i]);
            }

            return hash.ToHashCode();
        }
    }
}