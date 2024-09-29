/**
 * Copyright (C) 2024 Emilian Roman
 * 
 * This file is part of Shantae.Progress.
 * 
 * Shantae is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Shantae is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Shantae.  If not, see <https://www.gnu.org/licenses/>.
 */

namespace Miris.Shantae;

class Program
{
    static void Main(string[] args)
    {
        var transfer = new Transfer
        {
            Source = Progress.Factory.Create(Game.Factory.Create(Version.GOG)),
            Target = Progress.Factory.Create(Game.Factory.Create(Version.Steam))
        };

        Banner();

        try
        {
            transfer.Commit();
        }
        catch (Exception e)
        {
            Error(e.Message);
        }
    }

    private static void Banner()
    {
        Console.WriteLine(@"
           _____ __                __           
          / ___// /_  ____ _____  / /_____ ____ 
          \__ \/ __ \/ __ `/ __ \/ __/ __ `/ _ \
         ___/ / / / / /_/ / / / / /_/ /_/ /  __/
        /____/_/ /_/\__,_/_/ /_/\__/\__,_/\___/ 
        =========================================

              Shantae and the Pirate's Curse

        GOG (1.04g) to Steam (1.03) Save Transfer

         github: miriswisdom/shantae.pc.progress
        ");
    }

    private static void Info(string message)
    {
        Console.WriteLine(new string(' ', 8) + message);
    }

    private static void Error(string message)
    {
        var oldColour = Console.ForegroundColor;
        var newColour = ConsoleColor.DarkRed;

        Console.ForegroundColor = newColour;
        Console.WriteLine(new string(' ', 8) + message);
        Console.ForegroundColor = oldColour;
    }
}
