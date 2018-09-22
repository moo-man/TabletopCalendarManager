# Tabletop Calendar Manager
This application is intended to be used alongside tabletop roleplaying games in order to record in-game events. As you take notes on days as events happen, you can see the relative time between events of different adventures, giving players a better perspective of time passage. 

Data is stored in the following structure:

**Calendar**: A calendar is a collection of campaigns. This could also be called **Calendar Contents**. In the case of Calendar Manager, this also includes the structure of the calendar. 

**Campaign**: A collection of notes, including a *Start Date* and *Current Date*. When you sit down to play a game, you activate whichever campaign you are playing, and as the in-game days go by, you advance the date in the program and add notes as events occur. 

**Notes**: Text associated with a date. When you record events, they are recorded into **Notes**.

**Timers**: Timers are also a date associated with a note, however, this is used to keep track of an upcoming date. When that date arrives, you can turn the **Timer** into a **Note**.


## Example Scenario
I'm starting a new adventure with some friends, so I create a new campaign, lets just call it Adventure 1. This campaign starts in the winter of 1490, so lets just say 12/05/1490. The first session covers 5 days, some of which may have events worthy of creating **Notes** for. Therefore, we have at least 2 **Notes**, *Start Date*, which is on 12/05/1490, and the *Current Date*, which is on 12/10/1490, and there may be other, user created notes, such as *Left town* on 12/7/1490.

Next, I want to create a new adventure in the same world. This adventure takes place in the past compared with Adventure 1. Lets say 1 year and 2 months before, so the *Start Date* is 10/12/1489. On the first session, this adventure has the characters traveling a long journey across the land, which takes 3 months. However, on the way we notice something. When we land on the date 12/05/1489, the program shows us that Adventure 1 will start on this day in one year! Whether that is relevant or not depends, of course, but it puts time in perspective for your players.   

Alternatively, if this second adventure doesn't take place in the same world and shouldn't be compared with the first adventure, you can simply create a new Calendar file for it. 

## Variants
Different program variants exist for different calendar structures.

### Calendar Manager
This variant is used in conjunction with [Donjon's Fantasy Calendar Generator](https://donjon.bin.sh/fantasy/calendar/). This variant allows for GMs with their own 'homebrew' world to use this program. 

### Harptos Calendar Manager
The Calendar of Harptos is the calendar of Faerun, or the Forgotten Realms, which is the default campaign setting for Dungeons & Dragons. This includes Harptos Calendar's special intercalary holidays, as well as a graphical image of the calendar that shows where the current date is. 

### Warhammer Calendar Manager
This variant is for use in the Warhammer Fantasy world, and has the most unique features of the different variants. For example, Morrslieb, the Chaos Moon, which is erratic and unpredictable in its phase and proximity to the world. 
