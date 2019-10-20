﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FuturistTranscriber.TranscribeAgent
{
    /// <summary>
    /// Provides transcription of a set of AudioSegments to create a formatted text file. 
    /// <para>See <see cref="TranscribeAgent.AudioSegment"></see> documentation for more info on AudioSegment.</para>
    /// <para>Uses the Microsoft Azure Cognitive Services Speech SDK to perform transcription of audio streams
    /// within each AudioSegment. </para>
    /// </summary>
    class SpeechTranscriber
    {
        public SpeechTranscriber(SortedList<AudioSegment, AudioSegment> audioSegments)
        {
            AudioSegments = audioSegments;
        }

        /// <summary>
        /// SortedList where the the AudioSegments are sorted by their Offset property. 
        /// This supports transcription in the correct order.
        /// </summary>
        public SortedList<AudioSegment, AudioSegment> AudioSegments { get; set; }


        /// <summary>
        /// Creates an audio transcript text file. The transcript contains speaker names,
        /// timestamps, and the contents of what each speaker said.
        /// 
        /// <para> The transcription follows the the correct order, so that
        /// the beginning of the meeting is at the start of the file, and the last
        /// speech around the end of the meeting is at the end of the file.</para>
        /// </summary>
        /// <returns>FileInfo object for the transcription output text file.</returns>
        public FileInfo CreateTranscription()
        {
            return new FileInfo("");
        }

    }
}
