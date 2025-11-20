# SimGA - Simple Geometric Algebra

A .NET 8 library for Geometric Algebra computation with arbitrary $(p,q,r)$ signature.

## Features
- Multivector operations: addition, subtraction, geometric product (`*`), wedge product (`^`), inner product (`|`), negation
- Support for custom algebra signatures: Euclidean, pseudo-Euclidean, degenerate
- Dense array representation for all blades, indexed by bitwise basis vector presence
- Operator overloading for natural C# syntax
- Grade projection and blade extraction
- Human-readable output for multivectors
- Unit tests ![✓](https://img.shields.io/badge/100/100%20passing-brightgreen)

## Internal representation
Multivectors are stored as arrays of coefficients, indexed by the blade's bit pattern. For $\mathcal{C}\ell(3,0,0)$:

| Index | Binary | Blade     | Description    |
|-------|--------|-----------|----------------|
| 0     | 000    | $1$       | Scalar         |
| 1     | 001    | $e_1$     | Basis vector 1 |
| 2     | 010    | $e_2$     | Basis vector 2 |
| 3     | 011    | $e_{12}$  | Bivector       |
| 4     | 100    | $e_3$     | Basis vector 3 |
| 5     | 101    | $e_{13}$  | Bivector       |
| 6     | 110    | $e_{23}$  | Bivector       |
| 7     | 111    | $e_{123}$ | Trivector      |

Each blade is uniquely identified by its index, with each bit representing the presence (1) or absence (0) of a basis vector.

## Example usage

> [!IMPORTANT]
> Before creating or operating on multivectors, you must set the algebra signature using `Algebra.Set(p, q, r)`, where:
> - `p` = number of basis vectors that square to +1
> - `q` = number of basis vectors that square to -1
> - `r` = number of basis vectors that square to 0 (degenerate)

### Geometric product (`*`)

**Example in 3D Euclidean algebra:**
```csharp
using SimGA;

Algebra.Set(3, 0, 0);

var e1 = Multivector.CreateBaseBlade(1);
var e2 = Multivector.CreateBaseBlade(2);

var result = e1 * e2; // e12 (bivector)
```

**Example with quaternions:**
```csharp
using SimGA;

Algebra.Set(0, 3, 0); // Quaternions: even subalgebra of Cl(0, 3, 0)

var i = Multivector.CreateBaseBlade(6); // e23
var j = -Multivector.CreateBaseBlade(5); // -e13
var k = Multivector.CreateBaseBlade(3); // e12

var ii = i * i; // -1
var ij = i * j; // k
var ijk = i * j * k; // -1
```

### Wedge product (`^`)

**Example in 3D Euclidean algebra:**
```csharp
using SimGA;

Algebra.Set(3, 0, 0);

var e1 = Multivector.CreateBaseBlade(1);
var e2 = Multivector.CreateBaseBlade(2);
var e3 = Multivector.CreateBaseBlade(4);

var bivector = e1 ^ e2; // e12
var trivector = e1 ^ e2 ^ e3; // e123 (pseudoscalar)
var zero = e1 ^ e1; // 0 (antisymmetric)
```

**Example detecting linear dependence:**
```csharp
using SimGA;

Algebra.Set(3, 0, 0);

var v1 = new Multivector(0, 1, 2); // e1 + 2*e2
var v2 = new Multivector(0, 2, 4); // 2*e1 + 4*e2 (parallel to v1)

var result = v1 ^ v2; // 0 (linearly dependent)
```

### Inner product (`|`)

**Example with 3D vectors:**
```csharp
using SimGA;

Algebra.Set(3, 0, 0);

var v1 = new Multivector(0, 1, 2, 0, 3); // e1 + 2*e2 + 3*e3
var v2 = new Multivector(0, 4, 5, 0, 6); // 4*e1 + 5*e2 + 6*e3

var dot = v1 | v2; // 32 (scalar: 1*4 + 2*5 + 3*6)
```

**Example with complex numbers:**
```csharp
using SimGA;

Algebra.Set(0, 2, 0); // Complex numbers: even subalgebra of Cl(0, 2, 0)

var z = new Multivector(3, 0, 0, 4); // 3 + 4i
var zConj = new Multivector(3, 0, 0, -4); // 3 - 4i

var norm = z | zConj; // 25 (modulus squared)
```

**Example with quaternions:**
```csharp
using SimGA;

Algebra.Set(0, 3, 0); // Quaternions: even subalgebra of Cl(0, 3, 0)

var i = Multivector.CreateBaseBlade(6); // e23
var j = -Multivector.CreateBaseBlade(5); // -e13

var ii = i | i; // -1
var ij = i | j; // 0 (orthogonal)
```

## Main API
- `Algebra.Set(byte p, byte q, byte r)` - Set algebra signature (p positive, q negative, r degenerate)
- `Multivector(params double[] coefficients)` - Create a multivector
- `Multivector.CreateBaseBlade(int index)` - Create a basis blade
- `GradeProjection(int k)` - Extract homogeneous part of grade $k$
- `ToString()` - Human-readable output
- Operators: `+`, `-`, `*` (geometric), `^` (wedge), `|` (inner)

## Testing
```bash
dotnet test
```