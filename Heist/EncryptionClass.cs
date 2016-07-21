using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Heist
{
    class EncryptionClass
    {

        public byte[] Encrypt(byte[] data)
        {

            String strMsg = String.Empty;           // Message string
            String strAlgName = String.Empty;       // Algorithm name
            UInt32 keyLength = 0;                   // Length of key
            BinaryStringEncoding encoding;          // Binary encoding type
            IBuffer iv;                             // Initialization vector
            CryptographicKey key;                   // Symmetric Key

            strMsg = BitConverter.ToString(data);
            strAlgName = SymmetricAlgorithmNames.AesCbcPkcs7;
            keyLength = 32;

            IBuffer buffEncrypted = this.SampleCipherEncryption(
              strMsg,
              strAlgName,
              keyLength,
              out encoding,
              out iv,
              out key);

            Stream str = buffEncrypted.AsStream();
            byte[] b = buffEncrypted.ToArray();
            return b;

        }

        
        //public byte[] Decrypt(byte[] data)
        //{
        //    byte[] bi = data;

        //    String strMsg = String.Empty;           // Message string
        //    String strAlgName = String.Empty;       // Algorithm name
        //    UInt32 keyLength = 0;                   // Length of key
        //    BinaryStringEncoding encoding;          // Binary encoding type
        //    IBuffer iv;                             // Initialization vector
        //    CryptographicKey key;                   // Symmetric Key

        //    strMsg = BitConverter.ToString(data);
        //    strAlgName = SymmetricAlgorithmNames.AesCbcPkcs7;
        //    keyLength = 32;


        //    IBuffer buffEncrypted = bi.AsBuffer();
        //    this.SampleCipherDecryption(
        //        strAlgName,
        //        buffEncrypted,
        //        iv,
        //        encoding,
        //        key);
        //}


        private IBuffer SampleCipherEncryption(
         String strMsg,
         String strAlgName,
         UInt32 keyLength,
         out BinaryStringEncoding encoding,
         out IBuffer iv,
         out CryptographicKey key)
        {
            // Initialize the initialization vector.
            iv = null;

            // Initialize the binary encoding value.
            encoding = BinaryStringEncoding.Utf8;

            // Create a buffer that contains the encoded message to be encrypted. 
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(strMsg, encoding);

            // Open a symmetric algorithm provider for the specified algorithm. 
            SymmetricKeyAlgorithmProvider objAlg = SymmetricKeyAlgorithmProvider.OpenAlgorithm(strAlgName);

            // Determine whether the message length is a multiple of the block length.
            // This is not necessary for PKCS #7 algorithms which automatically pad the
            // message to an appropriate length.
            if (!strAlgName.Contains("PKCS7"))
            {
                if ((buffMsg.Length % objAlg.BlockLength) != 0)
                {
                    throw new Exception("Message buffer length must be multiple of block length.");
                }
            }

            // Create a symmetric key.
            byte[] br = new byte[] { 3, 9, 4, 5, 6, 7, 6, 8, 3, 9, 4, 6, 5, 4, 1, 5, 5, 2, 7, 5, 7, 6, 8, 1, 6, 5, 2, 3, 1, 6, 4, 3 };
            IBuffer keyMaterial = CryptographicBuffer.CreateFromByteArray(br);
            key = objAlg.CreateSymmetricKey(keyMaterial);

            // CBC algorithms require an initialization vector. Here, a random
            // number is used for the vector.
            if (strAlgName.Contains("CBC"))
            {
                iv = CryptographicBuffer.GenerateRandom(objAlg.BlockLength);
            }

            // Encrypt the data and return.
            IBuffer buffEncrypt = CryptographicEngine.Encrypt(key, buffMsg, iv);
            return buffEncrypt;
        }

        private void SampleCipherDecryption(
          String strAlgName,
          IBuffer buffEncrypt,
          IBuffer iv,
          BinaryStringEncoding encoding,
          CryptographicKey key)
        {
            // Declare a buffer to contain the decrypted data.
            IBuffer buffDecrypted;

            // Open an symmetric algorithm provider for the specified algorithm. 
            SymmetricKeyAlgorithmProvider objAlg = SymmetricKeyAlgorithmProvider.OpenAlgorithm(strAlgName);

            // The input key must be securely shared between the sender of the encrypted message
            // and the recipient. The initialization vector must also be shared but does not
            // need to be shared in a secure manner. If the sender encodes a message string 
            // to a buffer, the binary encoding method must also be shared with the recipient.
            buffDecrypted = CryptographicEngine.Decrypt(key, buffEncrypt, iv);

            // Convert the decrypted buffer to a string (for display). If the sender created the
            // original message buffer from a string, the sender must tell the recipient what 
            // BinaryStringEncoding value was used. Here, BinaryStringEncoding.Utf8 is used to
            // convert the message to a buffer before encryption and to convert the decrypted
            // buffer back to the original plaintext.
            String strDecrypted = CryptographicBuffer.ConvertBinaryToString(encoding, buffDecrypted);
        }

    }
}
