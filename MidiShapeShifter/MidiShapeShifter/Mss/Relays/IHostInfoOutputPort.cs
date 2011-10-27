﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    [Flags]
    public enum HostInfoFields
    {
        None = 0x0,
        SampleRate = 0x1,
        Tempo = 0x2,
        TimeSignature = 0x4,
        TransportPlaying = 0x8,
        CalculatedBarZero = 0x10
    }

    public delegate void ProcessingCycleEndEventHandler(long cycleEndTimestampInTicks);
    public delegate void SampleRateChangedEventHandler(double sampleRate);
    public delegate void TempoChangedEventHandler(double tempo);
    public delegate void TimeSignatureChangedEventHandler(int numerator, int denominator);
    public delegate void TransportPlayingChangedEventHandler(bool cransportPlaying);
    public delegate void CalculatedBarZeroChangedEventHandler(long calculatedBarZeroTimestamp);
    public delegate void HostUpdateFinishedEventHandler(HostInfoFields changedHostFields);

    public interface IHostInfoOutputPort
    {
        event ProcessingCycleEndEventHandler BeforeProcessingCycleEnd;
        event ProcessingCycleEndEventHandler ProcessingCycleEnd;

        event HostUpdateFinishedEventHandler HostUpdateFinished;
        HostInfoFields UpdatedFieldsInLastUpdate { get; }
        HostInfoFields ChangedFieldsInLastUpdate { get; }

        double SampleRate { get; }
        bool SampleRateIsInitialized { get; }
        event SampleRateChangedEventHandler SampleRateChanged;

        double Tempo { get; }
        bool TempoIsInitialized { get; }
        event TempoChangedEventHandler TempoChanged;

        int TimeSignatureNumerator { get; }
        int TimeSignatureDenominator { get; }
        bool TimeSignatureIsInitialized { get; }
        event TimeSignatureChangedEventHandler TimeSignatureChanged;

        bool TransportPlaying { get; }
        bool TransportPlayingIsInitialized { get; }
        event TransportPlayingChangedEventHandler TransportPlayingChanged;

        bool CalculatedBarZeroIsInitialized { get; }
        double GetBarPosAtTimestamp(long timestamp);
        long CalculatedBarZeroTimestamp {get;}
        event CalculatedBarZeroChangedEventHandler CalculatedBarZeroChanged;
    }
}
