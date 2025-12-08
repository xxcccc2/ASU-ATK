# TransportCompany (ASU-ATK) - Полная Документация

Документация проекта TransportCompany, экспортированная из [DeepWiki](https://deepwiki.com/xxcccc2/ASU-ATK).

## Содержание

### 1. [TransportCompany Application Overview](1_TransportCompany_Overview.md)
- Обзор приложения, основные функции
- Структура базы данных и главная форма
- **Application Manifests and Deployment** - ClickOnce развертывание, манифесты
- **Third-Party Dependencies and Licensing** - 26 библиотек, лицензии Apache 2.0/MIT

### 2. [Data Validation and Type Safety](2_Data_Validation.md)
- Система валидации данных через System.ComponentModel.Annotations
- Validator класс и ValidationContext
- **System.ComponentModel.Annotations Framework** - полный список атрибутов валидации
- **Display and Metadata Attributes** - UI атрибуты для отображения
- **Entity Relationships and Schema Attributes** - ForeignKey, InverseProperty, Table, Column
- **Enums.NET Type-Safe Enum Operations** - boxing-free операции с enum

### 3. [Document Processing Systems](3_Document_Processing.md)
- **Excel Document Handling with NPOI** - IWorkbook, ISheet, IRow, ICell
- **Excel to HTML Conversion** - ExcelToHtmlConverter
- **Office Document Encryption and Digital Signatures** - AgileEncryptor/Decryptor, SignatureInfo
- **HTML Processing with HtmlAgilityPack** - HtmlDocument, HtmlNode, XPath queries

### 4. [Computational and Mathematical Systems](4_Computational_Systems.md)
- **ExtendedNumerics.BigDecimal** - произвольная точность для финансовых расчетов
- **Precision Management** - AlwaysTruncate, AlwaysNormalize, Precision
- **Trigonometric and Logarithmic Operations** - Sin, Cos, Tan, Ln, Exp с произвольной точностью
- **Helper Classes and Optimization Algorithms** - TrigonometricHelper, MathNet.Numerics для TSP/VRP

### 5. [Security and Cryptography Infrastructure](5_Security_Cryptography.md)
- Многоуровневая архитектура: Application → Libraries → Document → Operations
- **PKCS#7/CMS Cryptographic Message Syntax** - EnvelopedCms, SignedCms, ContentInfo
- **CMS Message Encryption and Key Management** - KeyAgreeRecipientInfo, KeyTransRecipientInfo
- **CMS Digital Signatures and Verification** - ComputeSignature, CheckSignature
- **XML Encryption and Digital Signatures** - EncryptedXml, SignedXml
- **EncryptedXml Processing Model** - EncryptData, DecryptData, key management

### 6. [Performance Infrastructure](6_Performance_Infrastructure.md)
- **System.Buffers Array Pooling** - ArrayPool<T>.Shared, Rent/Return pattern
- **System.Runtime.CompilerServices.Unsafe** - pointer arithmetic, type reinterpretation
- **Pointer Arithmetic and Memory Access Patterns** - Add, Subtract, ByteOffset, CopyBlock
- **ValueTuple Extensions and Tuple Interoperability** - Deconstruct, ToTuple, ToValueTuple

## Статистика

- **Всего разделов**: 6 основных + 24 подраздела
- **Библиотек**: 26 (Apache 2.0, MIT, BSD)
- **Размер зависимостей**: ~23 MB
- **Криптография**: BouncyCastle 2.0.0.0, PKCS 8.0.0.1, XML 8.0.0.2
- **Документообработка**: NPOI 2.7.3.0, HtmlAgilityPack 1.12.1.0
- **Вычисления**: BigDecimal 2025.1001.2.129, MathNet.Numerics 5.0.0.0

## Источник

Документация сгенерирована из репозитория [xxcccc2/ASU-ATK](https://deepwiki.com/xxcccc2/ASU-ATK) через DeepWiki MCP.

**Дата экспорта**: 8 декабря 2025  
**Метод**: Постепенная выгрузка по разделам через `ask_question` API  
**Охват**: 100% структуры документации DeepWiki
