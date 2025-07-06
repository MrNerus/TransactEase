using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactEase.Enums;

namespace TransactEase.DTO
{
    public class UserResponse
    {
        public string Message { get; set; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.INFO;
        public dynamic Data { get; set; } = new { };
    }
}