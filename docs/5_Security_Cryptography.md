# Security and Cryptography Infrastructure

The security and cryptography infrastructure within the `TransportCompany` application employs a multi-layered approach to security, encompassing application deployment, document encryption, digital signatures, and cryptographic message handling.

## Security Architecture Overview

The security infrastructure is structured into four main layers: Application Security, Cryptographic Libraries, Document Security, and Security Operations.

### Layer 1: Application Security
Application security primarily focuses on ensuring the integrity of the deployed application through ClickOnce manifests. Each assembly and file is verified using SHA-256 hashing, as specified by `dsig:DigestMethod` with `Algorithm="http://www.w3.org/2000/09/xmldsig#sha256"`.

### Layer 2: Cryptographic Libraries
The application utilizes three core cryptographic libraries:
* **BouncyCastle.Cryptography**: Provides fundamental cryptographic primitives like RSA, AES, SHA-256/512, and X.509 certificate support. This library is version `2.0.0.0` and has a public key token of `072EDCF4A5328938`.
* **System.Security.Cryptography.Pkcs**: Handles CMS/PKCS#7 messaging, including `EnvelopedCms` for encrypted messages and `SignedCms` for digitally signed messages. This library is version `8.0.0.1`.
* **System.Security.Cryptography.Xml**: Offers XML-specific cryptographic operations such as `EncryptedXml` for encryption/decryption and `SignedXml` for signature creation. This library is version `8.0.0.2`.

### Layer 3: Document Security
Document security integrates with NPOI.OOXML for Office document encryption and uses XML Digital Signatures and CMS/PKCS#7 messages.

### Layer 4: Security Operations
This layer covers the actual cryptographic operations, including encryption (AES-128, AES-256), digital signing (RSA, DSA), and validation (certificate chains).

## PKCS#7/CMS Cryptographic Message Syntax

The `System.Security.Cryptography.Pkcs` assembly is central to handling PKCS#7/CMS messages. Key classes include `EnvelopedCms` for encrypted messages and `SignedCms` for signed messages.

### `SignedCms` Operations
The `SignedCms` class allows for computing and verifying digital signatures. For example, `ComputeSignature(CmsSigner)` creates a digital signature, and `CheckSignature(Boolean)` verifies it, optionally validating the certificate chain.

## XML Encryption and Digital Signatures

The `System.Security.Cryptography.Xml` assembly provides functionalities for XML encryption and digital signatures.

### `SignedXml` Operations
The `SignedXml` class is used for creating and verifying XML signatures. Methods like `ComputeSignature()` generate a signature, and `CheckSignature()` verifies it using various key types or certificates.

## Key Management and Certificate Handling

X.509 certificates are used for asymmetric cryptography. Certificates can be loaded from user or machine stores, or from files. The `CmsRecipient` and `CmsSigner` classes utilize these certificates for encryption and signing, respectively.

## Encryption Algorithm Support

The infrastructure supports various algorithms, including AES-128/256 for symmetric encryption, RSA-OAEP/PKCS1 for asymmetric key wrapping, and SHA-256/384/512 for hashing. The `AlgorithmIdentifier` class specifies the algorithm, key length, and parameters.

## Security Best Practices

The implementation adheres to best practices such as using SHA-256 minimum for hashing, AES-256 for symmetric encryption, and RSA-2048 minimum for asymmetric operations with OAEP padding. Private keys are kept secure, and certificate chains are validated.

---
*Source: [DeepWiki Search](https://deepwiki.com/search/security-and-cryptography-infr_528353ad-e3f2-4d35-a2a3-d383e24b2c97)*


---

## PKCS#7/CMS Cryptographic Message Syntax

The `TransportCompany` application utilizes the `System.Security.Cryptography.Pkcs` library for handling PKCS#7/CMS (Cryptographic Message Syntax) messages, enabling both encrypted and digitally signed message exchanges. This functionality is crucial for secure document exchange within the application's multi-layered security architecture.

### Core CMS Classes and Functionality

The `System.Security.Cryptography.Pkcs` assembly provides several key classes for working with PKCS#7/CMS messages:

* **`ContentInfo`**: This class represents the fundamental data structure for all CMS/PKCS#7 messages. It encapsulates the actual content and its type, identified by an Object Identifier (OID). You can create a `ContentInfo` object with raw byte data and a content type OID.

* **`EnvelopedCms`**: This class manages encrypted message envelopes. It supports encrypting content for one or more recipients using their certificates and decrypting messages using a recipient's private key. The `EnvelopedCms` class uses a hybrid encryption pattern, where a symmetric key encrypts the content, and this symmetric key is then encrypted for each recipient using their public key. Key properties include `RecipientInfos` for recipient details, `ContentInfo` for the encrypted content, and `ContentEncryptionAlgorithm` for the symmetric algorithm used.

* **`SignedCms`**: This class handles digitally signed messages. It allows for computing signatures using a `CmsSigner` and verifying them, optionally validating the signer's certificate chain. The `SignedCms` class can create both enveloping and detached signatures. Key properties include `SignerInfos` for signer details, `ContentInfo` for the signed content, and `Certificates` for the associated certificate chain.

### Supporting Classes

* **`CmsRecipient`**: This class defines information about a message recipient, including their certificate and how they are identified (e.g., by issuer and serial number or subject key identifier).
* **`CmsSigner`**: This class specifies information about a message signer, such as their certificate and the digest algorithm used for signing. It also allows for specifying RSA signature padding and associating signed and unsigned attributes with the signature.
* **`AlgorithmIdentifier`**: This class is used to specify cryptographic algorithms, including their OID, key length, and optional parameters.
* **`CryptographicAttributeObject`**: This class represents an attribute associated with a CMS message, identified by an OID and containing ASN.1 encoded values.

### Usage Patterns

The application uses these classes to implement security operations such as hybrid encryption and digital signatures. For instance, `EnvelopedCms` encrypts content with a symmetric key, then encrypts that key for each recipient using their public key, forming a secure envelope. `SignedCms` computes a hash of the content, signs it with a private key, and can include attributes and the certificate chain.

---

## CMS Message Encryption and Key Management

The `TransportCompany` application uses the `EnvelopedCms` class to handle encrypted CMS/PKCS#7 messages.

### Encryption Process
To encrypt content, an instance of `EnvelopedCms` is initialized with the content to be encrypted, typically wrapped in a `ContentInfo` object. Optionally, a specific symmetric encryption algorithm can be provided during initialization. The actual encryption is performed by calling the `Encrypt` method, which takes either a single `CmsRecipient` or a `CmsRecipientCollection` to specify the intended recipients. This process uses a hybrid encryption pattern where a symmetric key encrypts the content, and this symmetric key is then encrypted for each recipient using their public key. The encrypted message can then be encoded into a byte array using the `Encode` method.

### Decryption Process
To decrypt a CMS message, the `EnvelopedCms` object first decodes the incoming byte array message using the `Decode` method. Decryption is then performed by calling one of the `Decrypt` methods. These methods attempt to find a matching certificate and private key in certificate stores or a provided collection to decrypt the content encryption key, and subsequently the message content.

### Key Management

Key management in CMS messages involves specifying how the symmetric content encryption key is securely delivered to recipients. The `System.Security.Cryptography.Pkcs` library supports two primary mechanisms for this: Key Agreement and Key Transport.

#### Key Agreement
Key agreement algorithms, typically Diffie-Hellman, are represented by the `KeyAgreeRecipientInfo` class. In this method, both the originator and recipient participate in generating a shared cryptographic key. The `KeyAgreeRecipientInfo` class provides properties to retrieve information about the encrypted keying material, the key agreement algorithm used, and the originator's information.

#### Key Transport
Key transport algorithms, typically RSA, are represented by the `KeyTransRecipientInfo` class. Here, the originator generates the shared cryptographic key and then encrypts (transports) it to the recipient using the recipient's public key. The `KeyTransRecipientInfo` class provides access to the encrypted key and the key encryption algorithm used.

---

## CMS Digital Signatures and Verification

The `SignedCms` class is central to creating and managing digitally signed CMS/PKCS #7 messages.

### Signature Creation
To create a digital signature, you instantiate a `SignedCms` object, optionally providing the content to be signed and whether the signature should be detached. The `ComputeSignature` method is then used, which can take a `CmsSigner` object to specify the signer's certificate and other signing parameters. The `SignedCms` object also allows adding certificates to its collection using `AddCertificate`.

### Signature Structure
A `SignedCms` message contains `SignerInfos`, which represent the signers associated with the message. Each `SignerInfo` object can have authenticated and unauthenticated attributes. The `Detached` property indicates if the signature is detached from the content.

### Verification Methods
The `SignedCms` class provides `CheckSignature` methods to verify digital signatures. You can choose to verify only the digital signatures or also validate the signers' certificates. The `CheckSignature(Boolean verifySignatureOnly)` method allows you to specify whether certificate validation should occur. An overload, `CheckSignature(X509Certificate2Collection extraStore, Boolean verifySignatureOnly)`, allows providing additional certificates for chain validation.

### Signer-Specific Verification
Each `SignerInfo` object also has its own `CheckSignature` methods to verify the digital signature of that specific signer. Similar to `SignedCms`, these methods allow for optional certificate validation and can accept an `X509Certificate2Collection` for additional certificates.

### Data Integrity Check
For scenarios where only data integrity needs to be verified without authenticating the sender, the `CheckHash` method can be used on both `SignedCms` and `SignerInfo` objects. This method verifies the hash of the CMS message but does not involve digital signature verification.

---

## XML Encryption and Digital Signatures

The `TransportCompany` application utilizes the `System.Security.Cryptography.Xml` assembly for XML Encryption and Digital Signatures. This library provides classes like `EncryptedXml` for encryption and decryption operations, and `SignedXml` for creating and verifying XML digital signatures.

### XML Encryption

XML encryption in the `TransportCompany` application is handled by the `EncryptedXml` class.

#### Encryption Operations
The `EncryptedXml` class provides methods to encrypt XML elements or raw byte data:
* `Encrypt(XmlElement, X509Certificate2)`: Encrypts an XML element using an X.509 certificate.
* `Encrypt(XmlElement, String)`: Encrypts an XML element using a key specified by a key name from a mapping table.
* `EncryptData(Byte[], SymmetricAlgorithm)`: Encrypts a byte array using a symmetric algorithm.
* `EncryptData(XmlElement, SymmetricAlgorithm, Boolean)`: Encrypts an XML element or its contents using a symmetric algorithm.
* `EncryptKey(Byte[], RSA, Boolean)`: Encrypts a key using an RSA asymmetric key.

#### Decryption Operations
The `EncryptedXml` class also supports decryption:
* `DecryptData(EncryptedData, SymmetricAlgorithm)`: Decrypts an `EncryptedData` element using a symmetric algorithm.
* `DecryptDocument()`: Decrypts all encrypted elements within the XML document.

#### Key Management
Key name mappings can be added and cleared using `AddKeyNameMapping(String, Object)` and `ClearKeyNameMappings()`.

### XML Digital Signatures

XML Digital Signatures are managed by the `SignedXml` class.

#### Signature Creation
To create an XML digital signature, you can use the following methods:
* `AddReference(Reference)`: Adds a reference to the data that will be signed.
* `AddObject(DataObject)`: Adds a `DataObject` to the list of objects to be signed.
* `ComputeSignature()`: Computes the XML digital signature.
* `ComputeSignature(KeyedHashAlgorithm)`: Computes an XML digital signature using a specified MAC algorithm.

The `SignedXml` object uses the `SigningKey` property to specify the asymmetric algorithm key for signing.

#### Signature Verification
Verification of XML digital signatures can be performed using:
* `CheckSignature()`: Verifies the signature using the public key embedded in the signature.
* `CheckSignature(AsymmetricAlgorithm)`: Verifies the signature using a specified asymmetric algorithm key.
* `CheckSignature(X509Certificate2, Boolean)`: Verifies the signature using an X.509 certificate, with an option to also validate the certificate.

The `SignedInfo` property of `SignedXml` contains information about what is signed and the signature method used, such as `SignatureMethod` (e.g., RSA-SHA256).

---

## EncryptedXml Processing Model

The `EncryptedXml` class provides the core functionality for XML encryption and decryption. It handles key management, encryption of XML elements or their content, and decryption of encrypted XML data.

### Key Management

The `EncryptedXml` class manages cryptographic keys through the following methods:
* `AddKeyNameMapping(String, Object)`: This method allows you to associate a key name with a symmetric or asymmetric key object. This mapping is crucial for the `EncryptedXml` object to locate the correct key for encryption or decryption operations when a key name is specified.
* `ClearKeyNameMappings()`: This method removes all previously defined key name mappings.
* `GetDecryptionKey(EncryptedData, String)`: This method retrieves the appropriate decryption key for a given `EncryptedData` object, potentially using a symmetric algorithm URI.

### Encryption Operations

The `EncryptedXml` class provides several methods for encrypting XML data:
* `EncryptData(XmlElement, SymmetricAlgorithm, Boolean)`: This method encrypts an XML element or its contents using a specified symmetric algorithm. The `content` parameter determines whether only the element's content or the entire element is encrypted.
* `EncryptData(Byte[], SymmetricAlgorithm)`: This method encrypts raw byte data using a specified symmetric algorithm.
* `Encrypt(XmlElement, X509Certificate2)`: This method encrypts the outer XML of an element using an X.509 certificate. This typically involves asymmetric encryption to protect a symmetric key, which then encrypts the data.
* `Encrypt(XmlElement, String)`: This method encrypts the outer XML of an element using a key identified by a key name from the key mapping table.
* `EncryptKey(Byte[], RSA, Boolean)`: This method encrypts a key using an RSA asymmetric algorithm. The `useOAEP` parameter specifies whether to use Optimal Asymmetric Encryption Padding (OAEP).
* `EncryptKey(Byte[], SymmetricAlgorithm)`: This method encrypts a key using a symmetric algorithm.

### Decryption Operations

For decryption, the `EncryptedXml` class offers:
* `DecryptData(EncryptedData, SymmetricAlgorithm)`: This method decrypts an `EncryptedData` element using a specified symmetric algorithm.
* `DecryptDocument()`: This method decrypts all `EncryptedData` elements within the XML document that was used to initialize the `EncryptedXml` object.
* `DecryptEncryptedKey(EncryptedKey)`: This method determines and decrypts the key contained within an `EncryptedKey` element.
* `DecryptKey(Byte[], RSA, Boolean)`: This method decrypts an encrypted key using an RSA asymmetric algorithm.
* `DecryptKey(Byte[], SymmetricAlgorithm)`: This method decrypts an encrypted key using a symmetric algorithm.

### XML Structure and Replacement

The `EncryptedXml` class also provides methods for manipulating the XML document structure during encryption and decryption:
* `ReplaceData(XmlElement, Byte[])`: This method replaces an `EncryptedData` element with its decrypted byte sequence.
* `ReplaceElement(XmlElement, EncryptedData, Boolean)`: This method replaces a specified XML element with an `EncryptedData` object. The `content` parameter dictates whether the entire element or just its content is replaced.

### Supported Algorithms

The `EncryptedXml` class supports various encryption algorithms, identified by Uniform Resource Identifiers (URIs). These include:
* AES (128, 192, 256-bit) for content encryption and key wrapping.
* Triple DES for content encryption and key wrapping.
* RSA-OAEP and RSA-PKCS1 for asymmetric key encryption.
* SHA-256 and SHA-512 for hashing, though these are more relevant to XML Digital Signatures.

---
*Source: [DeepWiki - PKCS#7/CMS](https://deepwiki.com/search/pkcs7cms-cryptographic-message_757ba076-5ca9-46c6-898b-8019aed84ac1)*
*Source: [DeepWiki - CMS Encryption](https://deepwiki.com/search/cms-message-encryption-and-key_1e56c915-7422-488a-b551-42e96ad15a39)*
*Source: [DeepWiki - CMS Signatures](https://deepwiki.com/search/cms-digital-signatures-and-ver_f5422f08-4a97-4079-8663-d4a75fb7f136)*
*Source: [DeepWiki - XML Encryption](https://deepwiki.com/search/xml-encryption-and-digital-sig_fde23d13-6529-4783-9b0c-39dd98cd667c)*
*Source: [DeepWiki - EncryptedXml](https://deepwiki.com/search/encryptedxml-processing-model_537c1221-e4c8-4833-8812-be9e4d8ae6b1)*
