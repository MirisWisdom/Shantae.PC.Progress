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

class Game
{
    public string Name     { get; set; } = string.Empty;
    public string Process  { get; set; } = string.Empty;
    public Version Version { get; set; }

    public static class Factory
    {
        public static Game Create(Version version)
        {
            return version switch
            {
                Version.GOG => new Game()
                {
                    Name    = "Shantae and the Pirate's Curse (GOG 1.04g)",
                    Process = "Shantae and the Pirate's Curse",
                    Version = Version.GOG,

                },
                Version.Steam => new Game()
                {
                    Name    = "Shantae and the Pirate's Curse (Steam 1.03)",
                    Process = "ShantaeCurse",
                    Version = Version.Steam,
                },
                _ => throw new ArgumentException("Invalid Game Version specified."),
            };
        }
    }
}
