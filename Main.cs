using BepInEx;
using System.Collections.Generic;
using RoR2;
using System;
using BepInEx.Configuration;

namespace StageRemover
{
    //this line defines your mod name to BepIn, set later
    [BepInPlugin(ModGuid, ModName, ModVer)]

    //defines the mod
    public class Main : BaseUnityPlugin
    {

        //enter the mod GUID inside quotes. Ex: com.Derslayr.CreateItemTemplate
        public const string ModGuid = "com.Derslayr.StageRemover";
        //enter the mod name inside quotes. Ex: TwistedLoopMod
        public const string ModName = "Stage Remover";
        //enter the mod version inside quotes. Ex: 1.0.0
        public const string ModVer = "2.0.1";

        //Define configs
        private Dictionary<String, bool> SceneConfig = new Dictionary<String, bool>();

        public static string CustomStages { get; set; }
        public void RunConfig()
        {
            SceneConfig.Add("blackbeach", Config.Bind<bool>("Vanilla Maps", "enableRoost", true, "Enable Distant Roost in Stage Destinations? Default: true").Value);
            SceneConfig.Add("golemplains", Config.Bind<bool>("Vanilla Maps", "enablePlains", true, "Enable Titanic Plains in Stage Destinations? Default: true").Value);
            SceneConfig.Add("goolake", Config.Bind<bool>("Vanilla Maps", "enableAqueduct", true, "Enable Abandoned Aqueduct in Stage Destinations? Default: true").Value);
            SceneConfig.Add("foggyswamp", Config.Bind<bool>("Vanilla Maps", "enableWetland", true, "Enable Wetland Aspect in Stage Destinations? Default: true").Value);
            SceneConfig.Add("frozenwall", Config.Bind<bool>("Vanilla Maps", "enableRallypoint", true, "Enable Rallypoint Delta in Stage Destinations? Default: true").Value);
            SceneConfig.Add("wispgraveyard", Config.Bind<bool>("Vanilla Maps", "enableAcres", true, "Enable Scorched Acres in Stage Destinations? Default: true").Value);
            SceneConfig.Add("dampcavesimple", Config.Bind<bool>("Vanilla Maps", "enableDepths", true, "Enable Abyssal Depths in Stage Destinations? Default: true").Value);
            SceneConfig.Add("shipgraveyard", Config.Bind<bool>("Vanilla Maps", "enableSiren", true, "Enable Siren's Call in Stage Destinations? Default: true").Value);
            SceneConfig.Add("rootjungle", Config.Bind<bool>("Vanilla Maps", "enableGrove", true, "Enable Sundered Grove in Stage Destinations? Default: true").Value);
            SceneConfig.Add("skymeadow", Config.Bind<bool>("Vanilla Maps", "enableMeadow", true, "Enable Sky Meadow in Stage Destinations? Default: true").Value);

            var customScenes = Config.Bind<string>("Custom Stages", "customStages", "", "Please enter the Scene Name of each modded map you wish to remove (Ex: Titanic Plains has scene name 'golemplains'). Separate each entry with a comma.");
            foreach (var customScene in customScenes.Value.Split(new[] { ',' }))
            {
                SceneConfig.Add(customScene, false);
            }
        }


        public void Awake()
        {
            RunConfig();
            Hooks();
        }

        public void Hooks()
        {
            On.RoR2.Run.PickNextStageScene += PickValidScene;
        }

        private void PickValidScene(On.RoR2.Run.orig_PickNextStageScene orig, Run self, SceneDef[] choices)
        {
            var newChoices = new List<SceneDef>();
            foreach(var choice in choices)
            {
                if( !(SceneConfig.ContainsKey(choice.baseSceneName) && !SceneConfig[choice.baseSceneName]))
                {
                    newChoices.Add(choice);
                }
            }
            orig(self, newChoices.Count > 0 ? newChoices.ToArray() : choices);

        }

    }

}
