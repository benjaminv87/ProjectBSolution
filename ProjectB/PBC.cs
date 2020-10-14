﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    public class PBC
    {
        public static string ComputeHash(string rawData)
        {
            using (SHA512 mySHA512 = SHA512.Create())
            {

                byte[] bytes = mySHA512.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var item in bytes)
                {
                    builder.Append(item.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
