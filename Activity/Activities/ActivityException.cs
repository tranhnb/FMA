using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity
{
    public class ActivityException : Exception
    {

    }

    public class LaunchApplicationException : ActivityException
    { 
        
    }

    /// <summary>
    /// FMA has no application to download
    /// </summary>
    public class NoApplicationException : ActivityException
    { 
    }
}
