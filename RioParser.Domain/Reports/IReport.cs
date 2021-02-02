using System.Collections.Generic;

namespace RioParser.Domain.Reports
{
    public interface IReport
    {
        public IEnumerable<string> PrintOut();
    }
}