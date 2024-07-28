using Newtonsoft.Json;

namespace Melancholy
{
    public static class GetAll
    {
        public static async Task Generate_GetAll()
        {
            Classes.GetAllData getAllData = new();

            getAllData.List.AddRange(Classes.Ids.DlcIds.Select(character => new Classes.CharacterItem
            {
                CharacterName = character.CharacterName,
                CharacterItems = GenerateItemData(character),
                BloodWebData = BloodwebGenerator.Make_Bloodweb(character.CharacterType, character.CharacterDefaultItem),
                PrestigeLevel = Market.PrestigeLevel == 0 ? new Random().Next(Market.PrestigeLevelMinimum, Market.PrestigeLevelMaximum) : Market.PrestigeLevel
            }));

            var json = JsonConvert.SerializeObject(getAllData, Formatting.Indented);
            await File.WriteAllTextAsync("Files/GetAll.json", json);
        }

        private static List<Classes.ItemBloodweb> GenerateItemData(Classes.Character character)
        {
            List<Classes.ItemBloodweb> characterItems = [];
            
            characterItems.AddRange(
                Classes.Ids.ItemIds
                    .Where(item =>
                    {
                        bool includeItem = true;

                        if (item.CharacterType != "EPlayerRole::VE_None" && character.CharacterType != item.CharacterType) includeItem = false;
                        if (!Extras.AddNonInventoryItems && !item.ShouldBeInInventory) includeItem = false;
                        if (!Extras.AddEventItems && item.EventId != "None") includeItem = false;

                        return includeItem;
                    })
                    .Select(item => new Classes.ItemBloodweb
                    {
                        ItemId = item.ItemId,
                        Quantity = (Market.ItemAmount == 0 ? new Random().Next(8, 88) : Market.ItemAmount)
                    }));
            
            characterItems.AddRange(
                Classes.Ids.AddonIds
                    .Where(addon =>
                    {
                        bool includeAddon = true;

                        if(addon.CharacterType != "EPlayerRole::VE_None" && character.CharacterType != addon.CharacterType) includeAddon = false;
                        if (character.CharacterDefaultItem != "" && character.CharacterType == "EPlayerRole::VE_Slasher" &&
                            character.CharacterDefaultItem != addon.CharacterDefaultItem) includeAddon = false;
                        if (!Extras.AddNonInventoryItems && !addon.ShouldBeInInventory) includeAddon = false;
                        if (!Extras.AddEventItems && addon.EventId != "None") includeAddon = false;

                        return includeAddon;
                    })
                    .Select(addon => new Classes.ItemBloodweb
                    {
                        ItemId = addon.ItemId,
                        Quantity = Market.ItemAmount == 0 ? new Random().Next(8, 88) : Market.ItemAmount
                    }));
            
            characterItems.AddRange(
                Classes.Ids.OfferingIds
                    .Where(offering =>
                    {
                        bool includeOffering = true;

                        if(offering.CharacterType != "EPlayerRole::VE_None" && character.CharacterType != offering.CharacterType) includeOffering = false;
                        if (!Extras.AddNonInventoryItems && !offering.ShouldBeInInventory) includeOffering = false;
                        if (!Extras.AddEventItems && offering.EventId != "None") includeOffering = false;
                        if (!Extras.AddRetiredOfferings && offering.Availability == "EItemAvailability::Retired")
                            includeOffering = false;

                        return includeOffering;
                    })
                    .Select(offering => new Classes.ItemBloodweb
                    {
                        ItemId = offering.ItemId,
                        Quantity = Market.ItemAmount == 0 ? new Random().Next(8, 88) : Market.ItemAmount
                    }));

            characterItems.AddRange(
                Classes.Ids.PerkIds
                    .Where(perk => 
                        character.CharacterType == perk.CharacterType ||
                        perk.CharacterType == "EPlayerRole::VE_None")
                    .Select(perk => new Classes.ItemBloodweb
                    {
                        ItemId = perk.ItemId,
                        Quantity = 3
                    }));

            return characterItems;
        }
    }
}
