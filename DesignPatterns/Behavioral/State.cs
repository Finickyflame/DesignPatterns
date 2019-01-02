using System;
using Xunit;

namespace DesignPatterns.Behavioral
{
    public class State : DesignPattern
    {
        public override void Execute()
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

        #region Definition

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
                this.SetState(new TelevisionOffState());
            }


            public bool? Muted => this.State.Muted;

            public bool Powered => this.State.Powered;

            public TelevisionState State { get; private set; }

            public int? Volume => this.State.Volume;


            public void IncreaseVolume() => this.State.IncreaseVolume();

            public void LowerVolume() => this.State.LowerVolume();

            public void SetState(TelevisionState state)
            {
                state.Television = this;
                this.State = state;
            }

            public void ToggleMute() => this.State.ToggleMute();

            public void TogglePower() => this.State.TogglePower();
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
                this.Television.SetState(state);
            }
        }

        #endregion

        #region Concrete Implementation

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
                this.SetState(new TelevisionOnState());
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
                this.Volume = Television.InitialVolume;
                this.Powered = true;
                this.Muted = false;
            }

            public TelevisionOnState(int? volume)
            {
                this.Volume = Normalize(volume);
                this.Powered = true;
                this.Muted = false;
            }


            public override void IncreaseVolume()
            {
                if (this.Volume < Television.MaximumVolume)
                {
                    this.Volume++;
                }
            }

            public override void LowerVolume()
            {
                if (this.Volume > Television.MinimumVolume)
                {
                    this.Volume--;
                }
            }

            public override void ToggleMute()
            {
                this.SetState(new TelevisionMutedState(this.Volume));
            }

            public override void TogglePower()
            {
                this.SetState(new TelevisionOffState());
            }


            private static int Normalize(int? volume)
            {
                // MinimumVolume <= (volume ?? InitialVolume) <= MaximumVolume
                return Math.Max(Television.MinimumVolume, Math.Min(Television.MaximumVolume, volume ?? Television.InitialVolume));
            }
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
                this.Muted = true;
            }


            public override void IncreaseVolume()
            {
                base.IncreaseVolume();
                this.ToggleMute();
            }

            public override void LowerVolume()
            {
                base.LowerVolume();
                this.ToggleMute();
            }

            public override void ToggleMute()
            {
                this.SetState(new TelevisionOnState(this.Volume));
            }
        }

        #endregion
    }
}