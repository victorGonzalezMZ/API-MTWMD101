﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MTWDM101.Models
{
    public class UserMin
    {
        public int ID { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public String Email { get; set; }


    }
    public class User : UserMin
    {
        public DateTime CreateDate { get; set; }
        public AccountType accountType { get; set; }
    }

    public enum AccountType : int
    {
        Basic = 0,
        Administrator = 1,
    }
}
