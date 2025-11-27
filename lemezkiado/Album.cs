using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lemezkiado
{
    public class Album
    {
        public int id { get; set; }
        public string albumName { get; set; } = "";
        public string artistName { get; set; } = "";
        public int releaseDate { get; set; }
        public float price { get; set; }
        public long streams { get; set; }
        public long copiesSold { get; set; }
        public string genre { get; set; } = "";
        public bool explicitAlbum { get; set; }
        public string[] trackList { get; set; } = [];

        public string Display => $"{(explicitAlbum ? "[E]" : "")} {albumName} ({releaseDate}) - {artistName}";

        public static List<Album> LoadFromJson(string filename)
        {
            using StreamReader file = new StreamReader(filename);
            string json = file.ReadToEnd();
            List<Album> albums = JsonSerializer.Deserialize<List<Album>>(json)!;
            return albums ?? new List<Album>();

        }
    }
}
