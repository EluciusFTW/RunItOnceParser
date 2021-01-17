using System.Collections.Generic;

namespace RioParser.Domain.Reports
{
    public interface IHandsReport
    {
        public IEnumerable<string> PrintOut();
    }
}