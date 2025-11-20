namespace SimGA
{
    public partial class Multivector
    {
        /// <summary>
        /// Geometric product of multivectors
        /// </summary>
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
        /// Wedge product (exterior product) of multivectors
        /// </summary>
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

        public static Multivector operator +(Multivector a, Multivector b)
        {
            var result = new double[Algebra.Dimension];

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                result[i] = a[i] + b[i];
            }

            return new Multivector(result);
        }

        public static Multivector operator -(Multivector a)
        {
            var result = new double[Algebra.Dimension];

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                result[i] = -a[i];
            }

            return new Multivector(result);
        }

        public static Multivector operator -(Multivector a, Multivector b)
        {
            var result = new double[Algebra.Dimension];

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                result[i] = a[i] - b[i];
            }

            return new Multivector(result);
        }

        public static Multivector operator *(double scalar, Multivector other)
        {
            var result = new double[Algebra.Dimension];

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                result[i] = scalar * other[i];
            }

            return new Multivector(result);
        }

        public static Multivector operator *(Multivector mv, double scalar) => scalar * mv;

        public static bool operator ==(Multivector? left, Multivector? right) => Equals(left, right);

        public static bool operator !=(Multivector? left, Multivector? right) => !Equals(left, right);

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