# Document Processing Systems

The document processing system utilizes the NPOI library for Excel document handling and the HtmlAgilityPack library for HTML processing. This system supports various operations including reading, writing, manipulation, encryption, and digital signatures for Excel files, and parsing, DOM navigation, and XPath queries for HTML content.

## Excel Processing with NPOI

The NPOI library is used for comprehensive Excel document handling, supporting both `.xls` and `.xlsx` formats.

### Core Components
NPOI provides core components such as `IWorkbook` for the document container, `ISheet` for worksheets, `IRow` for row data, and `ICell` for cell data. The `WorkbookFactory` class is responsible for creating the appropriate workbook type (HSSFWorkbook or XSSFWorkbook) from a given input.

### Excel to HTML Conversion
The `NPOI.SS.Converter.ExcelToHtmlConverter` class facilitates the conversion of Excel spreadsheets to HTML. It offers properties to control the output, such as `OutputColumnHeaders`, `OutputHiddenColumns`, `OutputHiddenRows`, `OutputLeadingSpacesAsNonBreaking`, `OutputRowNumbers`, and `UseDivsToSpan`. The `ProcessRow` method is used to render a row, and `ProcessColumnWidths` creates `COLGROUP` elements for column widths. Utility methods like `BuildMergedRangesMap` and `GetMergedRange` in `NPOI.SS.Converter.ExcelToHtmlUtils` help in handling merged cells during conversion.

### Document Security
NPOI.OOXML also provides cryptographic capabilities for securing Office documents. The `AgileDecryptor` and `AgileEncryptor` classes handle encryption using the Agile Encryption specification. Digital signatures are managed through classes like `SignatureInfo` and `SignatureConfig`, which utilize a facet-based architecture for various signature features.

## HTML Processing with HtmlAgilityPack

The HtmlAgilityPack library is used for parsing and manipulating HTML documents, including malformed ones.

### Core Components
Key classes include `HtmlDocument` for representing the entire HTML document, `HtmlNode` for individual HTML elements, and `HtmlAttribute` for element attributes.

### Parsing Features
HtmlAgilityPack supports loading HTML from various sources (files, URLs, strings) and includes features like encoding detection and error handling for malformed HTML. The `HtmlDocument` class provides options such as `OptionCheckSyntax`, `OptionFixNestedTags`, and `OptionOutputAsXml` to configure parsing behavior.

## Document Processing Workflows

### Excel Import Workflow
Excel files are loaded using NPOI.OOXML. If encrypted, they are decrypted using `AgileDecryptor`. The data is then read from `IWorkbook`, `ISheet`, `IRow`, and `ICell` objects, validated, transformed into business entities, and persisted to a database.

### HTML Scraping Workflow
HTML content is loaded and parsed by `HtmlDocument`. After applying parsing options and handling errors, XPath queries are used to extract data from `HtmlNode` objects. This extracted data is then transformed, validated, and stored.

## Notes
The XML documentation files for NPOI.OOXML and HtmlAgilityPack are consistent across different target frameworks (`net472`, `net8.0`, `netstandard2.0`, `netstandard2.1`), indicating a stable API. The `XSSFExcelExtractor` class provides functionality to extract text content from XLSX files, including options to include sheet names, formulas, cell comments, headers, footers, and text boxes.

---
*Source: [DeepWiki Search](https://deepwiki.com/search/document-processing-systems-ex_b98f973d-f03b-4eda-8a00-6bd30c7997e7)*


---

## Office Document Encryption and Digital Signatures

The TransportCompany application utilizes the `NPOI.OOXML` library to handle Office document encryption and digital signatures. This system provides functionalities for both encrypting documents using Agile Encryption and applying digital signatures with a facet-based architecture.

### Office Document Encryption

The encryption of Office documents is managed by the `NPOI.POIFS.Crypt.Agile` namespace, specifically through the `AgileDecryptor` and `AgileEncryptor` classes. These classes implement the Agile Encryption specification.

#### Encryption Process
Documents are encrypted in 4096-byte segments using CBC mode with dynamic initialization vectors. The `AgileCipherInputStream` and `AgileCipherOutputStream` classes handle these segmented operations. The integrity of the encrypted data is maintained using HMAC, as specified in RFC2104, which is updated by the `AgileEncryptor.UpdateIntegrityHMAC` method.

#### Decryption Process
Decryption can be performed using a password via `AgileDecryptor.VerifyPassword(System.String)` or experimentally with a certificate using `AgileDecryptor.VerifyPassword(NPOI.POIFS.Crypt.Agile.KeyPair,Org.BouncyCastle.X509.X509Certificate)`.

### Digital Signatures

Digital signatures are handled by classes within the `NPOI.POIFS.Crypt.Dsig` namespace. The primary entry point for XML signatures is the `SignatureInfo` class.

#### Signature Configuration
The `SignatureConfig` class is used to configure various aspects of the digital signature, including the private key, certificate chain, and the OPC package to be signed. It also allows for the addition of `SignatureFacet` implementations, which define specific signature features.

#### Signature Facets
The system supports various signature facets, including:
* `EnvelopedSignatureFacet`: For creating enveloped signatures.
* `KeyInfoSignatureFacet`: Adds `ds:KeyInfo` to the XML signature.
* `OOXMLSignatureFacet`: Specific to Office OpenXML signatures.
* `XAdESSignatureFacet`: Implements XAdES v1.4.1 (BES/EPES format).
* `XAdESXLSignatureFacet`: Upgrades XAdES-BES/EPES to XAdES-X-L.
* `Office2010SignatureFacet`: A workaround for Office 2010 compatibility with XAdES-BES/EPES signatures.

#### Signing and Validation Workflow
To sign an Office document, you would typically:
1. Load a keystore containing a private key and its certificate.
2. Extract the private key and X.509 certificate.
3. Configure a `SignatureConfig` object with the private key, certificate chain, and the `OPCPackage` of the document.
4. Create a `SignatureInfo` object and set its `SignatureConfig`.
5. Call `si.confirmSignature()` to add the XML signature to the document.
6. Optionally, verify the generated signature using `si.verifySignature()`.

For validating an existing signed document, you would open the `OPCPackage` in read mode, set it in `SignatureConfig`, and then call `si.validate()` on a `SignatureInfo` object.

#### Error Handling
The system defines specific exceptions for security-related issues:
* `CertificateSecurityException`: For general problems with incoming certificates.
* `ExpiredCertificateSecurityException`: When an incoming certificate has expired.
* `RevokedCertificateSecurityException`: When an incoming certificate has been revoked.

---
*Source: [DeepWiki - Office Document Encryption](https://deepwiki.com/search/office-document-encryption-and_84eecada-46ea-43b0-9de3-541ec259474d)*
