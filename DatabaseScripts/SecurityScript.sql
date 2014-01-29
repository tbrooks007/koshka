/* Create Database Master Key */
CREATE MASTER KEY ENCRYPTION
BY PASSWORD = 'BitsBlogSuperDBAdmin' --should change this and make your own!

-- generate self-signed X.509 certificate
CREATE CERTIFICATE BitsBlogEncryptCert
WITH SUBJECT = 'BitsBlogSuperDBAdmin'

-- generate key and encrypt it with the above certificate
CREATE SYMMETRIC KEY BitsBlogTableKey
	WITH ALGORITHM = AES_256,
	KEY_SOURCE = 'When I left you, I was but the learner, now I am the master...',IDENTITY_VALUE = 'Hail, Lord Vader!'
	ENCRYPTION BY CERTIFICATE BitsBlogEncryptCert
	
--Example: Encrypte a column (1st decrypt our key so we can use it to encrypt the column data)
--OPEN SYMMETRIC KEY BitsBlogTableKey DECRYPTION
--BY CERTIFICATE BitsBlogEncryptCert
--UPDATE TestTable
--SET EncryptSecondCol = ENCRYPTBYKEY(KEY_GUID('BitsBlogTableKey'),SecondCol)

--Example: Decrypt 
-- OPEN SYMMETRIC KEY BitsBlogTableKey DECRYPTION
--BY CERTIFICATE BitsBlogEncryptCert
--SELECT CONVERT(VARCHAR(50),DECRYPTBYKEY(columnname)) AS DecryptSecondCol
--FROM TestTable

GO