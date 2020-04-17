using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace ChallengePad.Models
{
    public class Objective
    {
#pragma warning disable CS8618
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public long Points { get; set; }
        public bool Solved { get; set; }
        public long OperationId { get; set; }
        [DefaultValue(true)]
        public bool Visible { get; set; }
        public List<UploadedFile> Files { get; set; } = new List<UploadedFile>();
#pragma warning restore CS8618 // Das Feld lässt keine NULL-Werte zu und ist nicht initialisiert. Deklarieren Sie das Feld ggf. als "Nullable".
    }
}
