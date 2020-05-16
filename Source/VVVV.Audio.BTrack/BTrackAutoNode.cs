using System;
using VVVV.Audio;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
    [PluginInfo(Name = "BTrack", Category = "VAudio", Version = "Sink", Help = "RealTime Audio Beat Tracker", AutoEvaluate = true, Tags = "BPM, Tempo", Author = "motzi", Credits = "Adam Stark")]
    public class BTrackAutoNode : AutoAudioSinkSignalNode<BTrackSignal>
    {
        
        [Input("Fixed Tempo", DefaultBoolean = false, IsToggle = true)]
        public IDiffSpread<bool> FFixedTempo;

        [Input("Tempo BPM", DefaultValue = 120, MinValue = 80, MaxValue = 160)]
        public IDiffSpread<double> FTempo;

        [Input("Count In", IsBang = true)]
        public IDiffSpread<bool> FODFSample;

        [Input("Alpha", DefaultValue = 0.9, MaxValue = 1, MinValue = 0.1, Visibility = PinVisibility.Hidden)]
        public IDiffSpread<double> FAlpha;



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

        protected override void SetParameters(int i, BTrackSignal instance)
        {
            if (FFixedTempo.IsChanged)
                instance.NotifyFixedTempo(FFixedTempo[i], FTempo[i]);
            else if (FTempo.IsChanged)
                instance.NotifyTempoChanged(FTempo[i]);

            
            if (FAlpha.IsChanged)
                instance.NotifyAlpha(FAlpha[i]);

            if (FODFSample.IsChanged && FODFSample[i] == true)
                instance.NotifyODFSample(1.0);


            base.SetParameters(i, instance);
        }
    }
}

