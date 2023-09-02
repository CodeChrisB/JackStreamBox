using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacktStreamBox.Database.Model.Common
{
    public interface IIdentifiable
    {
        ulong Id { get; }
    }
}
