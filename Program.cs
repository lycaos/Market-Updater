using Melancholy;
using Newtonsoft.Json;

try
{
    Console.Title = "DBD OwO";
    Extras.Header();
    Extras.MigrateFiles();

    if (!Directory.Exists("Files")) Directory.CreateDirectory("Files");
    if (!Directory.Exists("Other")) Directory.CreateDirectory("Other");
    if (!Directory.Exists("IDs")) Directory.CreateDirectory("IDs");

    if (File.Exists(Extras.SettingsFile))
    {
        Extras.Settings = JsonConvert.DeserializeObject<Classes.Settings>(File.ReadAllText(Extras.SettingsFile));

        Console.WriteLine(
            $"Current settings:\nPak path: {Extras.Settings.PakPath}\nAES key: {Extras.Settings.AesKey}\nMappings path: {Extras.Settings.MappingsPath}\n");
        Console.Write("Would you like to load settings? (Y/n): ");
        switch (Console.ReadLine().ToLower())
        {
            case "no":
            case "n":
                goto PopulateSettings;
            default:
                goto SkipSettings;
        }
    }

PopulateSettings:
	string directoryPath = AppContext.BaseDirectory;
    var usmapFiles = Directory.GetFiles(directoryPath, "*.usmap");

    if (usmapFiles.Length == 0)
        Extras.Settings.MappingsPath = Extras.PromptInput("Provide DBD Mapping file: ");
	else {
		Console.WriteLine("Select a mappings file by entering its number:");
		for (int i = 0; i < usmapFiles.Length; i++)
		{
			Console.WriteLine($"{i + 1}. {Path.GetFileName(usmapFiles[i])}");
		}

		int choice = 0;
		while (true)
		{
			Console.Write("Enter your choice (number): ");
			if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= usmapFiles.Length) break;
			Console.WriteLine("Invalid choice, try again...");
		}

		string selectedFile = usmapFiles[choice - 1];
		Extras.Settings.MappingsPath = selectedFile;
	}
	
    Extras.Settings.PakPath = Extras.PromptInput("Provide path to your paks folder: ");
    Extras.Settings.AesKey = Extras.PromptInput("Provide AES key: ");
	
    File.WriteAllText(Extras.SettingsFile, JsonConvert.SerializeObject(Extras.Settings, Formatting.Indented));

SkipSettings:
    Extras.Header();

    Console.WriteLine("Doing CUE4Parse stuffs...");

    Cue4Parse.Initialize();
    Cue4Parse.CdnAccessKey = Cue4Parse.GetAccessKey();
    Cue4Parse.Get_Files();
    if(Classes.Ids.DlcIds.Count < 79)
    {
        Console.WriteLine($"{Classes.Ids.DlcIds.Count} characters founded, they should be at least 79");
        Extras.populateDlcs();
    }

    if (Cue4Parse.IsListEmpty())
        throw new Exception("Mappings file outdated, contact @bhvr on Discord for the new mappings file.");

    Classes.Choices choices = Extras.LoadChoices(Extras.ChoicesFile);

    if(!choices.LoadedFromFile || (choices.LoadedFromFile && Extras.PromptOptionInput("Do you want to load your previous choices? (y/N): ") == 'n'))
    {
        choices.ItemAmount = Market.ItemAmount = Extras.PromptIntInput("Enter desired item amount (0 for random): ");

        choices.PrestigeLevel = Market.PrestigeLevel = Extras.PromptIntInput("Enter desired prestige for characters (0 for random): ");
        choices.PrestigeLevelMinimum = 0;
        choices.PrestigeLevelMaximum = 0;

        if (Market.PrestigeLevel == 0)
        {
            choices.PrestigeLevelMinimum = Market.PrestigeLevelMinimum = Extras.PromptIntInput("Enter minimum desired prestige: ");
            choices.PrestigeLevelMaximum = Market.PrestigeLevelMaximum = Extras.PromptIntInput("Enter maximum desired prestige: ");
        }

        choices.PlayerLevel = Player.PlayerLevel = Extras.PromptIntInput("Enter desired player level: ");
        choices.DevotionLevel = Player.DevotionLevel = Extras.PromptIntInput("Enter desired devotion level: ");

        Extras.Header();

        char ShouldHaveNonInventoryItems = Extras.PromptOptionInput("Should non-inventory items be included such as Onryo Tape, Wesker Spray, etc...? (y/N): ");
        choices.AddNonInventoryItems = Extras.AddNonInventoryItems = ShouldHaveNonInventoryItems is 'y';

        char ShouldHaveRetiredOfferings = Extras.PromptOptionInput("Should retired offerings be included such as killer splinters? (y/N): ");
        choices.AddRetiredOfferings = Extras.AddRetiredOfferings = ShouldHaveRetiredOfferings is 'y';

        char ShouldHaveEventItems = Extras.PromptOptionInput("Should event items be included such as anniversary cakes? (Y/n): ");
        choices.AddEventItems = Extras.AddEventItems = ShouldHaveEventItems is 'y';

        char ShouldHaveBannersBadges = Extras.PromptOptionInput("Should banners and badges be included? (Y/n): ");
        choices.AddBannersBadges = Extras.AddBannersBadges = ShouldHaveBannersBadges is 'y';

        char ShouldHaveLegacy = Extras.PromptOptionInput("Should legacy (pre 2016) prestige clothing be added? (Y/n): ");
        choices.AddLegacy = Extras.AddLegacy = ShouldHaveLegacy is 'y';

        char ShouldHaveScaryItems = Extras.PromptOptionInput("Should \"scary\" things be included (like dev only items etc.)? (y/N): ");
        choices.AddScaryItems = Extras.AddScaryItems = ShouldHaveScaryItems is 'y';
    
        Extras.SaveChoices(Extras.ChoicesFile, choices);
    }
    else
    {
        Market.ItemAmount = choices.ItemAmount;
        Market.PrestigeLevel = choices.PrestigeLevel;
        Market.PrestigeLevelMinimum = choices.PrestigeLevelMinimum;
        Market.PrestigeLevelMaximum = choices.PrestigeLevelMaximum;
        Player.PlayerLevel = choices.PlayerLevel;
        Player.DevotionLevel = choices.DevotionLevel;
        Extras.AddNonInventoryItems = choices.AddNonInventoryItems;
        Extras.AddRetiredOfferings = choices.AddRetiredOfferings;
        Extras.AddEventItems = choices.AddEventItems;
        Extras.AddBannersBadges = choices.AddBannersBadges;
        Extras.AddLegacy = choices.AddLegacy;
        Extras.AddScaryItems = choices.AddScaryItems;
    }

    bool cdnStatus = Extras.PromptOptionInput("Do you want to generate Catalog and Killswitch? (y/N): ") == 'y';
    Extras.Header();

    if(cdnStatus)
        await Cdn.GetCdnFiles();

    await Extras.MakeFile(Classes.Ids.CosmeticIds, "Cosmetics.json");
    await Extras.MakeFile(Classes.Ids.OutfitIds, "Outfits.json");
    await Extras.MakeFile(Classes.Ids.DlcIds, "Dlc.json");
    await Extras.MakeFile(Classes.Ids.ItemIds, "Items.json");
    await Extras.MakeFile(Classes.Ids.AddonIds, "Addons.json");
    await Extras.MakeFile(Classes.Ids.OfferingIds, "Offerings.json");
    await Extras.MakeFile(Classes.Ids.PerkIds, "Perks.json");

    await GeneratePrestigeList.GenerateList();

    await Market.Generate_Market("all");
    await Market.Generate_Market("dlconly");
    await Market.Generate_Market("notemp");
    await Market.Generate_Market("tempNoCosmetics");
    await Market.Generate_Market("perks");
    Console.WriteLine("Market files generated");

    await GetAll.Generate_GetAll();
    Console.WriteLine("GetAll.json generated");

    await Bloodweb.Generate_Bloodweb("BloodwebNoPerks.json", false);
    Console.WriteLine("BloodwebNoPerks.json generated");
    await Bloodweb.Generate_Bloodweb();
    Console.WriteLine("Bloodweb.json generated");

    await Player.Generate_PlayerLevel();

    Console.WriteLine($"Key to decrypt CDN stuff (Catalog, KillSwitch..): {Cue4Parse.CdnAccessKey}");

    Console.WriteLine("\nPress any key to close...");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.ReadLine();
}