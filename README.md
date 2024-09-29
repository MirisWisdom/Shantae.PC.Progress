# Shantae and the Pirate's Curse
# GOG to Steam Progress Transfer

Nimble program to transfer your *Shantae and the Pirate's Curse* save progress from the GOG (1.04g) release to the Steam (1.03) release.

<details>
<summary>Display rambling...</summary>
<blockquote>
<p>
I'd be surprised if more than 0 people actually end up using this project, though I've nevertheless released it for the benefit of humanity.
</p>
<p>
In essence, there are two official PC releases of *Shantae and the Pirate's Curse* - Steam and GOG, respectively versions 1.03 and 1.04g. I've initially played the GOG release, then wanted to replay on the Steam version for the achievements and whatnot. One would think that moving the save file across was all that's needed, but alas, that couldn't be further from the truth.
</p>
<p>
The save system is interesting - there is no consistent layout and size for the save files. This nerd sniped me into trying to figure out a way of transferring the save progress across. After iterating through a few approaches, the most pragmatic yet effective way ended up being copying a specific block of process memory from one game onto the other, and simply let the games do the saving.
</p>
<p>
This was quite a delightful return to tinkering with this kind of stuff; the final solution ended up being very simply, though the journey required a considerable amount of poking and experimenting. :)
</p>
</blockquote>
</details>

## Usage

**Notes**

- Although all 3 save slots will be transferred across, it's recommended to treat only the 2nd and 3rd slots as the ones you want to transfer 100% correctly. This is because the 1st save slot will likely end up with an incorrect play time when you save after the transfer.
- At the moment, this is only one-way -- from GOG to Steam (i.e. from 1.04g to 1.03). In theory, it should be possible to transfer the other way around.

**Steps**

1. You will need to run both the GOG (1.04g) and Steam (1.03) versions at the same time. This is because we'll rely on the games to actually save the progress. This program will read their memories and copy the data from one process to the other.
2. Both games will need to be loaded with their first save slots. If your Steam version is blank, simply start a new game and go to the first save point.
3. When you've got both games running and loaded from the first save slot, simply run this program, and the stuff should be transferred across.
4. Save your progress in the Steam version, then go to the main menu, and you should see the stuff having transferred across.

## Thanks

The [CT by JizzaBeez](https://fearlessrevolution.com/viewtopic.php?t=1135) helped during the research/prototype phase.
