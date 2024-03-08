namespace DesignPatterns.Behavioral;

public class Memento
{
    [Fact]
    public void Execute()
    {
        var fileMemory = new FileMemory();

        // Set the initial state of the originator
        var file = new File
        {
            Author = "John Doe",
            Content = "",
            Title = "The mermaid"
        };

        // Save the state
        fileMemory.AddState(file.CreateState());

        FileState initialState = fileMemory.GetLastState();
        Assert.Equal(file.Content, initialState.Content);

        // Change the state of the originator
        file.Content = "Once upon a time.";

        Assert.NotEqual(file.Content, initialState.Content);

        // Restore the state to the previous one.
        file.RestoreState(initialState);

        Assert.Equal("", file.Content);
    }

    /// <summary>
    /// Originator
    /// </summary>
    /// <remarks>
    /// - Creates a memento containing a snapshot of its current internal state.
    /// - Uses the memento to restore its internal state
    /// </remarks>
    public class File
    {
        public string Author { get; set; }

        public string Content { get; set; }
        public string Title { get; set; }

        public FileState CreateState() => new(Content);

        public void RestoreState(FileState state)
        {
            Content = state.Content;
        }
    }

    /// <summary>
    /// Memento
    /// </summary>
    /// <remarks>
    /// - Stores internal state of the Originator object.
    /// - The memento may store as much or as little of the originator's internal state as necessary at its originator's
    /// discretion.
    /// </remarks>
    public class FileState(string content)
    {
        public string Content { get; } = content;
    }

    /// <summary>
    /// Caretaker
    /// </summary>
    /// <remarks>
    /// - Is responsible for the memento's safekeeping.
    /// - Never operates on or examines the contents of a memento.
    /// </remarks>
    public class FileMemory
    {
        private readonly Stack<FileState> _fileStates = [];

        public void AddState(FileState fileState)
        {
            _fileStates.Push(fileState);
        }

        public FileState GetLastState() => _fileStates.Peek();
    }
}