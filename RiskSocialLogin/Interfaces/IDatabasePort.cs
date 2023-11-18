using RiskSocialLogin.Domain;

namespace RiskSocialLogin.Interfaces
{
    public interface IDatabasePort
    {
        Response? Login(Request request);
    }
}
