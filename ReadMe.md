![image](https://user-images.githubusercontent.com/55576076/235742815-f471e12a-7e11-45ee-aad4-25b1b0aa38ab.png)

### A 24/7 Jackbox Party Pack Bot.

## How does it work
The bot automatically opens the games so that players can join.
After a game players can vote for the next game, after the voting the next game gets opend.


## Project Structure
| Project | Description |
|---------|-------------|
| JackStreamBox.ConApp        | A console application used for testing the JackStreamBox.Util Library            |
| JackStreamBox.Util        |  A library containing all the functions to automatically Open, Close and Restart Jackbox Games           |
| **Todo** JackStreamBox.DiscordBot        | The discord bot that will host all the Jackbox games using the JackStreamBox.Util Library        |



## JackStreamBox.Util Public Interface
- OpenGame(Game game) -> return boolean 
- CloseGame() -> return boolean
- SetSteamPath(string path) -> return void
