﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifter.Mss
{
    public class SendMssEventsToHostTrigger
    {
        protected IWetMssEventReceiver wetMssEventReceiver;

        public void Init(IHostInfoEchoer hostInfoEchoer, IWetMssEventReceiver wetMssEventReceiver)
        { 
            hostInfoEchoer.ProcessingCycleEndTimestampRecieved += new ProcessingCycleEndTimestampRecievedEventHandler(hostInfoEchoer_ProcessingCycleEndTimestampRecieved);
            this.wetMssEventReceiver = wetMssEventReceiver;
        }

        protected void hostInfoEchoer_ProcessingCycleEndTimestampRecieved(long cycleEndTimestampInTicks)
        {
            this.wetMssEventReceiver.OnProcessingCycleEnd(cycleEndTimestampInTicks);
        }
    }
}
