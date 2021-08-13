using ChallengeDisneyOctavioLafourcadePre_Aceleracion.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChallengeDisneyOctavioLafourcadePre_Aceleracion.Context
{
    
    public class UserContext : IdentityDbContext<User>
    {
        private const string Schema = "users";
        //Agregando un constructor
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(Schema);
        }
    }
}
