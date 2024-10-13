using GenericModConfigMenu;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Becoming_Familiar;

/// <summary>The mod entry point.</summary>
internal sealed class ModEntry : Mod
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static List<string> _villagers = new();
    private ModConfig _config = null!;

    public override void Entry(IModHelper helper)
    {
        _config = Helper.ReadConfig<ModConfig>();
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
        helper.Events.GameLoop.DayEnding += GameLoopOnDayEnding;
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null) return;
        configMenu.Register(
            ModManifest,
            () => _config = new ModConfig(),
            () => Helper.WriteConfig(_config)
        );
        configMenu.AddBoolOption(
            ModManifest,
            name: () => Helper.Translation.Get("settings.show-notifications"),
            tooltip: () => Helper.Translation.Get("settings.show-notifications.description"),
            getValue: () => _config.ShowNotification,
            setValue: value => _config.ShowNotification = value
        );
        configMenu.AddBoolOption(
            ModManifest,
            name: () => Helper.Translation.Get("settings.show-bubbles"),
            tooltip: () => Helper.Translation.Get("settings.show-bubbles.description"),
            getValue: () => _config.ShowBubbles,
            setValue: value => _config.ShowBubbles = value
        );
        configMenu.AddNumberOption(
            ModManifest,
            name: () => Helper.Translation.Get("settings.notification-duration"),
            tooltip: () => Helper.Translation.Get("settings.notification-duration.description"),
            getValue: () => _config.NotificationDuration,
            setValue: value => _config.NotificationDuration = value
        );

        configMenu.AddNumberOption(
            ModManifest,
            name: () => Helper.Translation.Get("settings.range"),
            tooltip: () => Helper.Translation.Get("settings.range.description"),
            getValue: () => _config.Range,
            setValue: value => _config.Range = value
        );
        string formattedString = Helper.Translation.Get("settings.how-much-friendship.description");
        formattedString = formattedString.Replace("{{newline}}", Environment.NewLine);
        configMenu.AddNumberOption(
            ModManifest,
            name: () => Helper.Translation.Get("settings.how-much-friendship"),
            tooltip: () => formattedString,
            getValue: () => _config.HowMuchFriendShip,
            setValue: value => _config.HowMuchFriendShip = value
        );
        configMenu.AddParagraph(
            ModManifest,
            () => Helper.Translation.Get("settings.request-for-translations")
            );
        configMenu.AddParagraph(
            ModManifest,
            () => Helper.Translation.Get("settings.request-for-accidental-encounters")
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
            if (npc is NPC villager && villager.IsVillager)
            {
                var villagerPosition = villager.Tile;
                var playerPosition = Game1.player.Tile;
                var distance = Vector2.Distance(villagerPosition, playerPosition);
                if (distance < _config.Range && !_villagers.Contains(villager.Name))
                {
                    _villagers.Add(villager.Name);
                    //Add +3 to the player's friendship with the villager
                    Game1.player.changeFriendship(_config.HowMuchFriendShip, villager);

                    Random rand = new Random();
                    var friendship = Game1.player.getFriendshipLevelForNPC(villager.Name);
                    //divide friendship by 250 and round to nearest whole number
                    var hearts = Math.Floor(friendship / 250.0);

                    if (_config.ShowNotification)
                    {

                        //Select on hearts, 1 - 14, and then display the appropriate message

                        
///RSV + SH's animals + Downtown Zuzu temporary patch??
                   {

                        //Select on hearts, 1 - 14, and then display the appropriate message
                        if (villager.Name.EndsWith("WA"))
                        {
                            //SH's Wild Animals
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "A wild animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.Contains("Tourist"))
                        {
                            //Tourist
                                string message = Helper.Translation.Get($"notifications.non-datable.{hearts}-heart");
                                message = message.Replace("{{villager}}", "A tourist");
                                Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                                {
                                    timeLeft = _config.NotificationDuration
                                });
                        }

                        if (villager.Name.EndsWith("DA"))
                        {
                            //SH's Domestic Animals
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }


                        if (villager.Name.StartsWith("Child"))
                        {
                            int randomMessage = rand.Next(0, 9); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.child.{randomMessage}");
                            message = message.Replace("{{villager}}", villager.displayName);
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });


                        }

                        if (villager.Name.StartsWith("Joja"))
                        {
                            return;
                        }

                        if (villager.Name.StartsWith("Man"))
                        {
                                string message = Helper.Translation.Get($"notifications.non-datable.{hearts}-heart");
                                message = message.Replace("{{villager}}", "A man");
                                Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                                {
                                    timeLeft = _config.NotificationDuration
                                });
                        }

                        if (villager.Name.StartsWith("Pigeon"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.StartsWith("Woman"))
                        {
                                string message = Helper.Translation.Get($"notifications.non-datable.{hearts}-heart");
                                message = message.Replace("{{villager}}", "A woman");
                                Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                                {
                                    timeLeft = _config.NotificationDuration
                                });
                        }

                        if (villager.Name.StartsWith("BrownChicken"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.StartsWith("FarmKiwi"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.StartsWith("BrownCow"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.StartsWith("Guest"))
                        {
                                string message = Helper.Translation.Get($"notifications.non-datable.{hearts}-heart");
                                message = message.Replace("{{villager}}", "Guest");
                                Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                                {
                                    timeLeft = _config.NotificationDuration
                                });
                        }

                        if (villager.Name.StartsWith("Howdy"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.StartsWith("MistFox"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.StartsWith("RingSpirit"))
                        {
                            return;
                        }

                        if (villager.Name.StartsWith("Spirit"))
                        {
                            return;
                        }

                        if (villager.Name.StartsWith("WanderingChicken"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.StartsWith("WhiteChicken"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }

                        if (villager.Name.StartsWith("WhiteCow"))
                        {
                            int randomMessage = rand.Next(0, 3); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.animal.{randomMessage}");
                            message = message.Replace("{{villager}}", "An animal");
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }
                        
///RSV + SH's animals + Downtown Zuzu temporary patch??






                        //Check if the NPC is datable
                        int result = rand.Next(0, 101);
                        if (result == 100)
                        {
                            int messageNumber = rand.Next(0, 4); // (0 to 4 is actually 0 to 3)
                            //1% chance of getting a special message
                            string message = Helper.Translation.Get($"notifications.random-message.{messageNumber}");
                            message = message.Replace("{{villager}}", villager.displayName);
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                            return;
                        }

                        if (villager.Age == NPC.adult)
                        {
                            if (villager.datable.Value == true)
                            {
                                {

                                    string message = Helper.Translation.Get($"notifications.datable.{hearts}-heart");
                                    message = message.Replace("{{villager}}", villager.displayName);
                                    Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                                    {
                                        timeLeft = _config.NotificationDuration
                                    });
                                }
                            }
                            else
                            {
                                //NPC is not datable
                                string message = Helper.Translation.Get($"notifications.non-datable.{hearts}-heart");
                                message = message.Replace("{{villager}}", villager.displayName);
                                Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                                {
                                    timeLeft = _config.NotificationDuration
                                });
                            }

                        }
                        else if (villager.Age == NPC.child)
                        {
                            //Since the NPC is a child, pick a random message from the child messages
                            int randomMessage = rand.Next(0, 9); // 0 to 8, 9 is exclusive

                            string message = Helper.Translation.Get($"notifications.child.{randomMessage}");
                            message = message.Replace("{{villager}}", villager.displayName);
                            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.achievement_type)
                            {
                                timeLeft = _config.NotificationDuration
                            });
                        }
                    }

                    if (_config.ShowBubbles && villager.Age != NPC.child)
                    {
                        int messageNumber;

                        messageNumber = rand.Next(hearts > 2 ? 2 : 0, (int)hearts + 1);

                        string message = Helper.Translation.Get($"bubbles.{messageNumber}h.{rand.Next(1, 3)}");

                        message = message.Replace("{{farmer}}", Game1.player.Name);
                        villager.showTextAboveHead(message);
                    }
                }
            }
    }

    public class ModConfig
    {
        public bool ShowNotification { get; set; }
        public bool ShowBubbles { get; set; }
        public int NotificationDuration { get; set; } = 5000;
        public int Range { get; set; } = 25;
        public int HowMuchFriendShip { get; set; } = 3;
    }
}
