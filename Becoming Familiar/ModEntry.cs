using System;
using GenericModConfigMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace Becoming_Familiar
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        private ModConfig _config = null!;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private static List<string> _villagers = new();

        public override void Entry(IModHelper helper)
        {
            _config = Helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
            helper.Events.GameLoop.DayEnding += GameLoopOnDayEnding;
        }
        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
            {
                return;
            }
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => _config = new ModConfig(),
                save: () => Helper.WriteConfig(_config)
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Show Notification",
                tooltip: () => "Show a notification when you see a villager",
                getValue: () => _config.ShowNotification,
                setValue: value => _config.ShowNotification = value
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => "Notification Duration",
                tooltip: () => "How long the notification will be displayed in milliseconds (1000 = 1 second)",
                getValue: () => _config.NotificationDuration,
                setValue: value => _config.NotificationDuration = value
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => "Range",
                tooltip: () => "The range in which the mod will detect villagers",
                getValue: () => _config.Range,
                setValue: value => _config.Range = value
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => "How much friendship",
                tooltip: () => "How much friendship you will gain with the villager." + Environment.NewLine + "1 Heart is 250 points, Daily decay of -2 for a normal NPC (not dating, married, etc).",
                getValue: () => _config.HowMuchFriendShip,
                setValue: value => _config.HowMuchFriendShip = value
            );
        }

        private void GameLoopOnDayEnding(object? sender, DayEndingEventArgs e)
        {
            _villagers.Clear();
        }
        
        private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
        {
            //Check if the game is ready
            if (!Context.IsWorldReady)
                return;
            //Check for any villagers on screen
            foreach (var npc in Game1.currentLocation.characters)
            {
                if (npc is NPC villager && villager.IsVillager)
                {
                    Vector2 villagerPosition = villager.Tile;
                    Vector2 playerPosition = Game1.player.Tile;
                    var distance = Vector2.Distance(villagerPosition, playerPosition);
                    if (distance < _config.Range && !_villagers.Contains(villager.Name))
                    {
                        _villagers.Add(villager.Name);
                        //Add +3 to the player's friendship with the villager
                        Game1.player.changeFriendship(_config.HowMuchFriendShip, villager);
                        if (_config.ShowNotification)
                        {
                            
                            var friendship = Game1.player.getFriendshipLevelForNPC(villager.Name);
                            //divide friendship by 250 and round to nearest whole number
                            var hearts = Math.Round(friendship / 250.0);
                            //Select on hearts, 1 - 14, and then display the appropriate message
                            switch (hearts)
                            {
                                case 1:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} notices your presence.",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                                case 2:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} grins in your direction.",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                                case 3:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} smiles when they see you.",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                                case 4:
                                    Game1.addHUDMessage(
                                        new HUDMessage($"{villager.Name} smiles widely when they see you!",
                                            HUDMessage.achievement_type)
                                        {
                                            timeLeft = _config.NotificationDuration
                                        });
                                    break;
                                case 5:
                                    Game1.addHUDMessage(
                                        new HUDMessage($"{villager.Name} says a friendly greeting before continuing.",
                                            HUDMessage.achievement_type)
                                        {
                                            timeLeft = _config.NotificationDuration
                                        });
                                    break;
                                case 6:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} says a few words about you.",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                                case 7:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} is blushing.",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                                case 8:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} is excited to see you!",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                                case 9:
                                    Game1.addHUDMessage(
                                        new HUDMessage(
                                            $"{villager.Name} makes a comment about enjoying spending time around you.",
                                            HUDMessage.achievement_type)
                                        {
                                            timeLeft = _config.NotificationDuration
                                        });
                                    break;
                                case 10:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} is happy to see you!",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                                case 11:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} is very happy to see you!",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                                default:
                                    Game1.addHUDMessage(new HUDMessage($"{villager.Name} is overjoyed to see you!",
                                        HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public class ModConfig
        {
            public bool ShowNotification { get; set; } = false;
            public int NotificationDuration { get; set; } = 5000;
            public int Range { get; set; } = 25;
            public int HowMuchFriendShip { get; set; } = 3;
        }
    }
}