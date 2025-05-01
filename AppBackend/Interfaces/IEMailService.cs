namespace AppBackend.Interfaces{
public interface IEMailService
{
    Task SendEmail(string to, string subject, string body,byte[] attachment=null,string attachmentFileName=null);
}

}
