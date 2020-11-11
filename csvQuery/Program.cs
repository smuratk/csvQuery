using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace csvQuery
{
    class PlayedSong : IEquatable<PlayedSong>
    {
        //PLAY_ID	SONG_ID	CLIENT_ID	PLAY_TS
        //public string play_id { get; set; }
        public string song_id { get; set; }
        public string client_id { get; set; }
        //public string play_ts { get; set; }

        public override int GetHashCode()
        {
            return client_id.GetHashCode() + song_id.GetHashCode();
        }

        public bool Equals(PlayedSong other)
        {
            if (this.client_id == other.client_id && this.song_id == other.song_id)
            { return true; }
            else
            { return false; }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Input csv file to parse
            //string filePath = @".\resources\exhibitA-input.csv";
            Console.WriteLine(@"Please put input file in C:\piworks\exhibitA-input.csv and press key");
            Console.ReadKey();
            string filePath = @"C:\piworks\exhibitA-input.csv";
            Console.WriteLine(filePath);

            HashSet<PlayedSong> playSet = new HashSet<PlayedSong>();

            if (File.Exists(filePath))
            {
                StreamReader reader = new StreamReader(File.OpenRead(filePath));
                string header = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var columns = line.Split('\t');

                    PlayedSong played = new PlayedSong();
                    //played.play_id = columns[0];
                    played.song_id = columns[1];
                    played.client_id = columns[2];
                    //played.play_ts = columns[3];

                    playSet.Add(played);
                }
            }
            else
            {
                Console.WriteLine("File doesn't exist");
            }

            PlayedSong[] playList = playSet.ToArray();

            // Output file contains distinct client, song records
            //string writePath = @".\resources\exhibitA-output.csv";
            string writePath = @"C:\piworks\exhibitA-output.csv";
            Console.WriteLine(@"Intermediate distinct records --> C:\piworks\exhibitA-output.csv");
            Console.WriteLine(writePath);
            StreamWriter writer = new StreamWriter(File.OpenWrite(writePath));
            writer.WriteLine("CLIENT_ID\tSONG_ID");

            foreach (PlayedSong played in playList)
            {
                writer.WriteLine(played.client_id + "\t" + played.song_id);
            }
            writer.Flush();
            writer.Close();
            Console.WriteLine("Output file write is over");

            // [Q2] How many users played 346 distinct songs? 
            // select client_id, count(*) 
            // from ( select client_id, song_id, count(*)  
            //        from this 
            //        group by client_id, song_id ) 
            // group by client_id having count(*)=1346
            int cnt346 = playList
                .GroupBy(client => client.client_id)
                .Select((id, cnt) => (id.Key, id.Count()))
                .Where(count => count.Item2 == 346)
                .Count();
            Console.WriteLine("[Q2] How many users played 346 distinct songs? {0} ", cnt346);

            // [Q3] What is the maximum number of distinct songs  played ?
            int cntMax = playList
                .GroupBy(client => client.client_id)
                .Select((id, cnt) => (id.Key, id.Count()))
                .Max(max => max.Item2);
            Console.WriteLine("[Q3] What is the maximum number of distinct songs  played ? {0} ", cntMax);

            Console.ReadKey();
        }
    }
}