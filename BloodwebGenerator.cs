namespace Melancholy
{
    public static class BloodwebGenerator
    {
        public static List<Classes.ItemBloodweb> TivoTigs = [];
        public static List<Classes.InventoryItemBloodweb> BloodwebList = [];

        public static Classes.Bloodweb Make_Bloodweb(string characterType, string characterPower, bool perkStatus = true)
        {
            TivoTigs.Clear();
            BloodwebList.Clear();

            foreach (var item in ConvertToInventoryItemBloodweb(Classes.Ids.ItemIds))
                if (item is Classes.InventoryItemBloodweb inventoryItem)
                {
                    if (!Extras.AddNonInventoryItems && !inventoryItem.ShouldBeInInventory) continue;
                    if (!Extras.AddEventItems && inventoryItem.EventId != "None") continue;

                    BloodwebList.Add(new Classes.InventoryItemBloodweb
                    {
                        ItemId = inventoryItem.ItemId,
                        CharacterType = inventoryItem.CharacterType,
                        CharacterDefaultItem = ""
                    });
                }

            foreach (var item in ConvertToInventoryItemBloodweb(Classes.Ids.AddonIds))
                if (item is Classes.InventoryItemBloodweb addon)
                {
                    if (characterType.Contains("Slasher") && (addon.CharacterDefaultItem != characterPower)) continue;
                    if (!Extras.AddNonInventoryItems && !addon.ShouldBeInInventory) continue;
                    if (!Extras.AddEventItems && addon.EventId != "None") continue;

                    BloodwebList.Add(new Classes.InventoryItemBloodweb
                    {
                        ItemId = addon.ItemId,
                        CharacterType = addon.CharacterType,
                        CharacterDefaultItem = addon.CharacterDefaultItem,
                    });
                }

            foreach (var item in ConvertToInventoryItemBloodweb(Classes.Ids.OfferingIds))
                if (item is Classes.InventoryItemBloodweb offering)
                {
                    if (!Extras.AddNonInventoryItems && !offering.ShouldBeInInventory) continue;
                    if (!Extras.AddEventItems && offering.EventId != "None") continue;
                    if (!Extras.AddRetiredOfferings && offering.Availability == "EItemAvailability::Retired") continue;

                    BloodwebList.Add(new Classes.InventoryItemBloodweb
                    {
                        ItemId = offering.ItemId,
                        CharacterType = offering.CharacterType,
                        CharacterDefaultItem = ""
                    });
                }

            foreach (var item in Classes.Ids.ItemIds)
            {
                if (!Extras.AddNonInventoryItems && !item.ShouldBeInInventory) continue;
                if (!Extras.AddEventItems && item.EventId != "None") continue;

                TivoTigs.Add(new Classes.ItemBloodweb
                {
                    ItemId = item.ItemId,
                    Quantity = (Market.ItemAmount == 0 ? new Random().Next(8, 88) : Market.ItemAmount)
                });
            }

            foreach (var item in Classes.Ids.AddonIds)
            {
                if (!Extras.AddNonInventoryItems && !item.ShouldBeInInventory) continue;
                if (!Extras.AddEventItems && item.EventId != "None") continue;

                TivoTigs.Add(new Classes.ItemBloodweb
                {
                    ItemId = item.ItemId,
                    Quantity = (Market.ItemAmount == 0 ? new Random().Next(8, 88) : Market.ItemAmount)
                });
            }

            foreach (var item in Classes.Ids.OfferingIds)
            {
                if (!Extras.AddNonInventoryItems && !item.ShouldBeInInventory) continue;
                if (!Extras.AddEventItems && item.EventId != "None") continue;
                if (!Extras.AddRetiredOfferings && item.Availability == "EItemAvailability::Retired") continue;

                TivoTigs.Add(new Classes.ItemBloodweb
                {
                    ItemId = item.ItemId,
                    Quantity = (Market.ItemAmount == 0 ? new Random().Next(8, 88) : Market.ItemAmount)
                });
            }

            if (perkStatus)
                foreach (var item in Classes.Ids.PerkIds)
                    TivoTigs.Add(new Classes.ItemBloodweb { ItemId = item.ItemId, Quantity = 3 });

            var pathData = new List<string>
            {
                
            };

            var ringData = new List<Classes.Ring>
            {
                new Classes.Ring
                {
                    NodeData = [
                        new Classes.Node { NodeId = 0 }
                    ]
                }
            };

            return new Classes.Bloodweb
            {
                Paths = pathData,
                RingData = ringData
            };
        }

        private static string GetRandomId(string characterType)
        {
            var filteredList = BloodwebList.Where(item => item.CharacterType == characterType).ToList();

            if (filteredList.Count == 0) return string.Empty;

            Random random = new();
            int randIndex = random.Next(filteredList.Count);

            return filteredList[randIndex].ItemId;
        }

        private static List<Classes.InventoryItemBloodweb> ConvertToInventoryItemBloodweb(IEnumerable<Classes.ItemOfferingPerk> items)
        {
            var bloodwebItem = items.Select(item => new Classes.InventoryItemBloodweb
            {
                ItemId = item.ItemId,
                CharacterType = item.CharacterType,
                CharacterDefaultItem = ""
            }).ToList();

            return bloodwebItem;
        }
    }
}
