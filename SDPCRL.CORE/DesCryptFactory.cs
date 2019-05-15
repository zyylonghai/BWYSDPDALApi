using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SDPCRL.CORE
{
    public class DesCryptFactory
    {
        // 创建Key
        public static string GenerateKey()
        {

            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            string key = ASCIIEncoding.ASCII.GetString(desCrypto.Key);
            while (key.Contains(SysConstManage.DBInfoArraySeparator) || key.Contains(SysConstManage.DBInfoArraySeparator2) || key.Contains("'"))
            {
                desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
                key = ASCIIEncoding.ASCII.GetString(desCrypto.Key);
            }
            return key;
            ////desCrypto.Key = new byte[] {0,2,3,4};
            //return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }
        // 加密字符串
        public static string EncryptString(string sInputString, string sKey)
        {

            byte[] data = ASCIIEncoding.ASCII.GetBytes(sInputString);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            //DES.Key = StrinTobyte(sKey);
            //DES.IV = StrinTobyte(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return BitConverter.ToString(result);
        }
        // 解密字符串
        public static string DecryptString(string sInputString, string sKey)
        {
            string[] sInput = sInputString.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateDecryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }
        /// <summary>
        /// 3des加密
        /// </summary>
        /// <param name="aStrString"></param>
        /// <param name="aStrKey"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string Encrypt3Des(string aStrString, string aStrKey, CipherMode mode = CipherMode.ECB)
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = StrinTobyte(aStrKey),
                    Mode = mode
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = StrinTobyte(aStrKey);
                }
                var desEncrypt = des.CreateEncryptor();
                byte[] buffer = StrinTobyte(aStrString);
                return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public static byte[] Encrypt3Destobyte(string aStrString, string aStrKey, CipherMode mode = CipherMode.ECB)
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = StrinTobyte(aStrKey),
                    Mode = mode
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = StrinTobyte(aStrKey);
                }
                var desEncrypt = des.CreateEncryptor();
                byte[] buffer = StrinTobyte(aStrString);
                return desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 3des解密
        /// </summary>
        /// <param name="aStrString"></param>
        /// <param name="aStrKey"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string Decrypt3Des(string aStrString, string aStrKey, CipherMode mode = CipherMode.ECB)
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = StrinTobyte(aStrKey),
                    Mode = mode,
                    //Padding = PaddingMode.PKCS7
                };
                //if (mode == CipherMode.CBC)
                //{
                //    des.IV = StrinTobyte(aStrKey);
                //}
                var desDecrypt = des.CreateDecryptor();
                var result = "";
                byte[] buffer = Convert.FromBase64String(aStrString);
                result = Encoding.UTF8.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                return result;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey">加密密钥</param>
        public static string AESEncrypt(string EncryptString, string EncryptKey)
        {
            return Convert.ToBase64String(AESEncrypt(Encoding.Default.GetBytes(EncryptString), EncryptKey));
        }

        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey">加密密钥</param>
        public static byte[] AESEncrypt(byte[] EncryptByte, string EncryptKey)
        {
            if (EncryptByte.Length == 0) { throw (new Exception("明文不得为空")); }
            if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }
            byte[] m_strEncrypt;
            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQyy");
            byte[] m_salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
            Rijndael m_AESProvider = Rijndael.Create();
            try
            {
                MemoryStream m_stream = new MemoryStream();
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(EncryptKey, m_salt);
                ICryptoTransform transform = m_AESProvider.CreateEncryptor(pdb.GetBytes(32), m_btIV);
                CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);
                m_csstream.Write(EncryptByte, 0, EncryptByte.Length);
                m_csstream.FlushFinalBlock();
                m_strEncrypt = m_stream.ToArray();
                m_stream.Close(); m_stream.Dispose();
                m_csstream.Close(); m_csstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_AESProvider.Clear(); }
            return m_strEncrypt;
        }
        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        public static byte[] AESDecrypt(byte[] DecryptByte, string DecryptKey)
        {
            if (DecryptByte.Length == 0) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
            byte[] m_strDecrypt;
            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQyy");
            byte[] m_salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
            Rijndael m_AESProvider = Rijndael.Create();
            try
            {
                MemoryStream m_stream = new MemoryStream();
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(DecryptKey, m_salt);
                ICryptoTransform transform = m_AESProvider.CreateDecryptor(pdb.GetBytes(32), m_btIV);
                CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);
                m_csstream.Write(DecryptByte, 0, DecryptByte.Length);
                m_csstream.FlushFinalBlock();
                m_strDecrypt = m_stream.ToArray();
                m_stream.Close(); m_stream.Dispose();
                m_csstream.Close(); m_csstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_AESProvider.Clear(); }
            return m_strDecrypt;
        }

        private static byte[] StrinTobyte(string str)
        {
            byte[] bytearray = new byte[str.Length / 2];
            int index = 0;
            for (int i = 0; i < str.Length; i += 2)
            {
                bytearray[index] = byte.Parse(str.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                index++;
            }
            return bytearray;
        }

        public static void test()
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            //ass.GetName().KeyPair;
        }
    }
}
