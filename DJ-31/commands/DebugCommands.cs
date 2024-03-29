﻿using DJ_31.Playlists;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DJ_31.Commands
{
    public class DebugCommands : ApplicationCommandModule
    {
        public ulong TidlixID = 1157774409067659334;

        [SlashCommandGroup("Debug", "NUR FÜR ENTWICKLER!!!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.Administrator)]
        public class Debug : ApplicationCommandModule
        {
            public ulong TidlixID = 1157774409067659334;
            [SlashCommand("Reload", "Lade die Commands neu")]
            public async Task Reload(InteractionContext ctx)
            {
                await ctx.DeferAsync();
                var response = new DiscordEmbedBuilder()
                {
                    Title = "DEBUG!",
                    Description = "Die Commands wurden neu geladen!",
                    Color = DiscordColor.Yellow
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(response));

                var SLC = Program.Client.GetSlashCommands();
                await SLC.RefreshCommands();
            }

            [SlashCommand("Playlist_Test", "Teste ob eine Playlist richtig funktioniert")]
            public async Task Pl_Test(InteractionContext ctx, [Option("PlaylistNr", "Welche Playlist soll getestet werden?")] long playlist)
            {
                await ctx.DeferAsync();
                // Perms
                if (ctx.User.Id != TidlixID)
                {
                    var ErrorMessage = new DiscordEmbedBuilder()
                    {
                        Title = "Fehlgeschlagen!",
                        Description = "Dieser Command ist nur für Entwickler!",
                        Color = DiscordColor.DarkRed
                    };
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(ErrorMessage));

                    return;
                }

                // Read Playlist (Debug Mode)
                var Reader = new playlistReader();
                await Reader.ReadPlaylist((int)playlist, true);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[DEBUG] Playlist-Test Complete!");

                var response = new DiscordEmbedBuilder()
                {
                    Title = "DEBUG!",
                    Description = "Die Playlist wurde getestet! (siehe Konsole)",
                    Color = DiscordColor.Yellow
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(response));
            }

            [SlashCommand("Shutdown", "Erzwingt das Abschalten des Bots auf allen Geräten!")]
            public async Task Shutdown(InteractionContext ctx)
            {
                await ctx.DeferAsync();

                // Perms
                if (ctx.User.Id != TidlixID)
                {
                    var ErrorMessage = new DiscordEmbedBuilder()
                    {
                        Title = "Fehlgeschlagen!",
                        Description = "Dieser Command ist nur für Entwickler!",
                        Color = DiscordColor.DarkRed
                    };
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(ErrorMessage));

                    return;
                }

                var response = new DiscordEmbedBuilder()
                {
                    Title = "DEBUG!",
                    Description = "Der Bot wird in wenigen Sekunden abgeschaltet!",
                    Color = DiscordColor.Yellow
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(response));

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Beep();
                Console.WriteLine("[ABSCHALTUNG!] Der Entwickler dieses Bots hat den Bot gestoppt! ");

                int i = 10;
                while (i >= 0)
                {
                    Console.Beep();
                    Console.WriteLine($"[ABSCHALTUNG!] Der Bot und diese Konsole schließen sich in: {i} Sekunden!");
                    await Task.Delay(1000);
                    i--;
                }

                await Program.Client.DisconnectAsync();
                Environment.Exit(0);
            }
        }

    }
}
