using RealEstate.Core.Interfaces.Repository;
using RealEstate.Core.Interfaces.Services;
using RealEstate.Core.Interfaces.Utils;
using RealEstate.Core.Options;
using RealEstate.Core.Services.Imp;
using RealEstate.Core.Services.Utils;
using RealEstate.Infraestructure.Repositories;

namespace RealEstate.API.App_Start
{

    internal static class DIConfiguration
    {
        internal static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddTransient(typeof(IReplaceVisitor), typeof(ReplaceVisitor));
            services.AddTransient(typeof(ITreeModifier), typeof(TreeModifier));
            services.AddTransient(typeof(IPasswordService), typeof(PasswordService));
            services.AddTransient(typeof(IFileService), typeof(FileService));

            services.AddTransient(typeof(IAuthService), typeof(AuthService));
            services.AddTransient(typeof(IPropertyService), typeof(PropertyService));
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));
            services.Configure<PaginationOptions>(configuration.GetSection("PaginationOptions"));

            return services;
        }
    }
}
