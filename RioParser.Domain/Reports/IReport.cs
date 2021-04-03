using RioParser.Domain.Reports.Artefact;
using System.Collections.Generic;

namespace RioParser.Domain.Reports
{
    public interface IReport
    {
        public IEnumerable<IReportArtefact> Artefacts();
    }
}