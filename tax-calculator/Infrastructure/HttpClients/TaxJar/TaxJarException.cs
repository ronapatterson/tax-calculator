namespace Infrastructure.HttpClients.TaxJar
{
    internal class TaxJarException : Exception
    {
        public TaxJarException() : base()
        {
        }

        public TaxJarException(string message) : base(message)
        {
        }

        public TaxJarException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
