using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model.MaterialStock;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NDE_Digital_Market.SharedServices
{
    public class CommonServices
    {

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012"); // 32 bytes for AES-256
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes for AES


        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        //public readonly string FilesPath = @"F:\Projects\Health Care\healthcare-frontend\src\assets\images\";
        //public static string FilesPath { get; } = @"F:\Projects\Health Care\healthcare-frontend\src\assets\images\";
        private static readonly string EncryptionKey = "MAKV2SPBNI99212";
        private static readonly byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        public string FilesPath { get; set; }
        public string Filespath1 { get; set; }
        public string HealthCareConnection { get; set; }
        private readonly IConfiguration _configuration;
        private readonly SqlConnection con;
        //private static readonly string EncryptionKey = "MAKV2SPBNI99212"; // Your encryption key
        //private static readonly string EncryptionKey = "your-encryption-key";

        public CommonServices(IConfiguration configuration)
        {
            _configuration = configuration;
            FilesPath = _configuration["FilesPaths:filepath"];
            //Filespath1 = _configuration["Filespaths:filepath" ];
            HealthCareConnection = _configuration.GetConnectionString("HealthCare");

        }


        public string GetConnectionString()
        {
            return HealthCareConnection = _configuration.GetConnectionString("HealthCare");
        }

        public static string EncryptPassword(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string DecryptPassword(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        //public static string EncryptPassword(string input)
        //{
        //    // Encode the string as a byte array using UTF-8 encoding
        //    byte[] bytes = Encoding.UTF8.GetBytes(input);

        //    // Convert the byte array to a hexadecimal string with uppercase characters
        //    return BitConverter.ToString(bytes).ToUpper();
        //}

        //public static string DecryptPassword(string hex)
        //{
        //    // Remove any non-hexadecimal characters
        //    string cleanHex = Regex.Replace(hex, "[^0-9A-F]", "");

        //    // Split the string into byte pairs
        //    int length = cleanHex.Length;
        //    byte[] bytes = new byte[length / 2];
        //    for (int i = 0; i < length; i += 2)
        //    {
        //        bytes[i / 2] = Convert.ToByte(cleanHex.Substring(i, 2), 16);
        //    }

        //    // Decode the byte array back to a string using UTF-8 encoding
        //    return Encoding.UTF8.GetString(bytes);
        //}




        //public static string EncryptPassword(string clearText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    // Padding the input string to make it 64 bits (8 bytes) long
        //    clearText = clearText.PadLeft(7, '0');
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //}

        //public static string DecryptPassword(string cipherText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            string clearText = Encoding.Unicode.GetString(ms.ToArray());
        //            // Removing padding added during encryption
        //            clearText = clearText.TrimStart('0');
        //            return clearText;
        //        }
        //    }
        //}




        //public static string EncryptPassword(string clearText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}

        //////DecryptPassword
        //public static string DecryptPassword(string cipherText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}


        //public static string UploadFiles(string foldername, string filename, IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return null;
        //    }

        //    // Get the original file name with extension
        //    string fileNameWithExtension = file.FileName;

        //    // Generate a unique file name
        //    var uniqueFileName = GetUniqueFileName(filename, fileNameWithExtension);

        //    // Combine the unique file name with the upload path
        //    var filePath = Path.Combine(foldername, uniqueFileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        file.CopyTo(fileStream);
        //    }
        //    return filePath;

        //}

        // ================= compress image ======================================


        public static string UploadFiles(string foldername, string filename, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Get the original file name with extension
            string fileNameWithExtension = file.FileName;

            // Generate a unique file name
            var uniqueFileName = GetUniqueFileName(filename, fileNameWithExtension);

            // Combine the unique file name with the upload path
            var filePath = Path.Combine(foldername, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Load the image using ImageSharp
                using (var image = Image.Load(file.OpenReadStream()))
                {
                    // Check if the image size is greater than 2MB
                    if (file.Length > 2 * 1024 * 1024)
                    {
                        // Resize the image to fit within 2MB
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(2048, 2048), // Adjust the size as needed
                            Mode = ResizeMode.Max
                        }));

                        // Save the resized image as JPEG with compression quality 80
                        image.Save(fileStream, new JpegEncoder
                        {
                            Quality = 80
                        });
                    }
                    else
                    {
                        // If the image is already smaller than 2MB, save it as is
                        file.CopyTo(fileStream);
                    }
                }
            }

            return filePath;
        }

        //public static string UploadFiles(string foldername, string filename, IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return null;
        //    }

        //    string fileNameWithExtension = file.FileName;
        //    var uniqueFileName = GetUniqueFileName(filename, fileNameWithExtension);
        //    var filePath = Path.Combine(foldername, uniqueFileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        // Load the image using ImageSharp
        //        using (var image = Image.Load(file.OpenReadStream()))
        //        {
        //            // Check if the image size is greater than 2MB
        //            if (file.Length > 2 * 1024 * 1024)
        //            {
        //                // Resize the image to fit within 2MB
        //                image.Mutate(x => x.Resize(new ResizeOptions
        //                {
        //                    Size = new Size(2048, 2048),
        //                    Mode = ResizeMode.Max
        //                }));
        //            }

        //            // Get the original file extension
        //            var originalExtension = Path.GetExtension(fileNameWithExtension).ToLowerInvariant();

        //            // Save the image using the appropriate encoder based on the file extension
        //            switch (originalExtension)
        //            {
        //                case ".jpg":
        //                case ".jpeg":
        //                    image.Save(fileStream, new JpegEncoder());
        //                    break;

        //                case ".png":
        //                    image.Save(fileStream, new PngEncoder());
        //                    break;



        //                // Add other formats as needed

        //                default:
        //                    // Use a default encoder or handle the case accordingly
        //                    image.Save(fileStream, new JpegEncoder());
        //                    break;
        //            }
        //        }
        //    }

        //    return filePath;
        //}



        public static byte[] GetFiles(string filepath)
        {
            if (!System.IO.File.Exists(filepath))
            {
                return null;
            }

            return System.IO.File.ReadAllBytes(filepath);
        }


        // Generate a unique file name to avoid overwriting existing files
        private static string GetUniqueFileName(string fileName, string fileNameWithExtension)
        {
            //string fileNameWE = Path.GetFileNameWithoutExtension(fileName, );
            return $"{fileName}_{DateTime.Now.Ticks}{Path.GetExtension(fileNameWithExtension)}";
        }


        //====================== Token code added by utshow ======================================
        public static void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        public static bool ArePasswordHashesEqual(byte[] passwordHash1, byte[] passwordSalt1, byte[] passwordHash2, byte[] passwordSalt2)
        {
            // Ensure the length of both password hashes and salts are equal
            if (passwordHash1.Length != passwordHash2.Length || passwordSalt1.Length != passwordSalt2.Length)
            {
                return false;
            }

            // Compare each byte of the password hash and salt
            for (int i = 0; i < passwordHash1.Length; i++)
            {
                if (passwordHash1[i] != passwordHash2[i])
                {
                    return false;
                }
            }

            for (int i = 0; i < passwordSalt1.Length; i++)
            {
                if (passwordSalt1[i] != passwordSalt2[i])
                {
                    return false;
                }
            }

            return true;
        }


    }
}
