using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity
{
    public class Activity : IActivity
    {
        protected string funtionName = string.Empty;

        public static IActivity CreateActivity(string functionName)
        {
            return new LaunchFreeMyApp();
        }

        public virtual void Start()
        {

        }

        public virtual  void Finish()
        {

        }

        public virtual void End()
        {

        }
    }
}
