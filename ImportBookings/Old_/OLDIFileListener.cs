using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportBookings.Domain
{
    public interface IFileListener
    {
        void StartListening(Guid id, string directory, string filter, Action<string> fileArrived);
        void StopListeners();
        void StopListener(Guid id);

    }
}
