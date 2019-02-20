using Api.Persistence;

namespace Api.Service
{
    public class AccessControlService : IAccessControlService
    {
        private SecurityDbContext _securityDbContext;

        public AccessControlService(SecurityDbContext securityDbContext)
        {
            _securityDbContext = securityDbContext;
        }
    }
}
