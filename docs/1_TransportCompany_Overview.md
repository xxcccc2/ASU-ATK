# TransportCompany Application Overview

The `TransportCompany` application is a Windows Forms application designed to manage various aspects of a transport company's operations, including vehicle maintenance, driver information, and financial tracking. It interacts with a SQL Server database named `TransportCompany` to store and retrieve data. The application provides a `MainForm` as its primary user interface, from which users can access different functionalities like technical inspections, fleet diary, data import, and various reports.

## Application Startup and Notifications

Upon launching, the application performs several checks and displays notifications to the user.

### OSAGO and License Expiry Checks
The application checks for expiring OSAGO (compulsory motor third-party liability insurance) policies and driver's licenses by querying the database.
If any OSAGO policies or licenses are found to be expiring, a message is constructed with details such as vehicle registration number, policy number, driver's full name, and expiry dates.

### Technical Inspection (TO) Overdue Check
The application also checks for vehicles with overdue technical inspections. A vehicle is considered to require maintenance if its last technical inspection date plus three months is earlier than the current date. If overdue inspections are found, a message listing the vehicle numbers and their last inspection dates is added to the overall notification.

All these notifications are combined and displayed in a single message box if there are any alerts to show. After displaying notifications, the application proceeds to run the `MainForm`.

## Main Form Functionality

The `MainForm` provides access to various modules of the application.

### Technical Inspection (ТО) Management
Users can open a dedicated form for technical inspections by clicking the "Техосмотр" button. This action triggers the `btnTO_Click` event handler, which creates and displays a `ТОForm1` instance. After the `ТОForm1` is closed, the application re-checks for overdue technical inspections.

### Fleet Diary
The "Дневник Автопарка" button opens the `FleetDiaryForm`. The `button1_Click` event handler is responsible for showing this form.

### Data Import
The application allows importing data through the `FormImport`. The `btnImportData_Click` event handler opens this form. The `FormImport` handles reading data from external sources and inserting it into the database, specifically into the `TransportRegistry` table.

### Registries and Earnings
Buttons like "Реестры" (`btnOpenReestrs`) and "Заработок" (`btnOpenEarningsForm`) open `DriverSalaryForm` and `VehicleDriverEarningsForm` respectively.

### Comparison and Analysis
The `MainForm` also includes buttons for "Сравнение" (`btnOpenComparisonForm`) and "Анализ зон" (`btnOpenZoneAnalysis`). The `ComparisonForm` loads distinct vehicle numbers (`GosNumber`) and driver names (`FIO`) from the `TransportRegistry` table for comparison purposes.

### Registry Editing
The "Редактировать реестр" (`btnEditRegistry`) button allows users to edit entries in the `TransportRegistry`. The `EditRegistryForm` handles updating rows in the `TransportRegistry` table with various fields such as date, driver's FIO, vehicle number, tonnage, and financial details.

## Database Structure
The application uses a SQL Server database. A key table is `Перевозки` (Transportations), which stores details about individual trips, including date, driver's full name, vehicle number, cargo weight, fuel consumption, and profit. The `Script.sql` file defines the schema for this table and populates it with initial data. Another important table is `TransportRegistry`, which stores comprehensive information about transport operations, including details like `Date`, `FIO`, `GosNumber`, `Tonnage`, `TripCost`, and `OrderNumber`.

## Configuration
The database connection string is managed through the `Config.cs` file, which reads from and writes to the `appSettings` section of the application's configuration file (`App.config`). The connection string is defined in `App.config` under the key "conString".

## Notes
The provided snippets indicate a C# Windows Forms application interacting with a SQL Server database. The application's core functionality revolves around managing transport operations, including vehicle maintenance, driver management, and financial reporting. The `Program.cs` file serves as the entry point, handling initial checks and launching the main user interface. The `MainForm` acts as a central hub, providing access to various sub-forms for specific tasks. The database schema, partially defined in `Script.sql`, suggests a focus on tracking individual transportation events and related data.

---
*Source: [DeepWiki Search](https://deepwiki.com/search/transportcompany-application-o_15820806-bb5f-4fb9-ae16-cf9c788a3f0f)*


---

## Application Manifests and Deployment

The `TransportCompany` application uses ClickOnce for deployment, with details specified in its application manifests, such as `TransportCompany.exe.manifest`. These manifests define the application's identity, required permissions, dependencies, and included files, ensuring integrity through SHA-256 hash verification for all deployed components.

### Application Manifest Structure

The application manifest is an XML file that outlines the deployment details of the `TransportCompany` application. It includes several key sections:

#### Assembly Identity
The manifest specifies the application's identity, including its name (`TransportCompany.exe`), version (`1.0.0.4`), processor architecture (`msil`), and type (`win32`). The `TransportCompany.csproj` file also defines the `ApplicationVersion` as `1.0.0.*` and `ApplicationRevision` as `4`.

#### Trust Information
The `trustInfo` section defines the security permissions requested by the application. For `TransportCompany`, it requests `Unrestricted` permissions and specifies an execution level of `asInvoker`, meaning it runs with the same permissions as the launching user.

#### Dependencies
The manifest lists all dependent assemblies required by the application. This includes:
* **Operating System**: The application requires Windows XP SP2 or later (major version 5, minor version 1, build 2600).
* **.NET Framework**: It depends on .NET Common Language Runtime version `4.0.30319.0`.
* **Third-Party Assemblies**: Numerous third-party libraries are listed as `dependentAssembly` elements, each with its `name`, `version`, `publicKeyToken`, `language`, `processorArchitecture`, `size`, and a SHA-256 `DigestValue` for integrity verification. Examples include `BouncyCastle.Cryptography.dll`, `MathNet.Numerics.dll`, `NPOI.OOXML.dll`, and `HtmlAgilityPack.dll`.

#### Included Files
The manifest also lists additional files deployed with the application, such as `icon.ico`, `Script.sql`, and `TransportCompany.exe.config`. Each of these files also includes a SHA-256 hash for verification.

### Deployment Model
The `TransportCompany` application is deployed using ClickOnce. This is indicated by the `co.v1` and `co.v2` XML namespaces in the manifest. The `TransportCompany.csproj` file confirms this with `<IsWebBootstrapper>false</IsWebBootstrapper>`, `<Install>true</Install>`, and `<InstallFrom>Disk</InstallFrom>` settings. The application's revision is tracked, with `ApplicationRevision` set to `4`.

### Security Hash Verification
A critical aspect of the deployment is the use of SHA-256 hash verification for all assemblies and included files. This ensures that the deployed components have not been tampered with since their publication. For example, `TransportCompany.exe` itself has a specific SHA-256 digest value.

---

## Third-Party Dependencies and Licensing

The `TransportCompany` application utilizes 26 third-party libraries, primarily under permissive open-source licenses such as Apache 2.0, MIT, and BSD-style licenses. These dependencies are categorized into core document processing, cryptography and security, numerical and computational, performance infrastructure, and utility and support. The total size of these third-party binaries is approximately 23 MB.

### Complete Dependency Catalog

#### Core Document Processing Dependencies

| Package | Version | License | Purpose |
|---|---|---|---|
| `NPOI.Core` | 2.7.3.0 | Apache 2.0 | Excel file format engine (HSSF/XSSF) |
| `NPOI.OOXML` | 2.7.3.0 | Apache 2.0 | Office Open XML format support |
| `NPOI.OpenXml4Net` | 2.7.3.0 | Apache 2.0 | OPC package manipulation |
| `NPOI.OpenXmlFormats` | 2.7.3.0 | Apache 2.0 | OOXML schema definitions |
| `ClosedXML.Parser` | 1.0.0.0 | MIT | Excel formula parsing |
| `ExcelNumberFormat` | 1.1.0.0 | MIT | Number format interpretation |
| `HtmlAgilityPack` | 1.12.1.0 | MIT | HTML document parsing |
| `ICSharpCode.SharpZipLib` | 1.4.2.13 | MIT (with exceptions) | ZIP/compression support |

#### Cryptography and Security Dependencies

| Package | Version | License | Purpose |
|---|---|---|---|
| `BouncyCastle.Cryptography` | 2.0.0.0 | MIT (adapted) | Cryptographic primitives |
| `System.Security.Cryptography.Pkcs` | 8.0.0.1 | MIT | PKCS#7/CMS message support |
| `System.Security.Cryptography.Xml` | 8.0.0.2 | MIT | XML digital signatures |

#### Numerical and Computational Dependencies

| Package | Version | License | Purpose |
|---|---|---|---|
| `MathNet.Numerics` | 5.0.0.0 | MIT | Linear algebra, statistics, optimization |
| `ExtendedNumerics.BigDecimal` | 2025.1001.2.129 | MIT | Arbitrary-precision decimal arithmetic |
| `RBush` | 4.0.0.0 | MIT | R-tree spatial indexing |
| `System.Numerics.Vectors` | 4.1.4.0 | MIT | SIMD vector operations |

#### Performance Infrastructure Dependencies

| Package | Version | License | Purpose |
|---|---|---|---|
| `System.Buffers` | 4.0.3.0 | MIT | Array pooling |
| `System.Memory` | 4.0.1.2 | MIT | Span<T> and Memory<T> types |
| `System.Runtime.CompilerServices.Unsafe` | 5.0.0.0 | MIT | Unsafe memory operations |
| `Microsoft.IO.RecyclableMemoryStream` | 3.0.1.0 | MIT | Pooled memory stream |

#### Utility and Support Dependencies

| Package | Version | License | Purpose |
|---|---|---|---|
| `System.ComponentModel.Annotations` | 4.2.1.0 | MIT | Data validation attributes |
| `Enums.NET` | 4.0.0.0 | MIT | Type-safe enum operations |
| `Microsoft.Bcl.HashCode` | 1.0.0.0 | MIT | HashCode generation |
| `SixLabors.Fonts` | 1.0.0.0 | Apache 2.0 (Six Labors Split) | Font rendering |
| `SixLabors.ImageSharp` | 2.0.0.0 | Apache 2.0 (Six Labors Split) | Image processing |
| `System.Text.Encoding.CodePages` | 5.0.0.0 | MIT | Extended encoding support |
| `System.Threading.Tasks.Extensions` | 4.2.0.0 | MIT | ValueTask support |

### License Categories and Terms

The application primarily uses Apache 2.0, MIT, and BSD-style licenses.

#### Apache License 2.0 Dependencies
Apache 2.0 licensed packages include the NPOI and SixLabors suites, permitting commercial use, modification, and distribution with attribution. Key requirements include providing a copy of the license, stating significant changes, and preserving copyright notices.

#### MIT License Dependencies
The majority of dependencies are under the MIT license, which is highly permissive. This includes many Microsoft .NET libraries and other third-party components. The main requirements are to include the copyright notice and license text, with no warranty provided.

#### BouncyCastle Cryptography License
`BouncyCastle.Cryptography` uses a custom MIT-style license, allowing free use for any purpose, including commercial, with the requirement to include copyright notice. Export control regulations may apply to cryptographic software.

### Compliance Requirements

#### Attribution Requirements
The application must include a `THIRD-PARTY-NOTICES.TXT` file, specific license texts for Apache 2.0, MIT, and BouncyCastle, and verify no GPL or copyleft dependencies.

#### Export Control Considerations
The application's cryptographic functionality, provided by `BouncyCastle.Cryptography`, `System.Security.Cryptography.Pkcs`, and `System.Security.Cryptography.Xml`, may be subject to export control regulations. Legal counsel should be consulted for specific jurisdictions.

---
*Source: [DeepWiki - Application Manifests](https://deepwiki.com/search/application-manifests-and-depl_369d8c5f-b81e-4bd4-a925-71c3354d8319)*
*Source: [DeepWiki - Third-Party Dependencies](https://deepwiki.com/search/thirdparty-dependencies-and-li_8e31d24a-a34f-4099-8a24-6028cc8fd16f)*
