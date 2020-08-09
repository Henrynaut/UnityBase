using UnityEngine;
using System.Collections.Generic;

namespace Photon.Voice.Unity
{
    // Plays back input audio via Unity AudioSource
    // May consume audio packets in thread other than Unity's main thread
    public class UnityAudioOut : IAudioOut<float>
    {
        private int frameSamples;
        private int frameSize;
        private int bufferSamples;

        private int clipWriteSamplePos;
        private int clipClearSamplePos;

        private int playSamplePosPrev;
        private int sourceTimeSamplesPrev;
        private int playLoopCount;

        private readonly AudioSource source;
        private int channels;
        private bool started;
        private bool flushed;

        private int maxDevPlayDelaySamples;
        private int targetPlayDelaySamples;

        private readonly ILogger logger;
        private readonly string logPrefix;
        private readonly bool debugInfo;

        float[] zeroFrame;
        public UnityAudioOut(AudioSource audioSource, ILogger logger, string logPrefix, bool debugInfo)
        {            
            this.source = audioSource;
            this.logger = logger;
            this.logPrefix = logPrefix;
            this.debugInfo = debugInfo;
        }
        public int Lag { get { return this.clipWriteSamplePos - (this.started ? this.playLoopCount * this.bufferSamples + this.source.timeSamples : 0); } }

        public bool IsPlaying
        {
            get { return started && !this.flushed; }
        }

        public void Start(int frequency, int channels, int frameSamples, int playDelayMs)
        {
            // frequency = (int)(frequency * 1.2); // underrun test
            // frequency = (int)(frequency / 1.2); // overrun test

            this.channels = channels;
            this.bufferSamples = 4*(playDelayMs * frequency / 1000 + frameSamples + frequency); // max delay + frame +  1 sec. just in case
            this.frameSamples = frameSamples;
            this.frameSize = frameSamples * channels;

            this.source.loop = true;
            // using streaming clip leads to too long delays
            this.source.clip = AudioClip.Create("UnityAudioOut", bufferSamples, channels, frequency, false);
            this.started = true;

            // add 1 frame samples to make sure that we have something to play when delay set to 0
            int playDelaySamples = playDelayMs * frequency / 1000 + frameSamples;
            this.maxDevPlayDelaySamples = playDelaySamples / 2;
            this.targetPlayDelaySamples = playDelaySamples + maxDevPlayDelaySamples;

            this.clipWriteSamplePos = this.targetPlayDelaySamples;

            if (this.framePool.Info != this.frameSize)
            {
                this.framePool.Init(this.frameSize);
            }

            this.zeroFrame = new float[this.frameSize];

            this.source.Play();
        }

        Queue<float[]> frameQueue = new Queue<float[]>();
        public const int FRAME_POOL_CAPACITY = 50;
        PrimitiveArrayPool<float> framePool = new PrimitiveArrayPool<float>(FRAME_POOL_CAPACITY, "UnityAudioOut");

        // should be called in Update thread
        public void Service()
        {
            if (this.started)
            {
                // cache source.timeSamples
                int sourceTimeSamples = source.timeSamples;                
                // loop detection (pcmsetpositioncallback not called when clip loops)
                if (sourceTimeSamples < sourceTimeSamplesPrev)
                {
                    playLoopCount++;
                }
                sourceTimeSamplesPrev = sourceTimeSamples;


                var playSamplePos = this.playLoopCount * this.bufferSamples + sourceTimeSamples;

                if (!this.flushed)
                {
                    var lagSamples = this.clipWriteSamplePos - playSamplePos;
                    if (lagSamples > targetPlayDelaySamples + maxDevPlayDelaySamples)
                    {
                        if (this.debugInfo)
                        {
                            this.logger.LogWarning("{0} UnityAudioOut overrun {1} {2} {3} {4} {5} {6}", this.logPrefix, targetPlayDelaySamples - maxDevPlayDelaySamples, targetPlayDelaySamples + maxDevPlayDelaySamples, lagSamples, playSamplePos, this.clipWriteSamplePos, playSamplePos + targetPlayDelaySamples);
                        }
                        this.clipWriteSamplePos = playSamplePos + targetPlayDelaySamples;
                    }
                    else if (lagSamples < targetPlayDelaySamples - maxDevPlayDelaySamples)
                    {
                        if (this.debugInfo)
                        {
                            this.logger.LogWarning("{0} UnityAudioOut underrun {1} {2} {3} {4} {5} {6}", this.logPrefix, targetPlayDelaySamples - maxDevPlayDelaySamples, targetPlayDelaySamples + maxDevPlayDelaySamples, lagSamples, playSamplePos, this.clipWriteSamplePos, playSamplePos + targetPlayDelaySamples);
                        }
                        this.clipWriteSamplePos = playSamplePos + targetPlayDelaySamples;
                    }
                    //else
                    //    this.logger.LogWarning("{0} === {1} {2} {3} {4} {5}", this.logPrefix, targetPlayDelaySamples - maxDevPlayDelaySamples, targetPlayDelaySamples + maxDevPlayDelaySamples, lagSamples, this.clipWriteSamplePos, playSamplePos);
                }

                lock (this.frameQueue)
                {
                    while (frameQueue.Count > 0)
                    {
                        var frame = frameQueue.Dequeue();
                        if (frame == null) // flush signalled
                        {
                            this.flushed = true;
                            return;
                        }
                        else
                        {
                            if (this.flushed)
                            {
                                this.clipWriteSamplePos = playSamplePos + targetPlayDelaySamples;
                                this.flushed = false;
                            }                            
                        }
                        this.source.clip.SetData(frame, this.clipWriteSamplePos % this.bufferSamples);
                        this.clipWriteSamplePos += frame.Length / this.channels;
                        framePool.Release(frame);
                    }
                }

                // clear played back buffer segment
                var clearStart = this.playSamplePosPrev;
                var clearMin = playSamplePos - this.bufferSamples;
                if (clearStart < clearMin)
                {
                    clearStart = clearMin;
                }
                // round up
                var framesToClear = (playSamplePos - clearStart - 1) / this.frameSamples + 1;                
//                Debug.LogFormat("{0} ----- {1} {2} {3} {4}", this.logPrefix, targetPlayDelaySamples - maxDevPlayDelaySamples, targetPlayDelaySamples + maxDevPlayDelaySamples, this.clipWriteSamplePos, playSamplePos);
                for (var offset = playSamplePos - framesToClear * this.frameSamples; offset < playSamplePos; offset += this.frameSamples)
                {
                    var o = offset % this.bufferSamples;
                    if (o < 0) o += this.bufferSamples;
//                    Debug.Log("=== " + offset  + " " + (offset % this.bufferSamples) + " " + this.playSamplePosPrev + " " + playSamplePos);
                    this.source.clip.SetData(this.zeroFrame, o);
                }
                this.playSamplePosPrev = playSamplePos;

            }
        }
        // may be called on any thread
        public void Push(float[] frame)
        {
            if (!this.started)
            {
                return;
            }

            if (frame.Length == 0)
            {
                return;
            }

            if (frame.Length != this.frameSize)
            {
                logger.LogError("{0} UnityAudioOut audio frames are not of  size: {1} != {2}", this.logPrefix, frame.Length, this.frameSize);
                return;
            }

            float[] b = framePool.AcquireOrCreate();
            System.Buffer.BlockCopy(frame, 0, b, 0, frame.Length * sizeof(float));
            lock (this.frameQueue)
            {
                this.frameQueue.Enqueue(b);
            }
        }

        public void Flush()
        {
            lock (this.frameQueue)
            {
                this.frameQueue.Enqueue(null);
            }
        }

        public void Stop()
        {
            this.started = false;
            if (this.source != null)
            {
                this.source.clip = null;
            }
        }
    }
}