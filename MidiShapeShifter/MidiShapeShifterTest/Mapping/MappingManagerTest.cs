﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using NUnit.Framework;

using MidiShapeShifter;
using MidiShapeShifter.Mapping;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;



namespace MidiShapeShifterTest.Mapping
{
    [TestFixture]
    public class MappingManagerTest
    {

        protected const bool DEFAULT_OVERRIDE_DUPLICATES = false;
        protected const MappingEntry.EquationInputMode DEFAULT_EQ_INPUT_MODE = MappingEntry.EquationInputMode.Text;
        protected const string DEFAULT_EQUATION = "x";
        protected const MssMsgUtil.MssMsgType DEFAULT_MSG_TYPE = MssMsgUtil.MssMsgType.NoteOn;
        //useful for when you need to test two message types that do not match. Other types do not need to be tested here
        //because MappingManager should not have any logic specific to a particular type.
        protected const MssMsgUtil.MssMsgType SECONDARY_MSG_TYPE = MssMsgUtil.MssMsgType.CC;

        [Test]
        public void AddMappingEntry_SingleEntry_SuccessfullyAdded()
        {
            //MssMsg mssMsg = new MssMsg(MssMsgUtil.MssMsgType.NoteOn, 1/*chan 1*/, 12/*C0*/, 100/*vel 100*/);

            MappingManager mappingMgr = Factory_MappingManager_Default();
            MappingEntry mappingEntry = Factory_MappingEntry_Basic();

            mappingMgr.AddMappingEntry(mappingEntry);

            Assert.AreEqual(mappingEntry, mappingMgr.GetMappingEntry(0));
        }

        [Test]
        public void AddMappingEntry_SingleEntry_AddsExactlyOneEntry()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            MappingEntry mappingEntry = Factory_MappingEntry_Basic();

            mappingMgr.AddMappingEntry(mappingEntry);

            Assert.AreEqual(1, mappingMgr.GetNumEntries());
        }

        [Test]
        public void RemoveMappingEntry_ManagerHasOneEntry_ExistingEntryIsRemoved()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            MappingEntry mappingEntry = Factory_MappingEntry_Basic();

            mappingMgr.AddMappingEntry(mappingEntry);
            mappingMgr.RemoveMappingEntry(0);

            Assert.AreEqual(0, mappingMgr.GetNumEntries());
        }

        [Test]
        public void MoveEntryUp_MoveBottomEntryUpInManagerThatHasTwoEntries_EntriesAreSwapped()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            MappingEntry mappingEntry1 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
            MappingEntry mappingEntry2 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 2, 2, 3, 3);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);

            mappingMgr.MoveEntryUp(1);

            Assert.AreEqual(mappingEntry2, mappingMgr.GetMappingEntry(0));
            Assert.AreEqual(mappingEntry1, mappingMgr.GetMappingEntry(1));
        }

        [Test]
        public void MoveEntryUp_MoveThirdEntryUpInManagerThatHasFourEntries_OnlyMiddleTwoEntriesAreSwapped()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            MappingEntry mappingEntry1 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
            MappingEntry mappingEntry2 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 2, 2, 0, 0);
            MappingEntry mappingEntry3 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 3, 3, 0, 0);
            MappingEntry mappingEntry4 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 4, 4, 0, 0);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);
            mappingMgr.AddMappingEntry(mappingEntry3);
            mappingMgr.AddMappingEntry(mappingEntry4);

            mappingMgr.MoveEntryUp(2);

            Assert.AreEqual(mappingEntry1, mappingMgr.GetMappingEntry(0));
            Assert.AreEqual(mappingEntry3, mappingMgr.GetMappingEntry(1));
            Assert.AreEqual(mappingEntry2, mappingMgr.GetMappingEntry(2));
            Assert.AreEqual(mappingEntry4, mappingMgr.GetMappingEntry(3));
        }

        [Test]
        public void MoveEntryDown_MoveTopEntryDownInManagerThatHasTwoEntries_EntriesAreSwapped()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            MappingEntry mappingEntry1 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
            MappingEntry mappingEntry2 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 2, 2, 3, 3);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);

            mappingMgr.MoveEntryDown(0);

            Assert.AreEqual(mappingEntry2, mappingMgr.GetMappingEntry(0));
            Assert.AreEqual(mappingEntry1, mappingMgr.GetMappingEntry(1));
        }

        [Test]
        public void MoveEntryDown_MoveSecondEntryDownInManagerThatHasFourEntries_OnlyMiddleTwoEntriesAreSwapped()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            MappingEntry mappingEntry1 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
            MappingEntry mappingEntry2 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 2, 2, 0, 0);
            MappingEntry mappingEntry3 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 3, 3, 0, 0);
            MappingEntry mappingEntry4 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 4, 4, 0, 0);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);
            mappingMgr.AddMappingEntry(mappingEntry3);
            mappingMgr.AddMappingEntry(mappingEntry4);

            mappingMgr.MoveEntryDown(1);

            Assert.AreEqual(mappingEntry1, mappingMgr.GetMappingEntry(0));
            Assert.AreEqual(mappingEntry3, mappingMgr.GetMappingEntry(1));
            Assert.AreEqual(mappingEntry2, mappingMgr.GetMappingEntry(2));
            Assert.AreEqual(mappingEntry4, mappingMgr.GetMappingEntry(3));
        }

        [Test]
        public void GetAssociatedEntries_MsgMatchesOneEntry_TheAssociatedEntryIsReturned()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            MappingEntry mappingEntry = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 1, 3, 0, 10);
            mappingMgr.AddMappingEntry(mappingEntry);

            MssMsg msg = Factory_MssMsg_InitializedValues(
                DEFAULT_MSG_TYPE, /*same as type in manager*/
                2, /*between 1 and 3*/
                5, /*between 0 and 10*/
                100 /*doesn't need to match anything*/);

            var matchingEntries = mappingMgr.GetAssociatedEntries(msg);
            Assert.IsTrue(matchingEntries.Contains<MappingEntry>(mappingEntry));
        }

        [Test]
        public void GetAssociatedEntries_MsgOnlyMatchesPartsOfEntriesInMgr_TheEnumerationReturnedIsEmpty()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            
            MappingEntry mappingEntry1 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 1, 1, 3, 127);
            MappingEntry mappingEntry2 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 2, 16, 2, 2);
            MappingEntry mappingEntry3 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(SECONDARY_MSG_TYPE, 1, 1, 2, 2);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);
            mappingMgr.AddMappingEntry(mappingEntry3);

            MssMsg msg = Factory_MssMsg_InitializedValues(
                DEFAULT_MSG_TYPE, /*matches entries 1 and 2*/
                1, /*matches entries 1 and 3*/
                2, /*matches entries 2 and 3*/
                100 /*doesn't need to match anything*/);

            var matchingEntries = mappingMgr.GetAssociatedEntries(msg);

            Assert.IsEmpty(matchingEntries.ToList());
        }

        [Test]
        public void GetAssociatedEntries_MultipleMatches_AllMatchingEntriesAreReturned()
        {
            TestCase_GetAssociatedEntries_ManagerWithThreeOverlappingEntriesThatMatchAMsg(
                false, false, false,
                true, true, true);
        }

        [Test]
        public void GetAssociatedEntries_MultipleMatchesButOverrideDuplicatesIsTrueForOne_TheOverrideDuplicatesEntryIsTheOnlyOneReturned()
        {
            TestCase_GetAssociatedEntries_ManagerWithThreeOverlappingEntriesThatMatchAMsg(
                false, true, false,
                false, true, false);
        }

        [Test]
        public void GetAssociatedEntries_MultipleMatchesButOverrideDuplicatesIsTrueForSome_TheFirstOverrideDuplicatesEntryIsTheOnlyOneReturned()
        {
            TestCase_GetAssociatedEntries_ManagerWithThreeOverlappingEntriesThatMatchAMsg(
                true, false, true,
                true, false, false);
        }

        //*******************************Helpers*******************************

        protected MappingManager Factory_MappingManager_Default()
        {
            return new MappingManager();
        }

        protected MappingEntry Factory_MappingEntry_Basic()
        {
            return Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
        }



        protected MappingEntry Factory_MappingEntry_MapsIdenticalMidiMsgInfos(
            MssMsgUtil.MssMsgType msgType, 
            int chanRangeBottom, int chanRamgeTop, 
            int paramRangeBottom, int paramRangeTop)
        {
            MidiMsgInfo inMsgInfo = (MidiMsgInfo)Factory_MssMsgInfo.Create(msgType);
            MidiMsgInfo outMsgInfo = (MidiMsgInfo)Factory_MssMsgInfo.Create(msgType);

            if (inMsgInfo == null || outMsgInfo == null)
            {
                //A midi type was not used
                Debug.Assert(false);
                return null;
            }

            inMsgInfo.Initialize(chanRangeBottom, chanRamgeTop, paramRangeBottom, paramRangeTop);
            outMsgInfo.Initialize(chanRangeBottom, chanRamgeTop, paramRangeBottom, paramRangeTop);

            MappingEntry mapEntry = new MappingEntry();
            mapEntry.InMssMsgInfo = inMsgInfo;
            mapEntry.OutMssMsgInfo = outMsgInfo;
            mapEntry.OverrideDuplicates = DEFAULT_OVERRIDE_DUPLICATES;
            mapEntry.EqInputMode = DEFAULT_EQ_INPUT_MODE;
            mapEntry.Equation = DEFAULT_EQUATION;

            return mapEntry;
        }

        protected MssMsg Factory_MssMsg_InitializedValues(MssMsgUtil.MssMsgType msgType, int data1, int data2, int data3)
        {
            return new MssMsg(msgType, data1, data2, data3);
        }

        protected void TestCase_GetAssociatedEntries_ManagerWithThreeOverlappingEntriesThatMatchAMsg(
            bool Entry1Override, bool Entry2Override, bool Entry3Override, 
            bool Entry1Matches, bool Entry2Matches, bool Entry3Matches)
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();

            MappingEntry mappingEntry1 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 1, 16, 0, 127);
            mappingEntry1.OverrideDuplicates = Entry1Override;
            MappingEntry mappingEntry2 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 4, 5, 10, 20);
            mappingEntry2.OverrideDuplicates = Entry2Override;
            MappingEntry mappingEntry3 = Factory_MappingEntry_MapsIdenticalMidiMsgInfos(DEFAULT_MSG_TYPE, 5, 5, 15, 15);
            mappingEntry3.OverrideDuplicates = Entry3Override;

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);
            mappingMgr.AddMappingEntry(mappingEntry3);

            MssMsg msg = Factory_MssMsg_InitializedValues(
                DEFAULT_MSG_TYPE, /*matches all entries*/
                5, /*matches all*/
                15, /*matches all*/
                100 /*doesn't need to match anything*/);

            var matchingEntries = mappingMgr.GetAssociatedEntries(msg);

            Assert.AreEqual(matchingEntries.Contains<MappingEntry>(mappingEntry1), Entry1Matches);
            Assert.AreEqual(matchingEntries.Contains<MappingEntry>(mappingEntry2), Entry2Matches);
            Assert.AreEqual(matchingEntries.Contains<MappingEntry>(mappingEntry3), Entry3Matches);
        }
    }
}