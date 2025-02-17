using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library
{
    public class UtazasSzervezoDbContext : DbContext
    {
        public UtazasSzervezoDbContext(DbContextOptions<UtazasSzervezoDbContext> options)
           : base(options)
        {

        }
    }
}
