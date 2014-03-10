using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace wndr.me.SoundWaveser
{
    /// <summary>
    /// Summary description for WaveFile.
    /// </summary>
    public class WaveFile
    {
        public WaveFile(Stream stream)
        {
            FileStream = stream;

            Riff = new RiffChunk();
            Format = new FmtChunk();
            Data = new DataChunk();
        }

        public void Read()
        {
            Riff.Read(FileStream);
            Format.Read(FileStream);
            Data.ReadData(FileStream);
        }

        /// <summary>
        /// Represent the 'RIFF' data chunk of the WAV file
        /// </summary>
        public RiffChunk Riff { get; set; }

        /// <summary>
        /// Represent the 'Format' data chunk of the WAV file
        /// </summary>
        public FmtChunk Format { get; set; }

        /// <summary>
        /// Represents the actual audio data chenk of the WAV file
        /// </summary>
        public DataChunk Data { get; set; }
       
        /// <summary>
        /// Represents the raw Wave's stream
        /// </summary>
        private Stream FileStream { get; set; }

        /// <summary>
        /// Represent the 'RIFF' data chunk of the WAV file
        /// </summary>
        /// <remarks>
        /// The Riff header is 12 bytes long:
        /// RiffID = 4
        /// Size = 4
        /// Format = 4
        /// </remarks>
        public class RiffChunk
        {
            public void Read(Stream strm)
            {
                ReadID(strm);

                BinaryReader binRead = new BinaryReader(strm);
                RiffSize = binRead.ReadUInt32();

                ReadFormat(strm);
            }

            /// <summary>
            /// Extract RiffID
            /// </summary>
            /// <param name="strm">WAV file stream</param>
            private void ReadID(Stream strm)
            {
                byte[] buffer_RIFFID = new byte[4];
                strm.Read(buffer_RIFFID, 0, 4);
                string riffID = System.Text.Encoding.ASCII.GetString(buffer_RIFFID);
                if (riffID != "RIFF")
                    throw new FormatException("The WAV file is malformed.");

                RiffID = riffID;
            }

            /// <summary>
            /// Extract Riff Format
            /// </summary>
            /// <param name="strm"></param>
            private void ReadFormat(Stream strm)
            {
                byte[] buffer_RIFFFormat = new byte[4];
                strm.Read(buffer_RIFFFormat, 0, 4);
                string riffFormat = System.Text.Encoding.ASCII.GetString(buffer_RIFFFormat);
                if (riffFormat != "WAVE")
                    throw new FormatException("The WAV file is malformed.");

                RiffFormat = riffFormat;
            }

            /// <summary>
            /// Contains the letters "RIFF" in ASCII form
            /// </summary>
            public string RiffID { get; set; }

            /// <summary>
            ///  36 + SubChunk2Size
            /// </summary>
            /// <remarks>
            /// 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size)
            /// This is the size of the rest of the chunk 
            /// following this number.  This is the size of the
            /// entire file in bytes minus 8 bytes for the
            /// two fields not included in this count:
            /// ChunkID and ChunkSize.
            /// </remarks>
            public uint RiffSize { get; set; }

            /// <summary>
            ///  Contains the letters "WAVE".
            /// </summary>
            public string RiffFormat { get; set; }
        }

        /// <summary>
        /// Represent the 'Format' data chunk of the WAV file
        /// </summary>
        /// <remarks>
        /// The Format header is 24 bytes long:
        /// Subchunk1ID (Format ID) = 4, 
        /// Subchunk1Size (Format Size) = 4, 
        /// AudioFormat = 2, 
        /// NumChannels = 2, 
        /// SampleRate = 4, 
        /// ByteRate = 4, 
        /// BlockAlign = 2, 
        /// BitPerSample = 2, 
        /// </remarks>
        public class FmtChunk
        {
            /// <summary>
            /// Contains the letters "fmt"
            /// </summary>
            public string FmtID { get; set; }

            /// <summary>
            ///  16 for PCM.  This is the size of the rest of the Subchunk which follows this number.
            /// </summary>
            public uint FmtSize { get; set; }
            /// <summary>
            ///  PCM = 1 (i.e. Linear quantization)
            ///  Values other than 1 indicate some 
            ///  form of compression.
            /// </summary>
            public ushort AudioFormat { get; set; }

            /// <summary>
            /// Mono = 1, Stereo = 2, etc.
            /// </summary>
            public ushort Channels { get; set; }

            /// <summary>
            /// 8000, 44100, etc
            /// </summary>
            public uint SampleRate { get; set; }

            /// <summary>
            /// SampleRate * NumChannels * BitsPerSample/8
            /// </summary>
            public uint ByteRate { get; set; }

            /// <summary>
            /// NumChannels * BitsPerSample/8 The number of bytes for one sample including all channels.
            /// </summary>
            public ushort BlockAlign { get; set; }

            /// <summary>
            /// 8 bits = 8, 16 bits = 16, etc.
            /// </summary>
            public ushort BitsPerSample { get; set; }


            public void Read(Stream strm)
            {
                ReadID(strm);

                BinaryReader binRead = new BinaryReader(strm);

                FmtSize = binRead.ReadUInt32();
                AudioFormat = binRead.ReadUInt16();
                Channels = binRead.ReadUInt16();
                SampleRate = binRead.ReadUInt32();
                ByteRate = binRead.ReadUInt32();
                BlockAlign = binRead.ReadUInt16();
                BitsPerSample = binRead.ReadUInt16();

                if (BitsPerSample != 16)
                    throw new NotSupportedException("The WAV file is not supported.");
            }

            /// <summary>
            /// Extract Format ID
            /// </summary>
            /// <param name="strm"></param>
            private void ReadID(Stream strm)
            {
                byte[] buffer_FormatID = new byte[4];
                strm.Read(buffer_FormatID, 0, 4);
                string formatID = System.Text.Encoding.ASCII.GetString(buffer_FormatID);
                if (formatID != "fmt ")
                    throw new FormatException("The WAV file is malformed.");

                FmtID = formatID;
            }
        }

        /// <summary>
        ///  Represents the actual audio data chenk of the WAV file
        /// </summary>
        /// <remarks>
        /// The Data block is 8 header bytes + actually data:
        /// Subchunk2ID (Data ID) = 4
        /// Subchunk2Size (Data Size) = 4
        /// </remarks>
        public class DataChunk
        {
            /// <summary>
            /// Contains the letters "data"
            /// </summary>
            public string DataID { get; set; }

            public uint DataSize { get; set; }

            public Int16[] Data { get; set; }


            public int NumSamples { get; set; }

            public void ReadData(Stream strm)
            {
                ReadID(strm);

                BinaryReader binRead = new BinaryReader(strm);
                DataSize = binRead.ReadUInt32();

                Read(binRead);
            }

            /// <summary>
            /// Retreive sound data
            /// </summary>
            /// <param name="binRead"></param>
            private void Read(BinaryReader binRead)
            {
                Data = new Int16[DataSize];

                /* NOTE: DataSize is uint32, each chunk of data is 16bit, meaning that each jump will be of 16bit each.
                   By deviding DataSize by 2 we get number oj jumps needed in order to cover entire file. */
                NumSamples = (int)(DataSize / 2);

                for (int i = 0; i < NumSamples; i++)
                    Data[i] = binRead.ReadInt16();
            }

            /// <summary>
            /// Retreive Data ID
            /// </summary>
            /// <param name="strm"></param>
            private void ReadID(Stream strm)
            {
                byte[] buffer_FormatID = new byte[4];
                strm.Read(buffer_FormatID, 0, 4);
                string dataID = System.Text.Encoding.ASCII.GetString(buffer_FormatID);
                if (dataID != "data")
                    throw new FormatException("The WAV file is malformed.");

                DataID = dataID;
            }

            internal Int16 this[int pos]
            {
                get
                {
                    return Data[pos];
                }
            }
        }
    }
}
