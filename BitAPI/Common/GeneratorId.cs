using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI.Common
{
    public class GeneratorId
    {
        static long id = 1;
        public string GetNext()
        {
            return string.Format("{0:yyyyMMddHHmmss}-{1}", DateTime.Now, id++);
        }
    }
}
