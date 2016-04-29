using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportBookings
{
    public class Report
    {
        public DateTime MissingFiles { get; set; }
    }

    public class Reporter : IObserver<Report>
    {
        private IDisposable _unsubscriber;
        private IList<Report> _reports = new List<Report>();

        public IList<Report> Reports => _reports;

        public virtual void Subscribe(IObservable<Report> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }
        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Report value)
        {
            _reports.Add(value);
        }
    }
}
