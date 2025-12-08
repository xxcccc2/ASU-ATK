# Performance Infrastructure

The performance infrastructure focuses on `System.Buffers` for memory pooling and `System.Runtime.CompilerServices.Unsafe` for low-level unsafe operations. This infrastructure aims to optimize performance by reducing garbage collection pressure and enabling direct memory manipulation.

## System.Buffers for Memory Pooling

The `System.Buffers` namespace provides `ArrayPool<T>`, which is used for reusing array instances to minimize allocations and garbage collection overhead. The `ArrayPool<T>.Shared` property provides a thread-safe shared instance for general use.

### Core API and Usage
You can obtain an array using the `Rent(int minimumLength)` method, which returns an array at least as long as requested. After use, the array should be returned to the pool using `Return(T[] array, bool clearArray)`. The `clearArray` parameter determines if the array's contents are cleared before being stored for reuse, which is crucial for preventing data leakage.

### Integration
`ArrayPool<T>` is utilized by the `RBush` R-tree spatial index for node allocation, query result buffers, and split operation buffers. It's also used by the NPOI Excel library for stream buffering, cell data caching, and compression buffers.

## System.Runtime.CompilerServices.Unsafe for Low-Level Operations

The `System.Runtime.CompilerServices.Unsafe` class provides low-level functionalities for pointer manipulation without requiring an `unsafe` context in C#. This enables bypassing bounds checking, type reinterpretation, and direct memory operations for performance-critical code.

### Operation Categories
The `Unsafe` class offers various operations, including:
* **Pointer Arithmetic**: Methods like `Add<T>` and `Subtract<T>` allow adding or subtracting element offsets from references, automatically accounting for `sizeof(T)`. `AddByteOffset<T>` and `SubtractByteOffset<T>` operate at the raw memory level using byte offsets.
* **Type Reinterpretation**: `As<TFrom, TTo>` reinterprets a reference as a different type, performing a bitwise reinterpretation without conversion. `AsPointer<T>` returns a pointer to a by-ref parameter, and `AsRef<T>` reinterprets a pointer location as a typed reference.
* **Memory Operations**: This includes `Read<T>` and `Write<T>` for single value operations, and `CopyBlock` and `InitBlock` for block operations. Unaligned versions like `ReadUnaligned<T>` and `CopyBlockUnaligned` are available for cases where memory alignment cannot be guaranteed.
* **Reference Comparison**: Methods like `AreSame<T>`, `IsAddressGreaterThan<T>`, and `IsAddressLessThan<T>` compare memory addresses.
* **Utility Operations**: `SizeOf<T>` returns the size of a type in bytes, and `Unbox<T>` returns a mutable reference to a boxed value.

### Safety Considerations
Using `System.Runtime.CompilerServices.Unsafe` carries risks of undefined behavior, such as type reinterpretation mismatches, out-of-bounds access, alignment violations, and lifetime violations. Mitigation strategies include using unaligned operations when necessary, validating offsets, ensuring object lifetimes, and encapsulating unsafe operations in well-tested helper methods.

### Integration
Numerical computing subsystems like `MathNet.Numerics` and `ExtendedNumerics.BigDecimal` leverage `Unsafe` for direct memory access in matrix operations, vectorized computations, and arbitrary precision arithmetic.

## Performance Optimization Patterns
The codebase employs specific patterns for performance optimization:
* **Rent-Use-Return**: This pattern involves renting a buffer from `ArrayPool<T>.Shared`, using it for temporary operations, and then returning it to the pool. This minimizes allocations in hot paths.
* **Type Punning with Unsafe**: This pattern reinterprets memory as different types for performance-critical conversions, such as converting an `int32` to a `float` using `Unsafe.As<int, float>`.
* **SIMD-Style Iterations**: Pointer arithmetic with `Unsafe` methods like `Unsafe.Add` and `Unsafe.IsAddressLessThan` enables loop unrolling and SIMD-like patterns, eliminating array bounds checks in hot loops.

## Notes
The `System.ValueTuple` package is also part of the performance infrastructure, providing `TupleExtensions` for deconstruction methods to ensure interoperability between `Tuple<...>` and `ValueTuple<...>`, supporting modern C# tuple syntax. While related to performance, it focuses on data structure interoperability rather than direct memory or buffer management.

---
*Source: [DeepWiki Search](https://deepwiki.com/search/performance-infrastructure-sys_d3b5c4cb-2682-4a6b-b6bc-5aad44eb82f1)*


---

## System.Buffers Array Pooling

The `ArrayPool<T>` class provides a mechanism for pooling and reusing arrays of type `T[]`. This helps to minimize memory allocations and reduce the frequency of garbage collection, which is crucial for performance-critical operations. The `ArrayPool<T>` implementation groups arrays into "buckets" based on their lengths to facilitate faster retrieval.

### Core API

The primary methods and properties for interacting with `ArrayPool<T>` are:

* **`ArrayPool<T>.Shared`**: This static property returns a thread-safe, shared instance of `ArrayPool<T>` suitable for general application use.
* **`Create()`**: This method creates a new, independent instance of `ArrayPool<T>` with default configuration.
* **`Create(int maxArrayLength, int maxArraysPerBucket)`**: This method allows you to create a new `ArrayPool<T>` instance with custom configurations for the maximum array length it can store and the maximum number of arrays per bucket.
* **`Rent(int minimumLength)`**: This method retrieves an array from the pool that is at least `minimumLength` long. The returned array might be larger than the requested minimum length.
* **`Return(T[] array, bool clearArray)`**: This method returns a previously rented array to the pool. The `clearArray` parameter determines whether the array's contents are cleared (zeroed out) before being made available for reuse. Setting `clearArray` to `true` prevents data leakage but incurs a performance cost.

### Usage Pattern

The typical usage pattern for `ArrayPool<T>` involves renting an array for temporary use and then returning it to the pool when no longer needed. This "Rent-Use-Return" pattern helps to reduce allocations in performance-critical code paths.

### Integration with Application Components

`ArrayPool<T>` is integrated into various components within the `TransportCompany` application to optimize memory usage:

* **Spatial Indexing**: The `RBush` R-tree spatial index uses `ArrayPool<T>` for node allocation during tree traversal, query result buffers, and intermediate storage during node split operations.
* **Document Processing**: The NPOI Excel library utilizes `System.Buffers` for stream buffering when reading/writing Excel files, caching cell data, and for compression buffers in XLSX files.

---

## Pointer Arithmetic and Memory Access Patterns

The `System.Runtime.CompilerServices.Unsafe` class offers methods for both element-level and byte-level pointer arithmetic.

### Element-Level Offsets
Element-level offset methods, such as `Unsafe.Add<T>` and `Unsafe.Subtract<T>`, automatically multiply the provided offset by `sizeof(T)`. For example, `Add(ref myStruct, 3)` would advance the pointer by `3 * sizeof(MyStruct)` bytes. These methods are available for both references (`ref T`) and void pointers (`void*`).

### Byte-Level Offsets
For more granular control, `Unsafe.AddByteOffset<T>` and `Unsafe.SubtractByteOffset<T>` allow adding or subtracting a specified number of bytes directly. The `Unsafe.ByteOffset<T>` method can compute the byte difference between two references. These operations are useful when precise memory layout control is necessary.

### Memory Access Patterns

The `Unsafe` class facilitates various memory access patterns, including type reinterpretation and block operations.

#### Type Reinterpretation
Methods like `Unsafe.As<TFrom, TTo>` allow reinterpreting a reference of one type as a reference to another type without actual type conversion. This is a form of "type punning" used for performance-critical conversions, but it requires compatible memory layouts to avoid undefined behavior. Additionally, `Unsafe.AsPointer<T>` converts a reference to a `void*` pointer, and `Unsafe.AsRef<T>` can convert a `void*` back to a typed reference or reinterpret a read-only reference as mutable.

#### Single Value Operations
The `Unsafe` class provides methods to `Read<T>` and `Write<T>` single values from/to memory locations specified by `void*` pointers. For situations where memory alignment cannot be guaranteed, `ReadUnaligned<T>` and `WriteUnaligned<T>` are available.

#### Block Operations
For bulk memory manipulation, `Unsafe.CopyBlock` and `Unsafe.InitBlock` are used for copying and initializing blocks of memory, respectively. These "aligned operations" assume that memory addresses meet architecture-dependent alignment requirements. If alignment is not guaranteed, `Unsafe.CopyBlockUnaligned` and `Unsafe.InitBlockUnaligned` should be used, which handle arbitrary byte alignments safely, albeit with potential performance costs.

### Safety Considerations

Using `System.Runtime.CompilerServices.Unsafe` introduces risks of undefined behavior, including type reinterpretation mismatches, out-of-bounds access, alignment violations, lifetime violations, and null pointer dereferences. Mitigation strategies include using unaligned operations when necessary, validating offsets, ensuring object lifetimes, and encapsulating unsafe code within well-tested helper methods.

### Usage in Application Components

The `TransportCompany` application utilizes `Unsafe` for performance-critical operations in numerical computing and other subsystems. For instance, `MathNet.Numerics` and `ExtendedNumerics.BigDecimal` use direct memory access for matrix operations, vectorized computations, and arbitrary precision arithmetic. A common pattern involves using pointer arithmetic to iterate through arrays without bounds checking, as shown in the SIMD-style iterations example.

---

## ValueTuple Extensions and Tuple Interoperability

The `System.ValueTuple` package provides the `TupleExtensions` class, which offers methods to facilitate conversions between the value-type `ValueTuple` and the reference-type `Tuple`. This is crucial for integrating modern C# tuple syntax with older APIs that might return `Tuple` objects.

### `Deconstruct` Methods

The `TupleExtensions` class includes `Deconstruct` extension methods for tuples with up to 21 elements. These methods allow you to extract the elements of a `Tuple` into individual `out` parameters, enabling the use of C# deconstruction syntax.

For example, a 2-element `Tuple` can be deconstructed using a method with the signature `void Deconstruct<T1, T2>(this Tuple<T1, T2> value, out T1 item1, out T2 item2)`. For tuples with more than 7 elements, the `Tuple` type uses a nested structure (e.g., `Tuple<T1...T7, TRest>`). The `Deconstruct` methods automatically handle this nesting, allowing seamless deconstruction of larger tuples.

### Conversion Methods: `ToTuple` and `ToValueTuple`

The `TupleExtensions` class also provides explicit conversion methods:
* **`ToTuple`**: These methods convert an instance of a `ValueTuple` structure to an instance of the `Tuple` class. There are multiple overloads for `ToTuple`, supporting `ValueTuple` instances with varying numbers of elements.
* **`ToValueTuple`**: These methods convert an instance of a `Tuple` class to an instance of the `ValueTuple` structure. Similar to `ToTuple`, there are multiple overloads to handle `Tuple` instances with different numbers of elements, including those with nested `Tuple` structures for more than 7 elements.

These conversion methods are essential for maintaining compatibility and allowing data to flow between codebases that might use different tuple representations.

---
*Source: [DeepWiki - Array Pooling](https://deepwiki.com/search/systembuffers-array-pooling-co_493d3788-5d58-4be3-ac00-f81c07be2f62)*
*Source: [DeepWiki - Pointer Arithmetic](https://deepwiki.com/search/pointer-arithmetic-and-memory_7b79e6f0-ecaa-4d3a-8afe-d2424b6342ca)*
*Source: [DeepWiki - ValueTuple](https://deepwiki.com/search/valuetuple-extensions-and-tupl_e5a2587b-8539-41db-b4eb-9d6561c8a7ea)*
