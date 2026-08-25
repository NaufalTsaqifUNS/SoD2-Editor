using System;
using System.Collections.Generic;
using System.Numerics;

namespace SoD2_Editor.Core.Domain.Enclave
{
    public sealed class EnclaveSnapshot
    {
        public IntPtr Address { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string SchemaPath { get; set; }
        public int Influence { get; set; }
        public int MemberDeaths { get; set; }
        public byte DisplayOnMap { get; set; }
        public byte EnclaveType { get; set; }
        public Vector3 BaseCenter { get; set; }
        public IList<IntPtr> CharacterAddresses { get; set; } = new List<IntPtr>();
    }
}
