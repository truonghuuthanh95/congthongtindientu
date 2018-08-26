using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Models.DTO
{
    public class ReturnFormat
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Results { get; set; }

        public ReturnFormat(int statusCode, string message, object results)
        {
            StatusCode = statusCode;
            Message = message;
            Results = results;
        }
    }
}