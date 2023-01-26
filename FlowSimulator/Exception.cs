namespace FlowSimulator
{
    internal static class Exception
    {
        public static System.Exception GetFirstException(System.Exception ex)
        {
            System.Exception e = ex;

            while (e.InnerException != null)
            {
                e = e.InnerException;
            }

            return e;
        }
    }
}