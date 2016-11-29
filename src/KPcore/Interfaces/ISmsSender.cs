using System.Threading.Tasks;

namespace KPcore.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
