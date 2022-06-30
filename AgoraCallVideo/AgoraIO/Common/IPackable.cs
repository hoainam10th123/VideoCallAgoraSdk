using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraIO.Media
{
    public interface IPackable
    {
        ByteBuf marshal(ByteBuf outBuf);
    }
}
