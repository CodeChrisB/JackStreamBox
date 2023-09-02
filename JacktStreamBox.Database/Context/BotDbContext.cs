using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacktStreamBox.Database.Context
{
    public class BotDbContext
    {
        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

    }
}
