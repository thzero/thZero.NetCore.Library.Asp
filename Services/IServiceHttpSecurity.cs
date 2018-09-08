using System;

using Microsoft.AspNetCore.Builder;

namespace thZero.Services
{
    public interface IServiceHttpSecurity : IService
    {
        void InitializeSsl(IApplicationBuilder app);
        void InitializeStaticPost(IApplicationBuilder app);
        void InitializeStaticPre(IApplicationBuilder app);
    }
}
