using Ical.Net;
using Ical.Net.DataTypes;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OperatingHours
{
    public class CalFeed: ICalFeed, IDisposable
    {
        public Uri Uri { get; protected set; }
        private CalendarCollection _calendarCollection;
        private Calendar _firstCalendar;
        public CalFeed(Uri link)
        {
            Uri = link;
        }
        private async Task GetICalFeedAsync()
        {
            _calendarCollection = await LoadFromUriAsync(Uri);
            _firstCalendar = _calendarCollection.First() as Calendar;
        }
        private async Task<CalendarCollection> LoadFromUriAsync(Uri uri)
        {
            CalendarCollection calCollection;
            if (uri.AbsoluteUri.StartsWith(@"http://") || uri.AbsoluteUri.StartsWith("https://"))
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(uri))
                    {
                        response.EnsureSuccessStatusCode();
                        var result = await response.Content.ReadAsStringAsync();
                        calCollection = Calendar.LoadFromStream(new StringReader(result)) as CalendarCollection;
                    }
                }
            }
            else if (uri.AbsoluteUri.StartsWith(@"file://"))
            {
                using (var client = new StreamReader(uri.AbsolutePath))
                {
                    calCollection = Calendar.LoadFromStream(new StringReader(await client.ReadToEndAsync())) as CalendarCollection;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return calCollection;
        }

        /// <summary>
        /// Tests if open using DateTime.Now on the server.
        /// </summary>
        /// <returns>True if open</returns>
        public async Task<bool> IsOpenAsync()
        {
            return await OpenTestAsync(DateTime.Now);
        }
        /// <summary>
        /// Check if service is open on the specified datetime.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<bool> IsOpenAsync(DateTimeOffset dateTime)
        {
            return await OpenTestAsync(dateTime);
        }
        private async Task<bool> OpenTestAsync(DateTimeOffset dateTime)
        {
            var result = false;
            await GetICalFeedAsync();
            var freeBusy = _firstCalendar.GetFreeBusy(new CalDateTime(dateTime.Date), new CalDateTime(dateTime.Date.AddHours(24)));
            if (freeBusy.GetFreeBusyStatus(new CalDateTime(dateTime.DateTime)) == FreeBusyStatus.Busy)
            {
                result = true;
            }

            return result;
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ICalFeed() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
