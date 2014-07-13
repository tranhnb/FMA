using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity.Activities
{
    /// <summary>
    /// Return Value for Activity
    /// </summary>
    public class ActivityResult
    {
        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }
        }

        private string _ErrorMessage = string.Empty;
        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
        }

        public ActivityResult(bool status, string errMessage)
        {
            this._Status = status;
            this._ErrorMessage = errMessage;
        }
    }
}
