using JacktStreamBox.Database.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacktStreamBox.Database.Model
{
    public class Player : IIdentifiable
    {
        public ulong Id { get; set; }
        public ulong PlayXP { get; set; }
    }
}
