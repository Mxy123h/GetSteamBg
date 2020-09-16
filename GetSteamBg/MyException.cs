using System;
using System.Collections.Generic;
using System.Text;

namespace GetSteamBg
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public class MyException : ApplicationException
    {
        public MyException(string errorcode, string message, object list) : base(message)
        {
            Errorcode = errorcode;
            List = list;
        }

        public MyException(string myMessage, string message) : base(message)
        {
            MyMessage = myMessage;
            message = message;
        }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        public string Errorcode { get; }
        public string MyMessage { get; }
        public object List { get; }
    }

    public class HISOrderCancleException : ApplicationException
    {
        public HISOrderCancleException(string errorcode, string message) : base(message)
        {
            Errorcode = errorcode;
        }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        public string Errorcode { get; }
    }
}
