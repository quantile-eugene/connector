using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BitAPI.Bitmex
{
    public class ErrorMessage
    {
        public string Message;

        public static ErrorMessage ReadFromJObject(JToken aValue)
        {
            ErrorMessage msg = null;
            JToken error = aValue.SelectToken("error");
            if (error != null)
            {
                JValue message = (JValue)error.SelectToken("message");
                if (message != null)
                {
                    msg = new ErrorMessage()
                    {
                        Message = (string)message.Value
                    };
                }
                else
                {
                    msg = new ErrorMessage()
                    {
                        Message = (string)((JValue)error).Value
                    };
                }
            }
            return msg;
        }
    }
}
