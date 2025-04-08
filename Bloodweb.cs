using Newtonsoft.Json;

namespace Melancholy
{
    public static class Bloodweb
    {
        public static async Task Generate_Bloodweb(string filename = "Bloodweb.json", bool perkStatus = true)
        {
            Classes.BloodwebData bloodwebObj = new()
            {
                PrestigeLevel = Market.PrestigeLevel == 0 ? new Random().Next(Market.PrestigeLevelMinimum, Market.PrestigeLevelMaximum) : Market.PrestigeLevel,
                BloodWebData = BloodwebGenerator.Make_Bloodweb("EPlayerRole::VE_None", "", perkStatus)
            };

            var json = JsonConvert.SerializeObject(bloodwebObj, Formatting.Indented);
            await File.WriteAllTextAsync($"Files/{filename}", json);
        }
    }
}
