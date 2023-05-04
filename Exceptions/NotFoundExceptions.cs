namespace BankListAPI.VsCode.Exceptions
{
    public class NotFoundExceptions: ApplicationException
    {
        public NotFoundExceptions(string name, object key) : base($"{name} ({key}) was not found")
        {

        }
    }
}
