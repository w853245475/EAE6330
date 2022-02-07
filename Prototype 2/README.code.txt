What Pattern(s) did you choose to use? 

	The Command Pattern. I firstly created a CommandSystem.cs. It is attached to every moveable objects (which are the player and the box)

	Then I implemented a CommandPattern.cs abstract class, other command classes can inherit from it

	Then I created different MoveCommand.cs classes for both box and player

	When recieving a moving command, the object will create a new MoveCommand first, then its CommandSystem will Execute the command and stack it.

	When Undo, the object's CommandSystem will pop from the command list, then move the object to its last position


What pattern(s), despite them potentially fitting, did you deliberately choose not to use?

	I didn't use Flyweight Pattern. Although the Sokoban game has repeated art assets such as Wall and Box, they cost little on the performance

	
What did you learn from doing your code this way?

	The Command Pattern is very useful on Undo/Redo, it can track every object's move. However, the Command Pattern brings duplicate code.

	I have to implement classes for every movable object. Their movements are similar. 

	But I do not want to simply move the duplicated code to their abstract class. 

	Thus I would like to create another base class to include their similar behaviors. 

	Another thing I prefer is to compose. I can create another class especially for the movement, then composite it to each MoveCommand class.

	After all composition is better than inherit