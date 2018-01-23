using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.IO;
using Ical.Net;
using Ical.Net.DataTypes;
using OperatingHours;

namespace OperatingHours_Tests
{
    [TestClass]
    public class CalFeedTests
    {

        private static readonly Uri _fileUri = new Uri($@"{System.Environment.CurrentDirectory}\basic.ics");
        private static readonly Uri _googleUri = new Uri("https://calendar.google.com/calendar/ical/mqi9p72ijmah29n96vmpj0pqds%40group.calendar.google.com/private-220fafc720769a8d9f8c9d8d95b816ad/basic.ics");
        private readonly CalFeed _webFeed = new CalFeed(_googleUri);
        private readonly CalFeed _fileFeed = new CalFeed(_fileUri);
        private readonly DateTime _repeatException = Convert.ToDateTime("2018-01-03");
        private readonly DateTime _firstMinute = Convert.ToDateTime("08:30");
        private readonly DateTime _lastMinute = Convert.ToDateTime("17:29");
        private readonly DateTime _closedAfter = Convert.ToDateTime("17:30");
        private readonly DateTime _closedBefore = Convert.ToDateTime("08:29");
        private readonly DateTime _repeatDay = Convert.ToDateTime("2018-01-02");
        private readonly DateTime _manualLateDay = Convert.ToDateTime("2018-01-04");
        private readonly DateTime _repeatShortDay = Convert.ToDateTime("2018-01-05");

        [TestMethod, TestCategory("WebFeed")]
        public async Task WebFeed_RepeatIsOpen()
        {
            Assert.IsTrue(await BaseTest(_repeatDay, _firstMinute, _webFeed));
            Assert.IsTrue(await BaseTest(_repeatDay, _lastMinute, _webFeed));
        }

        [TestMethod, TestCategory("WebFeed")]
        public async Task WebFeed_RepeatIsClosed()
        {
            Assert.IsFalse(await BaseTest(_repeatDay, _closedBefore, _webFeed));
            Assert.IsFalse(await BaseTest(_repeatDay, _closedAfter, _webFeed));
        }

        [TestMethod, TestCategory("WebFeed")]
        public async Task WebFeed_DayRemoved()
        {
            Assert.IsFalse(await BaseTest(_repeatException, _firstMinute, _webFeed));
            Assert.IsFalse(await BaseTest(_repeatException, _lastMinute, _webFeed));
        }

        [TestMethod, TestCategory("WebFeed")]
        public async Task WebFeed_ManualLate()
        {
            Assert.IsFalse(await BaseTest(_manualLateDay, _firstMinute, _webFeed));
            Assert.IsTrue(await BaseTest(_manualLateDay, _lastMinute, _webFeed));
        }

        [TestMethod, TestCategory("WebFeed")]
        public async Task WebFeed_RepeatShort()
        {
            Assert.IsTrue(await BaseTest(_repeatShortDay, _firstMinute, _webFeed));
            Assert.IsFalse(await BaseTest(_repeatShortDay, _lastMinute, _webFeed));
        }



        [TestMethod, TestCategory("FileFeed")]
        public async Task FileFeed_RepeatIsOpen()
        {
            Assert.IsTrue(await BaseTest(_repeatDay, _firstMinute, _fileFeed));
            Assert.IsTrue(await BaseTest(_repeatDay, _lastMinute, _fileFeed));
        }

        [TestMethod, TestCategory("FileFeed")]
        public async Task FileFeed_RepeatIsClosed()
        {
            Assert.IsFalse(await BaseTest(_repeatDay, _closedBefore, _fileFeed));
            Assert.IsFalse(await BaseTest(_repeatDay, _closedAfter, _fileFeed));
        }

        [TestMethod, TestCategory("FileFeed")]
        public async Task FileFeed_DayRemoved()
        {
            Assert.IsFalse(await BaseTest(_repeatException, _firstMinute, _fileFeed));
            Assert.IsFalse(await BaseTest(_repeatException, _lastMinute, _fileFeed));
        }

        [TestMethod, TestCategory("FileFeed")]
        public async Task FileFeed_ManualLate()
        {
            Assert.IsFalse(await BaseTest(_manualLateDay, _firstMinute, _fileFeed));
            Assert.IsTrue(await BaseTest(_manualLateDay, _lastMinute, _fileFeed));
        }

        [TestMethod, TestCategory("FileFeed")]
        public async Task FileFeed_RepeatShort()
        {
            Assert.IsTrue(await BaseTest(_repeatShortDay, _firstMinute, _fileFeed));
            Assert.IsFalse(await BaseTest(_repeatShortDay, _lastMinute, _fileFeed));
        }

        //[TestMethod, TestCategory("FileFeed"), TestCategory("SIC")]
        //public async Task FileFeed_SicIsOpen()
        //{
        //    Assert.IsTrue(await SicBaseTest(_sicFirstMinute, _fileUri));
        //    Assert.IsTrue(await SicBaseTest(_sicLastMinute, _fileUri));
        //}

        //[TestMethod, TestCategory("FileFeed"), TestCategory("SIC")]
        //public async Task FileFeed_SicIsClosed()
        //{
        //    Assert.IsFalse(await SicBaseTest(_sicClosed, _fileUri));
        //}

        //[TestMethod, TestCategory("FileFeed"), TestCategory("SIC")]
        //public async Task FileFeed_SicHolidayIsClosed()
        //{
        //    Assert.IsFalse(await SicBaseTest(_sicHoliday, _fileUri));
        //}

        private static async Task<bool> BaseTest(DateTime day, DateTime time, ICalFeed queue)
        {

            return await queue.IsOpenAsync(day.Date.AddTicks(time.TimeOfDay.Ticks));
        }

    }


    //[TestClass]
    //public class FreeBusyTest
    //{

    //    //[TestMethod, Category("FreeBusy")]
    //    //public void GetFreeBusyStatus1()
    //    //{
    //    //    var cal = new Ical.Net.Calendar();

    //    //   var evt = cal.Create<Ical.Net.Event>();
    //    //    evt.Summary = "Test event";
    //    //    evt.Start = new CalDateTime(2010, 10, 1, 8, 0, 0);
    //    //    evt.End = new CalDateTime(2010, 10, 1, 9, 0, 0);

    //    //    var evt2 = cal.Create<Ical.Net.Event>();
    //    //    evt2.Summary = "Test event";
    //    //    evt2.Start = new CalDateTime(2010, 10, 1, 9, 0, 0);
    //    //    evt2.End = new CalDateTime(2010, 10, 1, 10, 0, 0);

    //    //    var freeBusy = cal.GetFreeBusy(new CalDateTime(2010, 10, 1, 0, 0, 0), new CalDateTime(2010, 10, 7, 11, 59, 59));
    //    //    Assert.AreEqual(FreeBusyStatus.Free, freeBusy.GetFreeBusyStatus(new CalDateTime(2010, 10, 1, 7, 59, 59)));
    //    //    Assert.AreEqual(FreeBusyStatus.Busy, freeBusy.GetFreeBusyStatus(new CalDateTime(2010, 10, 1, 8, 0, 0)));
    //    //    Assert.AreEqual(FreeBusyStatus.Busy, freeBusy.GetFreeBusyStatus(new CalDateTime(2010, 10, 1, 8, 59, 59)));
    //    //    Assert.AreEqual(FreeBusyStatus.Free, freeBusy.GetFreeBusyStatus(new CalDateTime(2010, 10, 1, 9, 0, 0)));
    //    //}
    //}
}
