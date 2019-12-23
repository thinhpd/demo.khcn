using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Utils
{
    /// <summary>
    /// Class message dùng để serialization 
    /// </summary>
    /// <Modified>        
    ///	Name		Date		    Comment 
    /// QuangND     16/11/2010      Tạo mới
    /// </Modified>
    [Serializable]
    public class JsonMessage
    {
        #region các biến
        private bool _erros;
        private string _message;
        private string _ID;
        #endregion

        #region Thuộc tính
        public bool Erros
        {
            get { return _erros; }
            set { _erros = value; }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public object Object { get; set; }
        #endregion

        #region Constructer
        public JsonMessage()
        {
            _erros = false;
            _message = string.Empty;
            _ID = string.Empty;
        }

        public JsonMessage(bool erros, string message)
        {
            this._erros = erros;
            this._message = message;
        }
        #endregion

    }
}
