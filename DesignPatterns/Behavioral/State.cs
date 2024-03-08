namespace DesignPatterns.Behavioral;

public class State
{
    [Fact]
    public void Execute()
    {
        var television = new Television();

        Assert.IsType<TelevisionOffState>(television.State);
        Assert.False(television.Powered);
        Assert.Null(television.Volume);
        Assert.Null(television.Muted);

        television.LowerVolume();
        // Nothing should changes when the television is off.
        Assert.IsType<TelevisionOffState>(television.State);
        Assert.False(television.Powered);
        Assert.Null(television.Volume);
        Assert.Null(television.Muted);

        television.IncreaseVolume();
        Assert.IsType<TelevisionOffState>(television.State);
        Assert.False(television.Powered);
        Assert.Null(television.Volume);
        Assert.Null(television.Muted);

        television.ToggleMute();
        Assert.IsType<TelevisionOffState>(television.State);
        Assert.False(television.Powered);
        Assert.Null(television.Volume);
        Assert.Null(television.Muted);


        // Let's power it up.
        television.TogglePower();
        Assert.IsType<TelevisionOnState>(television.State);
        Assert.True(television.Powered);
        Assert.Equal(2, television.Volume);
        Assert.False(television.Muted);

        television.LowerVolume();
        television.LowerVolume();
        television.LowerVolume();
        Assert.IsType<TelevisionOnState>(television.State);
        Assert.True(television.Powered);
        Assert.Equal(0, television.Volume);
        Assert.False(television.Muted);

        television.IncreaseVolume();
        television.IncreaseVolume();
        television.IncreaseVolume();
        Assert.IsType<TelevisionOnState>(television.State);
        Assert.True(television.Powered);
        Assert.Equal(3, television.Volume);
        Assert.False(television.Muted);

        television.ToggleMute();
        Assert.IsType<TelevisionMutedState>(television.State);
        Assert.True(television.Powered);
        Assert.Equal(3, television.Volume);
        Assert.True(television.Muted);

        television.IncreaseVolume();
        Assert.IsType<TelevisionOnState>(television.State);
        Assert.True(television.Powered);
        Assert.Equal(4, television.Volume);
        Assert.False(television.Muted);

        television.ToggleMute();
        television.ToggleMute();
        Assert.IsType<TelevisionOnState>(television.State);
        Assert.True(television.Powered);
        Assert.Equal(4, television.Volume);
        Assert.False(television.Muted);

        television.TogglePower();
        Assert.IsType<TelevisionOffState>(television.State);
        Assert.False(television.Powered);
        Assert.Null(television.Volume);
        Assert.Null(television.Muted);
    }

    /// <summary>
    /// Context
    /// </summary>
    /// <remarks>
    /// - Defines the interface of interest to clients.
    /// - Maintains an instance of a Concrete State subclass that defines the current state.
    /// </remarks>
    public class Television
    {
        public const int InitialVolume = 2;
        public const int MaximumVolume = 10;
        public const int MinimumVolume = 0;


        public Television()
        {
            // Default state is off
            SetState(new TelevisionOffState());
        }


        public bool? Muted => State.Muted;

        public bool Powered => State.Powered;

        public TelevisionState State { get; private set; }

        public int? Volume => State.Volume;


        public void IncreaseVolume() => State.IncreaseVolume();

        public void LowerVolume() => State.LowerVolume();

        public void SetState(TelevisionState state)
        {
            state.Television = this;
            State = state;
        }

        public void ToggleMute() => State.ToggleMute();

        public void TogglePower() => State.TogglePower();
    }

    /// <summary>
    /// State
    /// </summary>
    /// <remarks>
    /// Defines an interface for encapsulating the behavior associated with a particular state of the Context.
    /// </remarks>
    public abstract class TelevisionState
    {
        public bool? Muted { get; protected set; }

        public bool Powered { get; protected set; }

        public Television Television { get; set; }

        public int? Volume { get; protected set; }


        public abstract void IncreaseVolume();

        public abstract void LowerVolume();

        public abstract void ToggleMute();

        public abstract void TogglePower();


        protected void SetState(TelevisionState state)
        {
            Television.SetState(state);
        }
    }

    /// <summary>
    /// Concrete State
    /// </summary>
    /// <remarks>
    /// Each subclass implements a behavior associated with a state of Context.
    /// </remarks>
    public class TelevisionOffState : TelevisionState
    {
        public override void IncreaseVolume()
        {
            // Television is off
        }

        public override void LowerVolume()
        {
            // Television is off
        }

        public override void ToggleMute()
        {
            // Television is off
        }

        public override void TogglePower()
        {
            SetState(new TelevisionOnState());
        }
    }

    /// <summary>
    /// Concrete State
    /// </summary>
    /// <remarks>
    /// Each subclass implements a behavior associated with a state of Context.
    /// </remarks>
    public class TelevisionOnState : TelevisionState
    {
        public TelevisionOnState()
        {
            Volume = Television.InitialVolume;
            Powered = true;
            Muted = false;
        }

        public TelevisionOnState(int? volume)
        {
            Volume = Normalize(volume);
            Powered = true;
            Muted = false;
        }


        public override void IncreaseVolume()
        {
            if (Volume < Television.MaximumVolume)
            {
                Volume++;
            }
        }

        public override void LowerVolume()
        {
            if (Volume > Television.MinimumVolume)
            {
                Volume--;
            }
        }

        public override void ToggleMute()
        {
            SetState(new TelevisionMutedState(Volume));
        }

        public override void TogglePower()
        {
            SetState(new TelevisionOffState());
        }


        private static int Normalize(int? volume) =>
            // MinimumVolume <= (volume ?? InitialVolume) <= MaximumVolume
            Math.Max(Television.MinimumVolume, Math.Min(Television.MaximumVolume, volume ?? Television.InitialVolume));
    }

    /// <summary>
    /// Concrete State
    /// </summary>
    /// <remarks>
    /// Each subclass implements a behavior associated with a state of Context.
    /// </remarks>
    public class TelevisionMutedState : TelevisionOnState
    {
        public TelevisionMutedState(int? volume)
            : base(volume)
        {
            Muted = true;
        }


        public override void IncreaseVolume()
        {
            base.IncreaseVolume();
            ToggleMute();
        }

        public override void LowerVolume()
        {
            base.LowerVolume();
            ToggleMute();
        }

        public override void ToggleMute()
        {
            SetState(new TelevisionOnState(Volume));
        }
    }
}