using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public DateTime releaseDate { get; set; }
        public float price { get; set; }
        public int streams { get; set; }
        public int copiesSold { get; set; }
        public string genre { get; set; } = "";
        public bool explicitAlbum { get; set; }
        public string[] trackList { get; set; } = [];

        public static List<Album> LoadFromJson(string filename)
        {
            var jsonContent = System.IO.File.ReadAllText(filename, Encoding.UTF8);
            var albums = JsonSerializer.Deserialize<List<Album>>(jsonContent, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return albums ?? new List<Album>();

        }
    }
}
