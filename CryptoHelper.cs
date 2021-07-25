using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace FourDepack_cs.Security
{
    public class CryptoHelper
    {
        public static readonly string[] KnownDecryptionKeys = new string[] { "BAD0B46C-EDB8-4BAE-B538-F7C99556A023", "493589549485043859430889230823", "4dotsSoftware012301230123", "433424234234-93435849839453", "3434233454657676434234-92323454534095", "4235r4jk5je4jt3klejr4" };
        public static readonly uint CRC32_DefaultPolynomial = 0xEDB88320;

        public enum SearchType
        {
            EncryptionKeys,
            DecryptionKeys
        }

        public static byte[] MD5_GetHash(byte[] data)
        {
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            return md5provider.ComputeHash(data);
        }

        public static string[] FindKeys(FourDepack_cs.FourDots_PackedApplication packedAppInfo, SearchType keySearchType ) // If a decryption method is called, it adds the string present just before to the found keys array and returns the final array.
        {
            string callOperandStringCompare = "EncryptHelper::Encrypt";
            if (keySearchType == SearchType.DecryptionKeys) { callOperandStringCompare = "EncryptHelper::Decrypt";}
            List<string> foundDecryptionKeys = new List<string>();
            foreach (TypeDef t_ in packedAppInfo.TargetApplicationModuleMD.Types)
            {
                foreach (MethodDef methods in t_.Methods)
                {
                    if (!methods.HasBody) { continue; }
                    for (int x = 0; x < methods.Body.Instructions.Count - 1; x++)
                    {
                        Instruction inst = methods.Body.Instructions[x];
                        if (inst.OpCode == OpCodes.Call && methods.Body.Instructions[x-1].OpCode == OpCodes.Ldstr)
                        {
                            int tempInt = 0;
                            for (int x_ = x; x_ > 0; x_--)
                            {
                                Instruction inst_ = methods.Body.Instructions[x_];
                                if (inst_.OpCode == OpCodes.Ldstr) { tempInt = x_; break; }
                            }
                            if (inst.Operand.ToString().Contains(callOperandStringCompare)) { foundDecryptionKeys.Add(methods.Body.Instructions[tempInt].Operand.ToString()); }
                        }
                    }
                }
            }
            return foundDecryptionKeys.ToArray();
        }

        public static byte[] CRC32_GetHash(byte[] data, uint polynomial = 0xEDB88320)
        {
            // HashCore
            uint[] crcTable = CRC32_BuildTable(polynomial);
            uint crc = uint.MaxValue;
            foreach (byte byte_ in data)
			{
                ulong num = (ulong)((crc & 0xFFU) ^ (uint)byte_);
				crc >>= 8;
                crc ^= crcTable[(int)(checked((IntPtr)num))];
			}

            // HashFinal
            byte[] result = new byte[4];
            ulong crcXor = (ulong)crc ^ uint.MaxValue;
            result[0] = Convert.ToByte(crcXor & 0xFF);
            result[1] = Convert.ToByte(crcXor >> 8 & 0xFF);
            result[2] = Convert.ToByte(crcXor >> 0x10 & 0xFF);
            result[3] = Convert.ToByte(crcXor >> 0x18 & 0xFF);
            return result;
        }

        public static uint[] CRC32_BuildTable(uint polynomial)
        {
            uint[] array = new uint[0x100];
            for (int i = 0; i < 0x100; i++)
            {
                uint num = (uint)i;
                for (int j = 8; j > 0; j--)
                {
                    if ((num & 1U) == 1U) { num = (num >> 1 ^ polynomial); }
                    else { num >>= 1; }
                }
                array[i] = num;
            }
            return array;
        }

        public static byte[] DecryptBytes(byte[] data, string password) // "protected strings" are in Base64, just use the 'Convert.ToBase64String' method to convert the Base64 string to a decoded array of bytes.
        {
            TripleDESCryptoServiceProvider tripleDESprovider = new TripleDESCryptoServiceProvider();
            tripleDESprovider.Key = MD5_GetHash(new UTF8Encoding().GetBytes(password)); // Triple DES key = MD5 Hash of password
            tripleDESprovider.Mode = CipherMode.ECB;
            tripleDESprovider.Padding = PaddingMode.PKCS7;
            try
            {
                ICryptoTransform CryptoTransform = tripleDESprovider.CreateDecryptor();
                return CryptoTransform.TransformFinalBlock(data, 0, data.Length); // decrypts & returns data
            }
            catch { throw; }
        }
    }
}