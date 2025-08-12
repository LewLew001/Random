# Easy Way to Encrypt/Decrypt Data in SQL Server 2012

SQL Server 2012 supports symmetric key encryption, which is efficient and relatively easy to implement.

---

## ✅ Step-by-Step Setup

### 1. Create a Master Key (once per database)
```sql
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'StrongPassword123!';
```

### 2. Create a Symmetric Key
```sql
CREATE SYMMETRIC KEY SymKey_AES
WITH ALGORITHM = AES_256
ENCRYPTION BY PASSWORD = 'AnotherStrongPassword!';
```

### 3. Encrypt Data on Insert/Update (in your stored procedures)
```sql
OPEN SYMMETRIC KEY SymKey_AES
DECRYPTION BY PASSWORD = 'AnotherStrongPassword!';

INSERT INTO YourTable (EncryptedColumn)
VALUES (EncryptByKey(Key_GUID('SymKey_AES'), @PlainTextData));

CLOSE SYMMETRIC KEY SymKey_AES;
```

### 4. Decrypt Data in Your View
```sql
OPEN SYMMETRIC KEY SymKey_AES
DECRYPTION BY PASSWORD = 'AnotherStrongPassword!';

SELECT CONVERT(VARCHAR, DecryptByKey(EncryptedColumn)) AS DecryptedData
FROM YourTable;

CLOSE SYMMETRIC KEY SymKey_AES;
```

---

## 🧠 Tips for Smooth Integration

- **Centralize Key Management:** Avoid hardcoding passwords in multiple places. Use secure configuration or a dedicated security module.
- **Use Views and Procedures:** Keep encryption logic inside procedures and decryption inside views.
- **Audit Access:** Track who accesses or modifies encrypted data.
- **Performance Consideration:** Encryption adds overhead. Test performance, especially with large datasets.

---

## ⚠️ Limitations in SQL Server 2012

- No built-in Always Encrypted (introduced in SQL Server 2016).
- No Transparent Data Encryption (TDE) for column-level encryption.
- You must manage keys and passwords manually.

---

## Example: Encrypting Multiple Columns

Suppose you have a table like this:

```sql
CREATE TABLE Customers (
    ID INT,
    Name VARBINARY(MAX),
    Email VARBINARY(MAX),
    SSN VARBINARY(MAX)
);
```

You’d encrypt each column separately:

```sql
OPEN SYMMETRIC KEY SymKey_AES
DECRYPTION BY PASSWORD = 'AnotherStrongPassword!';

INSERT INTO Customers (ID, Name, Email, SSN)
VALUES (
    1,
    EncryptByKey(Key_GUID('SymKey_AES'), @Name),
    EncryptByKey(Key_GUID('SymKey_AES'), @Email),
    EncryptByKey(Key_GUID('SymKey_AES'), @SSN)
);

CLOSE SYMMETRIC KEY SymKey_AES;
```

And decrypt them in your view:

```sql
OPEN SYMMETRIC KEY SymKey_AES
DECRYPTION BY PASSWORD = 'AnotherStrongPassword!';

SELECT
    ID,
    CONVERT(VARCHAR, DecryptByKey(Name)) AS Name,
    CONVERT(VARCHAR, DecryptByKey(Email)) AS Email,
    CONVERT(VARCHAR, DecryptByKey(SSN)) AS SSN
FROM Customers;

CLOSE SYMMETRIC KEY SymKey_AES;
```

---

## 🧰 Alternative: Encrypt Entire Payload

If you want to encrypt the entire row:
- Serialize the row into a single string (e.g., JSON or XML).
- Encrypt that string as one column.

> **Note:** Querying individual fields is impossible without decrypting the whole thing — not ideal for performance or flexibility.

---
## 🧩 What Happens If You Lose the Server?

If you physically lose the server but still have:
- 	✅ The database files (MDF/LDF or backup)
- 	✅ The symmetric key (stored in the database)
- 	✅ The password used to encrypt the key
Then yes — you can decrypt the data on another SQL Server instance.
But if you lose the password or the master key, and you didn’t back it up, then the encrypted data is effectively lost. There’s no backdoor
---








