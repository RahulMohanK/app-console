using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;
namespace ModelLibrary{
  public class User
    {
        public String Username
        {
            get;
            set;
        }
        public String Password
        {
            get;
            set;
        }
    }
    public class ChangeUser
    {
        public String Username
        {
            get;
            set;
        }
        public String OldPassword
        {
            get;
            set;
        }
          public String NewPassword
        {
            get;
            set;
        }
    }
}