using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ChallengePad.Models
{
    public class UploadedFile
    {
#pragma warning disable CS8618
        public long Id { get; set; }
        public string Filename { get; set; }
        public string Username { get; set; }
        public DateTime Timestamp { get; set; }
#pragma warning restore CS8618
    }
}
