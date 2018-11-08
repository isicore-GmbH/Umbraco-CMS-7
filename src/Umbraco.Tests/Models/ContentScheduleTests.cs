﻿using System;
using System.Linq;
using NUnit.Framework;
using Umbraco.Core.Models;

namespace Umbraco.Tests.Models
{
    [TestFixture]
    public class ContentScheduleTests
    {
        [Test]
        public void Release_Date_Less_Than_Expire_Date()
        {
            var now = DateTime.Now;
            var schedule = new ContentScheduleCollection();
            Assert.Throws<InvalidOperationException>(() => schedule.Add(now, now));
        }

        [Test]
        public void Cannot_Add_Duplicate_Dates_Invariant()
        {
            var now = DateTime.Now;
            var schedule = new ContentScheduleCollection();
            schedule.Add(now, null);
            Assert.Throws<ArgumentException>(() => schedule.Add(null, now));
        }

        [Test]
        public void Cannot_Add_Duplicate_Dates_Variant()
        {
            var now = DateTime.Now;
            var schedule = new ContentScheduleCollection();
            schedule.Add(now, null);
            schedule.Add("en-US", now, null);
            Assert.Throws<ArgumentException>(() => schedule.Add("en-US", null, now));
            Assert.Throws<ArgumentException>(() => schedule.Add(null, now));
        }

        [Test]
        public void Can_Remove_Invariant()
        {
            var now = DateTime.Now;
            var schedule = new ContentScheduleCollection();
            schedule.Add(now, null);
            var invariantSched = schedule.GetSchedule(string.Empty);
            schedule.Remove(invariantSched.First());
            Assert.AreEqual(0, schedule.FullSchedule.Count());
        }

        [Test]
        public void Can_Remove_Variant()
        {
            var now = DateTime.Now;
            var schedule = new ContentScheduleCollection();
            schedule.Add(now, null);
            schedule.Add("en-US", now, null);
            var invariantSched = schedule.GetSchedule(string.Empty);
            schedule.Remove(invariantSched.First());
            Assert.AreEqual(0, schedule.GetSchedule(string.Empty).Count());
            Assert.AreEqual(1, schedule.FullSchedule.Count());
            var variantSched = schedule.GetSchedule("en-US");
            schedule.Remove(variantSched.First());
            Assert.AreEqual(0, schedule.GetSchedule("en-US").Count());
            Assert.AreEqual(0, schedule.FullSchedule.Count());
        }

        [Test]
        public void Can_Clear_Start_Invariant()
        {
            var now = DateTime.Now;
            var schedule = new ContentScheduleCollection();
            schedule.Add(now, now.AddDays(1));

            schedule.Clear(ContentScheduleChange.Start);

            Assert.AreEqual(0, schedule.GetSchedule(ContentScheduleChange.Start).Count());
            Assert.AreEqual(1, schedule.GetSchedule(ContentScheduleChange.End).Count());
            Assert.AreEqual(1, schedule.FullSchedule.Count());
        }

        [Test]
        public void Can_Clear_End_Variant()
        {
            var now = DateTime.Now;
            var schedule = new ContentScheduleCollection();
            schedule.Add(now, now.AddDays(1));
            schedule.Add("en-US", now, now.AddDays(1));

            schedule.Clear(ContentScheduleChange.End);

            Assert.AreEqual(0, schedule.GetSchedule(ContentScheduleChange.End).Count());
            Assert.AreEqual(1, schedule.GetSchedule(ContentScheduleChange.Start).Count());
            Assert.AreEqual(1, schedule.GetSchedule("en-US", ContentScheduleChange.End).Count());
            Assert.AreEqual(1, schedule.GetSchedule("en-US", ContentScheduleChange.Start).Count());
            Assert.AreEqual(3, schedule.FullSchedule.Count());

            schedule.Clear("en-US", ContentScheduleChange.End);

            Assert.AreEqual(0, schedule.GetSchedule(ContentScheduleChange.End).Count());
            Assert.AreEqual(1, schedule.GetSchedule(ContentScheduleChange.Start).Count());
            Assert.AreEqual(0, schedule.GetSchedule("en-US", ContentScheduleChange.End).Count());
            Assert.AreEqual(1, schedule.GetSchedule("en-US", ContentScheduleChange.Start).Count());
            Assert.AreEqual(2, schedule.FullSchedule.Count());
        }

    }
}
