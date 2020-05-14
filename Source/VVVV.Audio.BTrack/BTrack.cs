using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using BTrackDotNet;

namespace VVVV.Audio
{
    class BTrack : IDisposable
    {
        private BTrackCLI btrack;

        protected double[] tmpdata = new double[1024];

        private bool beat;
        private double bpm;


        public BTrack(int hopSize, int frameSize)
        {
            this.btrack = new BTrackCLI(hopSize, frameSize);
        }

        public void ProcessFrame(float[] data)
        {
            ProcessFrame(data, data.Length);
        }

        unsafe public void ProcessFrame(float[] data, int sampleCount)
        {
            if (btrack == null)
                return;

            for (int i = 0; i < sampleCount; i++)
            {
                tmpdata[i] = data[i];
            }

            GCHandle handle = GCHandle.Alloc(tmpdata, GCHandleType.Pinned);
            IntPtr framePtr = handle.AddrOfPinnedObject();

            btrack.processAudioFrame(framePtr);
            beat = btrack.beatDueInCurrentFrame();
            bpm = btrack.getCurrentTempoEstimate();

            framePtr = (IntPtr)null;
            handle.Free();

            //fixed (float* fptr = &tmp[0])
            //{
            //    //call ProcessAudioFrame -uses IntPtr to access the sample array
            //    btrack.processAudioFrame(new IntPtr(fptr));
            //}
        }

        public bool Beat
        {
            get { return this.beat;  }
        }

        public double Bpm
        {
            get { return this.bpm; }
        }

        public void Dispose()
        {
            if (btrack != null)
            {
                btrack.Dispose();
                btrack = null;
            }
        }
    }
}
