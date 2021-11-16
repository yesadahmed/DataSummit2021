using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace V3DurableCore3CrmTemplate
{
    public interface ICrmTokenService
    {
        Task<string> GetCRMTokenAsync();
    }
}
