# Data Validation and Type Safety

The `TransportCompany` application utilizes `System.ComponentModel.Annotations` for its data validation infrastructure.

## Data Validation System

The `TransportCompany` application implements a data validation system that uses declarative attributes from `System.ComponentModel.Annotations` to enforce data integrity rules. This system is crucial for preventing invalid data from corrupting business operations.

### Core Components

The validation pipeline involves several key components:
* **Parsing & Conversion**: Initial processing of external input.
* **`System.ComponentModel.Annotations`**: The primary library used for validation.
* **Validation Attributes**: These include attributes like `Required`, `Range`, `StringLength`, and `RegularExpression`.
* **`ValidationContext`**: Describes the context in which a validation check is performed.
* **`ValidationResult`**: Represents the result of a validation.

### Validation Process

The validation process flows as follows:
1. External input is received.
2. The input undergoes parsing and conversion.
3. `System.ComponentModel.Annotations` is used to apply validation attributes.
4. A `ValidationContext` is created to manage the validation process.
5. The validation results are collected in a `ValidationResult` object.
6. If valid, the data is used to update business entities (e.g., `Route`, `Vehicle`, `Driver`, `Customer`, `Shipment`).
7. Finally, the validated data is persisted to the SQL Database.

### `Validator` Class

The `System.ComponentModel.DataAnnotations.Validator` class is a helper class used to validate objects, properties, and methods based on their associated `ValidationAttribute` attributes.

Key methods of the `Validator` class include:
* `TryValidateObject`: Determines if an object is valid, collecting all failed validations.
* `TryValidateProperty`: Validates a specific property of an object.
* `TryValidateValue`: Checks if a value is valid against specified attributes.
* `ValidateObject`: Validates an object and throws a `ValidationException` if invalid.
* `ValidateProperty`: Validates a property and throws a `ValidationException` if invalid.
* `ValidateValue`: Validates a value against specified attributes and throws a `ValidationException` if invalid.

### Data Types and Custom Validation

The `System.ComponentModel.DataAnnotations.DataType` enumeration provides various data types for validation, such as `CreditCard`, `Currency`, `Date`, `DateTime`, `EmailAddress`, `PhoneNumber`, and `PostalCode`. Additionally, custom validation methods can be specified using the `CustomValidationAttribute`.

## Type Safety

The use of `System.ComponentModel.Annotations` contributes to type safety by ensuring that data conforms to predefined rules and types before being processed or stored. This framework helps maintain the integrity and correctness of data throughout the application.

---
*Source: [DeepWiki Search](https://deepwiki.com/search/data-validation-and-type-safet_156e3e35-541e-4799-97b5-8bdd5b71a66a)*


---

## System.ComponentModel.Annotations Framework

The `System.ComponentModel.Annotations` framework is primarily used in the `TransportCompany` application for data validation. It provides a set of attributes that can be applied to properties of business entities to enforce data integrity rules. This framework is a core dependency for the application's data validation infrastructure.

### Validation Attributes
The framework includes attributes such as:
* `AssociationAttribute`: Specifies that an entity member represents a data relationship, like a foreign key.
* `CompareAttribute`: Compares two properties.
* `DataTypeAttribute`: Specifies the data type of a data field.
* `EmailAddressAttribute`: Validates an email address.
* `EnumDataTypeAttribute`: Maps a .NET Framework enumeration type to a data column.
* `FileExtensionsAttribute`: Validates file name extensions.
* `KeyAttribute`: Denotes properties that uniquely identify an entity.
* `MaxLengthAttribute`: Specifies the maximum length of array or string data.
* `MinLengthAttribute`: Specifies the minimum length of array or string data.
* `UrlAttribute`: Provides URL validation.
* `ValidationAttribute`: Serves as the base class for all validation attributes.

### Display and UI Hint Attributes
The framework also includes attributes for controlling UI display:
* `DisplayAttribute`: Provides localizable strings for types and members, and controls automatic UI generation for fields and filtering.
* `DisplayFormatAttribute`: Specifies how data fields are displayed and formatted in ASP.NET Dynamic Data.
* `EditableAttribute`: Indicates whether a data field is editable.
* `FilterUIHintAttribute`: Specifies the user control to use for filtering data fields.
* `UIHintAttribute`: Specifies the template or user control Dynamic Data uses to display a data field.

### Schema Attributes
Attributes for database schema mapping are also present:
* `ColumnAttribute`: Represents a database column corresponding to a property.
* `ComplexTypeAttribute`: Indicates that a class is a complex type.
* `DatabaseGeneratedAttribute`: Specifies how a property's value is generated in the database.

---

## Entity Relationships and Schema Attributes

The TransportCompany application utilizes attributes from `System.ComponentModel.DataAnnotations.Schema` to define entity relationships and schema attributes for its SQL database. These attributes allow for declarative configuration of how classes and their properties map to database tables and columns, including defining foreign keys, inverse properties, and database-generated values.

### Entity Relationship Attributes

The following attributes are used to define relationships between entities:
* **`ForeignKeyAttribute`**: This attribute denotes a property used as a foreign key in a relationship. It can be placed on either the foreign key property, specifying the associated navigation property name, or on the navigation property, specifying the associated foreign key name(s).
* **`InversePropertyAttribute`**: This attribute specifies the inverse of a navigation property, indicating the other end of the same relationship. It is initialized with the name of the navigation property representing the other end of the relationship.

### Schema Attributes

Attributes for defining schema characteristics include:
* **`TableAttribute`**: This attribute specifies the database table that a class is mapped to. It can be initialized with the name of the table and optionally specify the schema.
* **`ColumnAttribute`**: This attribute represents a database column corresponding to a property. It allows specifying the column's name, order, and database provider-specific data type.
* **`NotMappedAttribute`**: This attribute indicates that a property or class should be excluded from database mapping.
* **`DatabaseGeneratedAttribute`**: This attribute specifies how the database generates values for a property. It uses the `DatabaseGeneratedOption` enumeration to define the generation pattern.
  * **`DatabaseGeneratedOption.Identity`**: The database generates a value when a row is inserted.
  * **`DatabaseGeneratedOption.Computed`**: The database generates a value when a row is inserted or updated.
  * **`DatabaseGeneratedOption.None`**: The database does not generate values for the property.
* **`ComplexTypeAttribute`**: This attribute denotes that a class is a complex type, which are non-scalar properties of entity types used to organize scalar properties within entities. Complex types do not have keys and cannot be managed independently by the Entity Framework.

---

## Enums.NET Type-Safe Enum Operations

The `Enums.NET` library provides high-performance, boxing-free enum operations, which are crucial for type safety in the `TransportCompany` application, especially where enum values represent states, types, and categories.

### Core Components of Enums.NET

* **`EnumComparer<TEnum>`**: This class enables efficient, boxing-free comparison of enum values, which is important for performance-critical operations. It provides methods like `Equals(TEnum x, TEnum y)` and `GetHashCode(TEnum obj)` for comparisons and hash code generation without the overhead of boxing.
* **`EnumFormat`**: This enumeration defines various formats for representing enum values as strings, such as `DecimalValue`, `HexadecimalValue`, `Name`, `Description`, `EnumMemberValue`, and `DisplayName`. Multiple formats can be combined, with a fallback mechanism if the primary format returns null.
* **`EnumMember`**: This struct provides access to an enum member's value, name, and attributes. It allows for retrieving attributes using methods like `Has<TAttribute>()` and `Get<TAttribute>()`.
* **`Enums` (static class)**: This static class offers a comprehensive API for enum operations, including retrieving members, names, and values using `GetMembers<TEnum>`, `GetNames<TEnum>`, and `GetValues<TEnum>`. It also provides methods for converting values to enums (`ToObject<TEnum>`) and parsing string representations (`Parse<TEnum>`, `TryParse<TEnum>`).
* **`EnumValidation`**: This enumeration specifies the type of validation to perform when converting or parsing enum values, including `None`, `Default`, `IsDefined`, and `IsValidFlagCombination`.

### Enum Validation and Parsing

The `Enums.NET` library facilitates robust enum validation and parsing. For instance, the `Enums.TryParse<TEnum>` method attempts to convert a string representation into an enum value, checking for format and validity. The `Enums.ToObject<TEnum>` method converts various value types to an enum, with an option to specify `EnumValidation` to ensure the resulting enum value is defined or a valid flag combination.

### Performance Characteristics

`Enums.NET` is designed for performance, offering significant improvements over standard .NET enum operations by avoiding boxing. This is achieved through internal caching of enum metadata and singleton patterns for `EnumComparer<TEnum>`, ensuring efficient lookups and operations after the first access.

---
*Source: [DeepWiki - System.ComponentModel.Annotations](https://deepwiki.com/search/systemcomponentmodelannotation_4ff8677b-d8aa-4204-b605-6a87cce0ed6c)*
*Source: [DeepWiki - Entity Relationships](https://deepwiki.com/search/entity-relationships-and-schem_0f136f86-0339-4716-96af-bda6668c4951)*
*Source: [DeepWiki - Enums.NET](https://deepwiki.com/search/enumsnet-typesafe-enum-operati_3a878501-5cc9-4567-9f3c-16bd5b7a835f)*
