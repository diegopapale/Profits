using Microsoft.EntityFrameworkCore;

namespace Cadastro_de_Documentos.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
    }
}
