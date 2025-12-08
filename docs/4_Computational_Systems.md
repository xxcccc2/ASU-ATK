# Computational and Mathematical Systems

The TransportCompany application utilizes the `ExtendedNumerics.BigDecimal` library for arbitrary-precision decimal arithmetic, which is crucial for exact financial calculations such as cost calculations and currency conversions. This library is part of a broader computational architecture that also includes `MathNet.Numerics` for linear algebra and optimization, and `RBush` for spatial indexing.

## ExtendedNumerics.BigDecimal Library

The `ExtendedNumerics.BigDecimal` library provides arbitrary-precision decimal arithmetic, which is essential for financial calculations to avoid floating-point rounding errors. It represents numbers using a mantissa-exponent system, where both the mantissa and exponent can have arbitrary precision.

### Precision Management
The library offers global configuration options for precision management:
* `Precision`: Defines the number of significant digits for operations.
* `AlwaysTruncate`: A boolean flag that, when `true`, truncates after each operation, which can accumulate rounding errors but prevents memory growth for irrational numbers. When `false` (recommended), precision can grow during calculations, and truncation should be done explicitly via `Round()`.
* `AlwaysNormalize`: A boolean flag that, when `true` (default), removes trailing zeros and adjusts the exponent for a canonical representation.

### Mathematical Operations
The `BigDecimal` class supports a wide range of mathematical operations, including:
* **Basic Arithmetic**: `Add`, `Subtract`, `Multiply`, `Divide`, `Mod`, `Negate`, `Abs`.
* **Trigonometric Functions**: `Sin`, `Cos`, `Tan`, `Cot`, `Sec`, `Csc`, and their inverse counterparts (`Arcsin`, `Arccos`, `Arctan`, `Arccot`, `Arccsc`). These are implemented using Taylor series expansions.
* **Hyperbolic Functions**: `Sinh`, `Cosh`, `Tanh`, `Coth`, `Sech`, `Csch`.
* **Logarithmic and Exponential Functions**: `Exp`, `Ln`, `Log2`, `Log10`, and `LogN` for arbitrary bases.
* **Power and Root Operations**: `Pow` (with `Pow_Fast` for performance and `Pow_Precision` for accuracy based on `AlwaysTruncate` setting) and `NthRoot`.
* **Rounding Operations**: `Truncate`, `Floor`, `Ceiling`, and `Round` (with various overloads for specific rounding modes and decimal places).

### Utility Methods
The library also provides utility methods such as `GetDecimalIndex()`, `GetWholePart()`, `GetFractionalPart()`, `PlacesLeftOfDecimal()`, `PlacesRightOfDecimal()`, `Normalize()`, and `AlignExponent()`.

### Parsing and Conversion
`BigDecimal` supports parsing from strings and doubles, and conversion to various numeric types like `double`, `float`, `decimal`, `int`, and `uint`.

## Algorithms and Use Cases
The `BigDecimal` library is critical for financial precision in the TransportCompany application, particularly for cost calculations and currency conversions, where standard `double` precision is insufficient and can lead to cumulative rounding errors. For example, in route optimization, `BigDecimal` is used to compute exact costs, complementing `MathNet.Numerics` for optimization algorithms and `RBush` for spatial queries.

---
*Source: [DeepWiki Search](https://deepwiki.com/search/computational-and-mathematical_88c7432e-6525-4208-85ba-328bd02e8be4)*


---

## Trigonometric and Logarithmic Operations

The `TransportCompany` application utilizes the `ExtendedNumerics.BigDecimal` library for arbitrary-precision trigonometric and logarithmic operations. This library provides a comprehensive set of functions for both standard and inverse trigonometric calculations, as well as various logarithmic and exponential functions.

### Trigonometric Functions

The `BigDecimal` class offers arbitrary precision for standard trigonometric functions such as `Sin`, `Cos`, `Tan`, `Cot`, `Sec`, and `Csc`. These functions are implemented using Taylor series expansions to achieve arbitrary precision. Each function has two overloads: one that uses the global `Precision` setting and another that accepts an explicit `precision` parameter.

#### Standard Trigonometric Functions
The standard trigonometric functions available include:
* `Sin(BigDecimal radians)` and `Sin(BigDecimal radians, int precision)`
* `Cos(BigDecimal radians)` and `Cos(BigDecimal radians, int precision)`
* `Tan(BigDecimal radians)` and `Tan(BigDecimal radians, int precision)`
* `Cot(BigDecimal radians)` and `Cot(BigDecimal radians, int precision)`
* `Sec(BigDecimal radians)` and `Sec(BigDecimal radians, int precision)`
* `Csc(BigDecimal radians)` and `Csc(BigDecimal radians, int precision)`

Some functions have input constraints, for example, `Tan` is undefined at π/2 or 3π/2, and `Cot` is undefined at zero.

#### Inverse Trigonometric Functions
The library also supports inverse trigonometric functions:
* `Arcsin(BigDecimal radians)` and `Arcsin(BigDecimal radians, int precision)`
* `Arccos(BigDecimal radians)` and `Arccos(BigDecimal radians, int precision)`
* `Arctan(BigDecimal radians)` and `Arctan(BigDecimal radians, int precision)`
* `Arccot(BigDecimal radians)` and `Arccot(BigDecimal radians, int precision)`
* `Arccsc(BigDecimal radians)` and `Arccsc(BigDecimal radians, int precision)`

#### Hyperbolic Functions
Hyperbolic functions are also available with arbitrary precision:
* `Sinh(BigDecimal radians)` and `Sinh(BigDecimal radians, int precision)`
* `Cosh(BigDecimal radians)` and `Cosh(BigDecimal radians, int precision)`
* `Tanh(BigDecimal radians)` and `Tanh(BigDecimal radians, int precision)`
* `Coth(BigDecimal radians)` and `Coth(BigDecimal radians, int precision)`
* `Sech(BigDecimal radians)` and `Sech(BigDecimal radians, int precision)`
* `Csch(BigDecimal radians)` and `Csch(BigDecimal radians, int precision)` - The `Csch` function requires that the input not be zero.

### Logarithmic and Exponential Functions

The `BigDecimal` library provides functions for exponential and various logarithmic calculations.

#### Exponential Functions
The exponential function `Exp` calculates e^x to arbitrary precision. It has overloads for using the global precision or specifying an explicit precision.

#### Logarithmic Functions
The logarithmic functions include:
* `Ln(BigDecimal argument)` and `Ln(BigDecimal argument, int precision)` for natural logarithm.
* `Log2(BigDecimal argument, int precision)` for base-2 logarithm.
* `Log10(BigDecimal argument, int precision)` for base-10 logarithm.
* `LogN(int base, BigDecimal argument, int precision)` for arbitrary base logarithm.

All logarithmic functions internally call `LogNatural()` which implements the natural logarithm using Taylor series.

---

## Helper Classes and Optimization Algorithms

The `ExtendedNumerics.BigDecimal` library includes helper classes, such as `ExtendedNumerics.Helpers.TrigonometricHelper`, which provides functions for calculating Taylor Series sums to achieve specified precision for trigonometric and other functions. For instance, `TaylorSeriesSum` is a general function that can approximate `sin`, `cos`, `sinh`, `cosh`, and `exp` functions. Another helper, `ExtendedNumerics.Helpers.BigIntegerHelper.FastFactorial`, is used for efficient factorial calculations, which are crucial for arbitrary-precision trigonometric functions. This `FastFactorial` method uses a divide and conquer approach to optimize factorial computation.

### Optimization Algorithms

Optimization algorithms are primarily provided by the `MathNet.Numerics` library. This library offers capabilities for linear algebra, statistics, and optimization, which are used for tasks like route optimization. Specifically, for route optimization, `MathNet.Numerics` includes algorithms for solving problems such as the Traveling Salesman Problem (TSP) and Vehicle Routing Problem (VRP), and also supports linear programming. The `MathNet.Numerics` library also integrates with `System.Numerics.Vectors` for SIMD (Single Instruction Multiple Data) operations, which enhances performance by processing multiple data points simultaneously.

### Application Use Cases

In the `TransportCompany` application, these helper classes and optimization algorithms are integral to several critical functions:
* **Route Optimization**: `MathNet.Numerics` optimization algorithms are used to find near-optimal routes, while `ExtendedNumerics.BigDecimal` ensures precise cost calculations.
* **Financial Precision**: `ExtendedNumerics.BigDecimal` is essential for exact financial calculations, eliminating floating-point rounding errors in operations like cost calculations and currency conversions.

---
*Source: [DeepWiki - Trigonometric Operations](https://deepwiki.com/search/trigonometric-and-logarithmic_4f38d1ca-f04a-4126-92bf-609cdfa58d04)*
*Source: [DeepWiki - Helper Classes](https://deepwiki.com/search/helper-classes-and-optimizatio_c65d0437-9168-4389-a016-5c08acc45078)*
