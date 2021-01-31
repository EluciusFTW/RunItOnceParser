using RioParser.Domain.Reports;
using System.Collections.Generic;

namespace RioParser.Domain.Sessions
{
    public class Cub3dSession : TourneySession, IReport
    {
        public Cub3dSession(string name, string content)
            : base(name, content)
        {
        }

        public IEnumerable<string> PrintOut()
        {
            throw new System.NotImplementedException();
        }
    }
}
