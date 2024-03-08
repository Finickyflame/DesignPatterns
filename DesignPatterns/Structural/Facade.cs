namespace DesignPatterns.Structural;

public class Facade
{
    [Fact]
    public void Execute()
    {
        var eventSystem = new EventSystem();

        IReadOnlyCollection<Event> events = eventSystem.FindEventsForDate(new DateTime(2018, 1, 3));
        Assert.Empty(events);

        events = eventSystem.FindEventsForDate(new DateTime(2018, 1, 10));
        Assert.NotEmpty(events);
        Assert.Equal(2, events.Count);

        Event firstEvent = events.FirstOrDefault();
        Assert.NotNull(firstEvent);
        Assert.Equal(EventType.Sport, firstEvent.EventType);
        Assert.Equal("Canadien vs Mapple Leafs", firstEvent.Title);
        Assert.Equal(new DateTime(2018, 1, 10, 12, 0, 0), firstEvent.EndsOn);

        Event secondEvent = events.ElementAtOrDefault(1);
        Assert.NotNull(secondEvent);
        Assert.Equal(EventType.Music, secondEvent.EventType);
        Assert.Equal("Bon Jovi", secondEvent.Title);
        Assert.Equal(new DateTime(2018, 1, 10, 22, 0, 0), secondEvent.EndsOn);
    }

    /* Facade */
    public class EventSystem
    {
        private readonly MusicEventSystem _musicEventSystem = new();
        private readonly SportEventSystem _sportEventSystem = new();

        public IReadOnlyCollection<Event> FindEventsForDate(DateTime date)
        {
            return _sportEventSystem.GetSportEventsForDate(date)
                .Select(Map)
                .Concat(_musicEventSystem.GetTickets(date).Select(Map))
                .OrderBy(ticket => ticket.StartsOn)
                .ToArray();
        }

        private static Event Map(SportEvent sportEvent) => new(sportEvent.MatchName, EventType.Sport, sportEvent.StartDate, sportEvent.StartDate + sportEvent.Duration);

        private static Event Map(MusicEvent musicEvent) => new(musicEvent.Band, EventType.Music, musicEvent.Start, musicEvent.End);
    }

    public class Event(string title, EventType type, DateTime startsOn, DateTime endsOn)
    {
        public DateTime EndsOn { get; } = endsOn;
        public EventType EventType { get; } = type;
        public DateTime StartsOn { get; } = startsOn;

        public string Title { get; } = title;
    }

    public enum EventType
    {
        Sport,
        Music
    }

    public class SportEventSystem
    {
        private readonly SportEvent[] _sportEvents =
        [
            new SportEvent("Canadien vs Mapple Leafs", new DateTime(2018, 1, 10, 10, 0, 0), TimeSpan.FromHours(2)),
            new SportEvent("Broncos vs Texans", new DateTime(2018, 1, 12, 16, 0, 0), TimeSpan.FromHours(3))
        ];

        public SportEvent[] GetSportEventsForDate(DateTime date)
        {
            return _sportEvents.Where(ticket => ticket.StartDate.Date == date.Date).ToArray();
        }
    }

    public class SportEvent(string matchName, DateTime startDate, TimeSpan duration)
    {
        public TimeSpan Duration { get; } = duration;

        public string MatchName { get; } = matchName;
        public DateTime StartDate { get; } = startDate;
    }


    public class MusicEventSystem
    {
        private readonly MusicEvent[] _musicEvents =
        [
            new MusicEvent("Celine Dion", new DateTime(2018, 1, 6, 18, 0, 0), new DateTime(2018, 1, 10, 22, 0, 0)),
            new MusicEvent("Bon Jovi", new DateTime(2018, 1, 10, 18, 0, 0), new DateTime(2018, 1, 10, 22, 0, 0))
        ];

        public IEnumerable<MusicEvent> GetTickets(DateTime date)
        {
            return _musicEvents.Where(musicEvent => musicEvent.Start.Date == date.Date);
        }
    }

    public class MusicEvent(string band, DateTime start, DateTime end)
    {
        public string Band { get; } = band;
        public DateTime End { get; } = end;
        public DateTime Start { get; } = start;
    }
}