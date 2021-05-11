using System;

namespace CS.Common.Models
{
    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Fields { get; set; }
    }
}