using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FocusSpace.DatabaseContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=FocusSpace2;User Id=postgres;Password=240505;");
            return new DataContext(optionsBuilder.Options);
        }
    }
}
