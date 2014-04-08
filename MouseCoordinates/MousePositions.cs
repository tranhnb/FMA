using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

namespace MouseCoordinates
{
    /// <summary>
    /// Map mouse position with function
    /// E.g: 
    /// </summary>
    public class MousePosition
    {
        
        public int X;
        
        public int Y;
        
        public string FunctionName = String.Empty;
        
    }

    /// <summary>
    /// Manage MousePosition
    /// </summary>
    ///
    public class MousePositionList : Hashtable
    {
        private const string XMLMousePositionFileName = "MousePosition.xml";

        /// <summary>
        /// Get MousePosition by FunctionName
        /// </summary>
        /// <param name="functionName"></param>
        public MousePosition GetMousePosition(string functionName)
        {
            return (MousePosition)this[functionName];
        }

        /// <summary>
        /// Initialize Mouse coordinates data
        /// </summary>
        public MousePositionList()
        {
            //Read XML to String

            string fileName = System.Configuration.ConfigurationManager.AppSettings["MousePositionFile"];
            try
            {
                System.Xml.XmlReader reader = System.Xml.XmlReader.Create(fileName);
                XmlSerializer serializer = new XmlSerializer(typeof(MousePosition[]));
                MousePosition[] mousePositionArray = (MousePosition[])serializer.Deserialize(reader);

                for (int i = 0, len = mousePositionArray.Length; i < len; i++)
                {
                    this.Add(mousePositionArray[i].FunctionName, mousePositionArray[i]);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }
    }


}


