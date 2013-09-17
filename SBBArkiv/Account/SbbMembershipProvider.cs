using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Policy;
using System.Security.Cryptography;
using System.Text;

namespace SBBArkiv
{
    public class SbbMembershipProvider : MembershipProvider
    {
        private const int minRequiredPasswordLength = 6;
        private const int minRequiredNonAlphanumericCharacters = 0;
        private const bool enablePasswordRetrieval = false;
        private const bool enablePasswordReset = true;
        private const bool requiresQuestionAndAnswer = false;
        private string applicationName = "SBB Notearkiv";
        private const int maxInvalidPasswordAttempts = 3;
        private const int passwordAttemptWindow = 10;
        private const bool requiresUniqueEmail = true;
        private const MembershipPasswordFormat passwordFormat = new MembershipPasswordFormat();
        private const string passwordStrengthRegularExpression = "";
        private const string passwordSalt = "#¤%&/&%¤#6846546854afughahjkfg";

        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
            {
                return false;
            }

            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
            User user = ctx.Users.FirstOrDefault(o => o.UserName == username);
            user.Password = GetHashedPassword(newPassword);
            ctx.SaveChanges();

            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { return enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return enablePasswordRetrieval; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            User user;

            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
            user = ctx.Users.FirstOrDefault(o => o.UserName == username);

            if (user != null)
            {
                return new MembershipUser(this.GetType().Name, user.UserName, user.Id, user.Email, null, null, true, user.Inactive, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            }
            else //user not found
            {
                return null;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[minRequiredPasswordLength];
            Random rd = new Random();

            for (int i = 0; i < minRequiredPasswordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            string randomPassword = new string(chars);

            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
            User user = ctx.Users.FirstOrDefault(o => o.UserName == username);
            user.Password = GetHashedPassword(randomPassword);
            ctx.SaveChanges();

            return randomPassword;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            bool validated = false;

            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
            User user = ctx.Users.Where(o => !o.Inactive).FirstOrDefault(o => o.UserName == username);

            if (user != null)
            {
                if (GetHashedPassword(password) == user.Password)
                {
                    validated = true;
                }
            }
            
            return validated;
        }

        private string GetHashedPassword(string password)
        {
            HashAlgorithm hash = new SHA512Managed();

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(passwordSalt);
            byte[] combined = new byte[passwordBytes.Length + saltBytes.Length];

            for (int i = 0; i < passwordBytes.Length; i++)
            {
                combined[i] = passwordBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                combined[i + passwordBytes.Length] = saltBytes[i];
            }

            byte[] hashed = hash.ComputeHash(combined);

            return Convert.ToBase64String(hashed);
        }
    }
}