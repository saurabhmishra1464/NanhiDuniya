using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.MessageBus
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
