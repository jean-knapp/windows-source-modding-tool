﻿using System.IO;
using System.Linq;

namespace SourceSDK.Packages.VPKPackage
{
    public class VpkFile : PackageFile
    {
        public byte[] PreloadData { get { return ReadPreloadData(); } }
        public bool HasPreloadData { get; set; }

        internal uint CRC;
        internal ushort PreloadBytes;
        internal uint PreloadDataOffset;
        internal ushort ArchiveIndex;
        internal uint EntryOffset;
        internal uint EntryLength;
        internal VpkArchive ParentArchive;

        internal VpkFile(VpkArchive parentArchive, uint crc, ushort preloadBytes, uint preloadDataOffset, ushort archiveIndex, uint entryOffset,
            uint entryLength, string extension, string path, string filename)
        {
            ParentArchive = parentArchive;
            CRC = crc;
            PreloadBytes = preloadBytes;
            PreloadDataOffset = preloadDataOffset;
            ArchiveIndex = archiveIndex;
            EntryOffset = entryOffset;
            EntryLength = entryLength;
            Extension = extension;
            Path = path;
            Filename = filename;
            HasPreloadData = preloadBytes > 0;
        }

        public override string ToString()
        {
            return string.Concat(Path, "/", Filename, ".", Extension);
        }

        private byte[] ReadPreloadData()
        {
            if (PreloadBytes > 0)
            {
                var buff = new byte[PreloadBytes];
                using (var fs = new FileStream(ParentArchive.ArchivePath, FileMode.Open, FileAccess.Read))
                {
                    buff = new byte[PreloadBytes];
                    fs.Seek(PreloadDataOffset, SeekOrigin.Begin);
                    fs.Read(buff, 0, buff.Length);
                }
                return buff;
            }
            return null;
        }

        protected override byte[] ReadData()
        {
            var partFile = ParentArchive.Parts.FirstOrDefault(part => part.Index == ArchiveIndex);
            if (partFile == null)
                return null;
            if (HasPreloadData)
                return ReadPreloadData();
            var buff = new byte[EntryLength];
            using (var fs = new FileStream(partFile.Filename, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(EntryOffset, SeekOrigin.Begin);
                fs.Read(buff, 0, buff.Length);
            }
            return buff;
        }

        public override void CopyTo(string destinationPath)
        {
            File.WriteAllBytes(destinationPath, Data);
        }

        public byte[] AnyData { get { if (HasPreloadData) return ReadPreloadData(); else return ReadData(); } }

    }
}
