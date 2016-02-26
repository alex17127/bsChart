using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsChart
{
    public interface IDataObject
    {
        Range xRange();
        Range yRange();
    }
}
