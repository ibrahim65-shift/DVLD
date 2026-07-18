using DVLD_Buisness;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DVLD.Classes
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;
        private static readonly string RegistryKeyPath = @"Software\DVLD1";
        // اجعل الـ Entropy أطول قليلاً لزيادة التعقيد (يمكن تحسينه أكثر)
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("DVLD1-Entropy-ChangeThis");

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                {
                    if (key == null)
                    {
                        MessageBox.Show("Failed To Create Registry Key!", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    key.SetValue("Username", Username ?? string.Empty);

                    if (!string.IsNullOrEmpty(Password))
                    {
                        byte[] encryptedPassword = ProtectedData.Protect(
                            Encoding.UTF8.GetBytes(Password),
                            Entropy,
                            DataProtectionScope.CurrentUser);

                        key.SetValue("PasswordEncrypted", encryptedPassword, RegistryValueKind.Binary);
                    }
                    else
                    {
                        if (key.GetValue("PasswordEncrypted") != null)
                            key.DeleteValue("PasswordEncrypted", false);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                clsUtil.WriteToEventLog(ex.Message, EventLogEntryType.Error);
                MessageBox.Show($"Error : {ex.Message}");
                return false;
            }
        }

        // يعيد true فقط إذا تم استرجاع كلمة المرور بنجاح (وهي نفس سلوكك الحالي)
        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
                {
                    if (key == null)
                        return false;

                    object usernameValue = key.GetValue("Username");
                    if (usernameValue != null)
                        Username = usernameValue.ToString();

                    object passwordValue = key.GetValue("PasswordEncrypted");
                    if (passwordValue is byte[] encryptedPassword)
                    {
                        byte[] decryptedData = ProtectedData.Unprotect(
                            encryptedPassword,
                            Entropy,
                            DataProtectionScope.CurrentUser);

                        Password = Encoding.UTF8.GetString(decryptedData);
                        return true; // نجحنا في جلب كل شيء
                    }

                    // إن أردت: لو الاسم موجود لكن الباسوورد غير موجود، يمكنك إرجاع true هنا
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsUtil.WriteToEventLog(ex.Message, EventLogEntryType.Error);
                MessageBox.Show($"Error : {ex.Message}");
                return false;
            }
        }

        public static void DeleteCredentialsFromRegistry()
        {
            try
            {
                // نتحقق أولا إن كان المفتاح موجوداً قبل الحذف لتجنب Exception
                using (var root = Registry.CurrentUser)
                {
                    using (var sub = root.OpenSubKey(RegistryKeyPath))
                    {
                        if (sub != null)
                        {
                            root.DeleteSubKey(RegistryKeyPath, false); // false => لا يرمي إذا لم يوجد
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsUtil.WriteToEventLog(ex.Message, EventLogEntryType.Error);
                MessageBox.Show($"Error : {ex.Message}");
            }
        }
    }
}