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

class Progress
{
    public Game Game  { get; set; } = new Game();
    public int Start  { get; set; } = 0;
    public int End    { get; set; } = 0;
    public int Length => (End - Start) * 5; // cover 3 save slots

    public static class Factory
    {
        public static Progress Create(Game game)
        {
            return game.Version switch
            {
                Version.GOG => new Progress
                {
                    Game  = game,
                    Start = Offsets.START_104g,
                    End   = Offsets.END_104g,
                },
                Version.Steam => new Progress
                {
                    Game  = game,
                    Start = Offsets.START_103,
                    End   = Offsets.END_103,
                },
                _ => throw new ArgumentException("Invalid Game Version specified."),
            };
        }
    }
}
