using Newtonsoft.Json;

namespace Melancholy
{
    public static class Extras
    {
        public static Classes.Settings Settings = new();
        public static bool AddNonInventoryItems { get; set; } = false;
        public static bool AddRetiredOfferings { get; set; } = false;
        public static bool AddEventItems { get; set; } = true;
        public static bool AddBannersBadges { get; set; } = true;
        public static bool AddScaryItems { get; set; } = false;
        public static bool AddLegacy { get; set; } = true;
        public const string BlacklistFile = "Other/Blacklist.json";
        public const string ChoicesFile = "Other/Choices.json";
        public const string SettingsFile = "Other/Settings.json";
        private static int Padding(string output) => (Console.WindowWidth / 2) - (output.Length / 2);
        private static void CenterText(string text) => Console.WriteLine("{0}{1}", new string(' ', Padding(text)), text);

        public static void populateDlcs()
        {
            Classes.Ids.DlcIds.Clear();
            List<Classes.Character> characters = new List<Classes.Character>
            {
                new Classes.Character { CharacterName = "Ace", CharacterIndex = "6", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Adam", CharacterIndex = "13", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Ash", CharacterIndex = "16", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Bear", CharacterIndex = "268435463", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Bill", CharacterIndex = "7", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Bob", CharacterIndex = "268435457", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Cannibal", CharacterIndex = "268435464", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Chuckles", CharacterIndex = "268435456", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Claudette", CharacterIndex = "2", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Clown", CharacterIndex = "268435467", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Demogorgon", CharacterIndex = "268435472", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Dwight", CharacterIndex = "0", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Eric", CharacterIndex = "12", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Feng", CharacterIndex = "8", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Ghostface", CharacterIndex = "268435471", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Gunslinger", CharacterIndex = "268435474", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "HillBilly", CharacterIndex = "268435458", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Jake", CharacterIndex = "3", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Jane", CharacterIndex = "15", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Jeff", CharacterIndex = "14", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Kate", CharacterIndex = "10", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Killer07", CharacterIndex = "268435462", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Laurie", CharacterIndex = "5", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Legion", CharacterIndex = "268435469", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Meg", CharacterIndex = "1", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Nancy", CharacterIndex = "17", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Nea", CharacterIndex = "4", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Nightmare", CharacterIndex = "268435465", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Nurse", CharacterIndex = "268435459", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Oni", CharacterIndex = "268435473", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Pig", CharacterIndex = "268435466", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Plague", CharacterIndex = "268435470", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Quentin", CharacterIndex = "11", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Shape", CharacterIndex = "268435461", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Smoke", CharacterIndex = "9", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Spirit", CharacterIndex = "268435468", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Steve", CharacterIndex = "18", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Witch", CharacterIndex = "268435460", CharacterType = "EPlayerRole::VE_Slasher" },
                new Classes.Character { CharacterName = "Yui", CharacterIndex = "19", CharacterType = "EPlayerRole::VE_Camper" },
                new Classes.Character { CharacterName = "Zarina", CharacterIndex = "20", CharacterType = "EPlayerRole::VE_Camper" }
            };

            int killerIndex = PromptIntInput("Write lastest killer number (Exampe K36 The Litch -> 36): ");
            if(killerIndex < 36) killerIndex = 36;
            int survivorIndex = PromptIntInput("Write lastest survivor number (Exampe S43 Lara Croft -> 43): ");
            if(survivorIndex < 43) survivorIndex = 43;
            const long baseKillerIndex = 268435455;
            for(int i=20;i<=killerIndex;i++){
                characters.Add(new Classes.Character { CharacterName = $"K{i}" , CharacterIndex = $"{baseKillerIndex+i}" , CharacterType = "EPlayerRole::VE_Slasher" });
            }
            for(int i=22;i<=survivorIndex;i++){
                characters.Add(new Classes.Character { CharacterName = $"S{i}" , CharacterIndex = $"{i-1}" , CharacterType = "EPlayerRole::VE_Camper" });
            }
            Classes.Ids.DlcIds.AddRange(characters);
        }

        public static void Header()
        {
			Console.Clear();
            CenterText("\"Two facts: boys make better girls and furries are superior to all\" - Essence");
            CenterText("MarketUpdater developed by Essence_BHVR (discord: bhvr)");
            Console.WriteLine(Environment.NewLine);
        }

        public static async Task MakeFile<T>(List<T> list, string fileName)
        {
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            await using (var writer = new StreamWriter($"IDs/{fileName}"))
            {
                await writer.WriteAsync(json);
            }

            Console.WriteLine($"{fileName} generated");
        }

        public static string PromptInput(string message)
        {
            string input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));

            return input.Replace("\"", "");
        }

        public static char PromptOptionInput(string message)
        {
            Console.Write(message);
            char input = Console.ReadLine().ToLower()[0];

            return input;
        }

        public static int PromptIntInput(string message)
        {
            int parsedInt = 0;
            string input;

            do
            {
                input = PromptInput(message);
                if (!int.TryParse(input, out parsedInt))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            } while (parsedInt == 0 & !int.TryParse(input, out parsedInt));

            return parsedInt;
        }

        public static void SaveChoices(string FilePath, Classes.Choices choices)
        {
            var json = JsonConvert.SerializeObject(choices, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static Classes.Choices LoadChoices(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var choices = JsonConvert.DeserializeObject<Classes.Choices>(json);
                choices.LoadedFromFile = true;
                Extras.Header();
                Console.WriteLine("Current choices:");
                foreach (var property in typeof(Classes.Choices).GetProperties())
                {
                    if(property.Name == "LoadedFromFile") continue;
                    var value = property.GetValue(choices);
                    Console.WriteLine($"{property.Name}: {value}");
                }
                return choices;
            }
            return new Classes.Choices();
        }

        public static void MigrateFiles()
        {
            if(File.Exists("blacklist.json")) File.Move("blacklist.json", BlacklistFile);
            if(File.Exists("choices.json")) File.Move("choices.json", ChoicesFile);
            if(File.Exists("settings.json")) File.Move("settings.json", SettingsFile);
        }
    }
}
