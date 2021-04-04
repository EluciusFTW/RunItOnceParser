using RioParser.Domain.Artefact;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Reports
{
    public class GenericReport : IReport
    {
        private IList<IReportArtefact> _artefacts = new List<IReportArtefact>();
        public GenericReport(IReadOnlyCollection<IReportArtefact> artefacts)
        {
            _artefacts = artefacts?.ToList() ?? throw new ArgumentNullException(nameof(artefacts));
        }

        public GenericReport(IReportArtefact artefact) 
            : this(new[] { artefact }) { }

        public GenericReport() { }

        public GenericReport AddArtefact(IReportArtefact artefact)
        {
            _artefacts.Add(artefact);
            return this;
        }

        public IEnumerable<IReportArtefact> Artefacts() => _artefacts.AsEnumerable();
    }
}
