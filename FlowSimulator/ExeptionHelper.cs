using System;

namespace FlowSimulator
{
    static internal class ExeptionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static Exception GetFirstException(Exception ex)
        {
            Exception e = ex;

            //only the first exception is useful
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }

            return e;
        }
    }
}