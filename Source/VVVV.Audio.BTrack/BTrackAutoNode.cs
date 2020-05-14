using System;
using VVVV.Audio;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
    [PluginInfo(Name = "BTrack", Category = "VAudio", Version = "Sink", Help = "RealTime Audio Beat Tracker", AutoEvaluate = true, Tags = "BPM", Author = "motzi", Credits = "Adam Stark")]
    public class BTrackAutoNode : AutoAudioSinkSignalNode<BTrackSignal>
    {
        public override void Dispose()
        {
            foreach (BTrackSignal audioSignal in GetSignalSpread())
            {
                audioSignal.invalidate();
                AudioService.RemoveSink(audioSignal);
                this.DisposeInstance(audioSignal);
            }

            base.Dispose();
        }

        protected override void SetOutputs(int i, BTrackSignal instance)
        {
            base.SetOutputs(i, instance);

            instance.NotifyBeatRead();
        }
    }
}

