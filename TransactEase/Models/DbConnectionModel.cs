using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactEase.Models
{
    public class DbConnectionModel
    {
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;

    }
}